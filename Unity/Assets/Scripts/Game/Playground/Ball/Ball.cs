using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * @class Ball
 * @brief Composant faisant de l'objet, une balle de rugby utilisable.
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[AddComponentMenu("Scripts/Game/Ball"), RequireComponent(typeof(Rigidbody))]
public class Ball : TriggeringTriggered {
	
    public Game Game;
	public Team Team {
		get {
			if(Owner != null) {
				return Owner.Team;
			}
			if(PreviousOwner != null) {
				return PreviousOwner.Team;	
			}
			
			return null;	
		}
		private set {
			
		}
	}
	
	public Vector3 multiplierDrop = new Vector3(50.0f, 70.0f, 0.0f);
	public Vector3 multiplierPass = new Vector3(20.0f, 70.0f, 20.0f);
	public float passSpeed = 20.0f;
	public float AngleOfFOV = 0.0f;

	private Unit _previousOwner;
	private bool goScrum;
	private Unit _owner;

	public float lastTackle = -1;

	public float timeOnPass = -1;
	private PassSystem p;
	
	public Color DiscTackle = new Color(0f, 0f, 255f, 33f);
	public float sizeOfTackleArea = 2f;
	
	public Zone inZone {get; set;}
	
	public Ball() {
		inZone = null;	
	}
	
	/*
	 * @author Maxens Dubois 
	 */
    public Unit Owner
    {
        get
        {
            return _owner;
        }
        set
        {
            if (_owner != value)
            {
                PreviousOwner = _owner;
                _owner = value;
                Game.OnOwnerChanged(_owner, value);
            }         
        }
    }

    public Unit PreviousOwner
    {
        get
        {
            return _previousOwner;
        }
        private set
        {
            _previousOwner = value;
        }
    }
   
	new void Start(){

		goScrum = false;
        base.Start();
	}
	
    public void Update()
    {
        if (Owner != null)
        {
            if (this.transform.position != Owner.BallPlaceHolderRight.transform.position &&
                this.transform.position != Owner.BallPlaceHolderLeft.transform.position)
            {
                this.transform.position = Owner.BallPlaceHolderRight.transform.position;
            }
                       
            this.transform.localRotation = Quaternion.identity;
        }

        UpdateTackle();
		UpdatePass();
		
		drawCone();
    }

    public void Drop()
    {              
		Drop (this.multiplierDrop);
    }
	
	public void Drop(Vector3 multiplierDrop)
    {              
        this.transform.parent = null;
        this.rigidbody.useGravity = true;
		this.rigidbody.isKinematic = false;
        this.rigidbody.AddForce(Owner.transform.forward * multiplierDrop.x + Owner.transform.up * multiplierDrop.y + Owner.transform.right * multiplierDrop.z);
        Owner = null;
    }

	public void Pass(Unit to)
	{
		//Game.right.But
		
		Debug.LogWarning("Sylvain il faut qu'il soit possible de désactiver l'IA de groupe pour dire à la cible d'aller où je lui dis");
		p = new PassSystem(Game.right.But.transform.position, Game.left.But.transform.position, this.Owner, to, this);
		p.CalculatePass();
		timeOnPass = 0;
	}

	public void UpdatePass()
	{
		if (timeOnPass != -1)
		{
			if (this.transform.position.y > 0.6f)
			{
				p.DoPass(timeOnPass);
				timeOnPass += Time.deltaTime;
			}
			else
			{
				timeOnPass = -1;
			}
		}
		if (this.Owner != null && timeOnPass != -1)
			timeOnPass = -1;
	}

	//Poser la balle
    public void Put()
    {
        setPosition(this.transform.position);    
    }

    public void Taken(Unit u)
    {	
		Debug.Log("i take the ball " + u.name);

		this.rigidbody.useGravity = false;
		this.rigidbody.isKinematic = true;
		this.transform.parent = u.BallPlaceHolderRight.transform;
		this.transform.localPosition = Vector3.zero;

		if (Owner != u)
		{
			Owner = u;
		}
	}

    public void setPosition(Vector3 v)
    {
        if (v.y == 0)
        {
            v.y = 0.5f;
        }

        this.transform.parent = null;
        this.transform.position = v;
        this.rigidbody.useGravity = true;
        this.rigidbody.isKinematic = false;
        this.rigidbody.velocity = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        this.Owner = null;
    }

    List<Unit> scrumFieldUnits = new List<Unit>();
    public override void Entered(Triggered o, Trigger t)
    {       
        if (t.GetType() == typeof(NearBall))
        {
            Unit u = o.GetComponent<Unit>();
            if (u != null)
            {
                u.sm.event_NearBall();
            }
        }
        else if (t.GetType() == typeof(ScrumField))
        {
            Unit u = o.GetComponent<Unit>();
            if (u != null)
            {
                if(!scrumFieldUnits.Contains(u))
                    scrumFieldUnits.Add(u);
            }
        }
    }

    public override void Left(Triggered o, Trigger t)
    {
        if (t.GetType() == typeof(ScrumField))
        {
            Unit u = o.GetComponent<Unit>();
            if (u != null)
            {
                if (scrumFieldUnits.Contains(u))
                    scrumFieldUnits.Remove(u);
            }
        }
    }

    public void EventTackle(Unit tackler, Unit tackled)
    {
        if(lastTackle == -1)
            lastTackle = Time.time;
    }
	
	public bool getGoScrum(){
		return goScrum;
	}
	
	public void setGoScrum(bool state){
		goScrum = state && (Game.state == Game.State.PLAYING);
	}
	
    public void UpdateTackle()
    {       
        if (lastTackle != -1)
        {
            // TODO cte : 2 -> temps pour checker
            if (Time.time - lastTackle > 2)
            {
                lastTackle = -1;
                int right = 0, left = 0;
                for (int i = 0; i < scrumFieldUnits.Count; i++)
                {
                    if (scrumFieldUnits[i].Team == Game.right)
                        right++;
                    else
                        left++;
                }

                // TODO cte : 3 --> nb de joueurs de chaque equipe qui doivent etre dans la zone
                if (right >= 3 && left >= 3)
                {
                    goScrum = true;
					//Debug.Log("Scruuum");
                }else{
					//goScrum = false;
				}
            }
        }
    }
	
	public void drawCone()
	{
		/*
		float newAngle = AngleOfFOV * Mathf.PI / 180f;
		Vector3 source = this.Owner.transform.position;
		source.y = 1f;
		float tmp = Vector3.Angle( Vector3.forward, this.Owner.transform.forward) * Mathf.PI / 180f;
		//float alpha = Mathf.Acos( Vector3.Dot(Vector3.forward, this.Owner.transform.forward) / (Vector3.forward.magnitude * this.Owner.transform.forward.magnitude) );
		Vector3 tmp2 = new Vector3( 10f * Mathf.Sin(tmp + newAngle), 1f, 10f * Mathf.Cos(tmp + newAngle) );
		Vector3 destination = 10f * this.Owner.transform.forward;
		Debug.Log("Angle entre x et x' = " + tmp);
		//destination.y = 1f;
		Vector3 tmp3 = new Vector3( 10f * this.Owner.transform.forward.x / Mathf.Sin(newAngle) , 1f, 10f * this.Owner.transform.forward.z / Mathf.Cos(newAngle) );
		Debug.Log(this.Owner.transform.forward);
		Debug.DrawRay(source, destination, Color.yellow, 10f);
		Debug.DrawRay(source, tmp2 - source, Color.cyan, 10f);
		*/
	}
	
}
