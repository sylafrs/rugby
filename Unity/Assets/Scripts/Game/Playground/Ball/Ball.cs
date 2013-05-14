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
	public GameObject CircleDrop;
	
    public bool onGround { get; set; }
	public Vector2 multiplierDropKick = new Vector2(15.0f, 15.0f);
	public Vector2 multiplierDropUpAndUnder = new Vector2(20.0f, 10.0f);
	public float angleDropKick = 45f;
	public float angleDropUpAndUnder = 70f;
	public float randomLimitAngle = 5f;
	public float accelerationDrop = -0.75f;

	public float passSpeed = 13.0f;
	public float accelerationPass = 1.5f;

	private Unit _previousOwner;
	private Unit _nextOwner;
	private Unit _owner;

	public float lastTackle = -1;
	public float timeOnDrop = -1;
	public float timeOnPass = -1;
	private PassSystem pass;
	private DropManager drop;
	
	public Zone inZone {get; set;}
	//public Touche inTouch {get; set;}

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
            if (PreviousOwner == null)
            {
                PreviousOwner = value;
            }

            if (_owner != value)
            {
                if (_owner != null)
                {
                    PreviousOwner = _owner;
                }

                _owner = value;
				if(value) this.Taken(value);
                Game.OnOwnerChanged(PreviousOwner, value);
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

	public Unit NextOwner
	{
		get
		{
			return _nextOwner;
		}
		set
		{
			_nextOwner = value;
		}
	}
   
	new void Start(){
        onGround = false;
        base.Start();
	}
	
    public void Update()
    {
        if (Owner != null)
        {
            if (this.transform.position != Owner.BallPlaceHolderRight.transform.position &&
                this.transform.position != Owner.BallPlaceHolderLeft.transform.position && 
				this.transform.position != Owner.BallPlaceHolderTransformation.transform.position)
            {
				if ( Game.state == Game.State.TRANSFORMATION )
					this.transform.position = Owner.BallPlaceHolderTransformation.transform.position;
				else
                	this.transform.position = Owner.BallPlaceHolderRight.transform.position;
            }
                       
            this.transform.localRotation = Quaternion.identity;
        }

        if (this.transform.position.y <= 0.6f)
        {
            if (!this.onGround)
            {
                this.Game.BallOnGround(true);
            }

            this.onGround = true;
        }
        else
        {
            if (this.onGround)
            {
                this.Game.BallOnGround(false);
            }

            this.onGround = false;
        }

        UpdateTackle();
		UpdatePass();
		UpdateDrop();
    }
	
	public void Drop(DropManager.TYPEOFDROP t)
    {

		drop = new DropManager(this, t);
		drop.setupDrop();
		timeOnDrop = 0;

        Game.OnDrop();
    }

	public void UpdateDrop()
	{
		if (timeOnDrop != -1)
		{
			if (this.transform.position.y > 0.399f)
			{
				drop.doDrop(timeOnDrop);
				timeOnDrop += Time.deltaTime;
			}
			else
			{
				timeOnDrop = -1;
				this.rigidbody.isKinematic = true;
			}
		}
		if (this.Owner != null && timeOnDrop != -1)
			timeOnDrop = -1;
	}

	public void Pass(Unit to)
	{
		//Game.right.But
		
		Game.OnPass(this.Owner, to);

		pass = new PassSystem(Game.right.But.transform.position, Game.left.But.transform.position, this.Owner, to, this);
		pass.CalculatePass();
		timeOnPass = 0;
	}

	public void UpdatePass()
	{
		if (timeOnPass != -1)
		{
			if (this.transform.position.y > 0.6f)
			{
                pass.DoPass(timeOnPass);
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

    private void setPosition(Vector3 v)
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

    private void Taken(Unit u)
    {		
		
		Debug.Log("i take the ball " + u.name);

		this.rigidbody.useGravity = false;
		this.rigidbody.isKinematic = true;
		this.transform.parent = u.BallPlaceHolderRight.transform;
		this.transform.localPosition = Vector3.zero;

		
	}

    public bool RightSide()
    {
        return this.transform.position.x < 0;
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

    public void OnTackle(Unit tackler, Unit tackled)
    {
        if(lastTackle == -1)
            lastTackle = Time.time;
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
                    Game.OnScrum();
                    //goScrum = true;
					//Debug.Log("Scruuum");
                }else{
					//goScrum = false;
				}
            }
        }
    }
	
}
