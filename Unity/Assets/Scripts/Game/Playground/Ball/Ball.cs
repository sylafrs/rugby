using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * @class Ball
 * @brief Composant faisant de l'objet, une balle de rugby utilisable.
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[AddComponentMenu("Scripts/Game/Ball"), RequireComponent(typeof(Rigidbody))]
public class Ball : TriggeringTriggered, Debugable
{
	const string rootName = "ROOT";
	private Transform root;

	public Game Game;
	public Team Team
	{
		get
		{
			if (Owner != null)
			{
				return Owner.Team;
			}

			if (PreviousOwner != null)
			{
				return PreviousOwner.Team;
			}

			return null;
		}
		private set
		{

		}
	}

	public Renderer Model;
	public GameObject CircleDrop;
	public bool onGroundFired { get; set; }
	public Vector2 multiplierDropKick = new Vector2(15.0f, 15.0f);
	public Vector2 multiplierDropUpAndUnder = new Vector2(20.0f, 10.0f);
	public float angleDropUpAndUnder = 70f;
	public float randomLimitAngle = 5f;
	public float accelerationDrop = -0.75f;
	public LayerMask poteauLayer;

	public float passSpeed = 13.0f;

	private Unit _nextOwner;
	private Unit _owner;

	public float lastTackle = -1;
	public float timeOnDrop = -1;
	public float timeOnPass = -1;
	public PassSystem passManager;
	public DropManager drop;

	public Zone inZone { get; set; }
	//public Touche inTouch {get; set;}
	
	// TODO : state machine pour la ball
	//utilité : plus de facilité à utiliser, débugger
	public enum ballState{
		NULL,
		ONGROUND,
		FLYING,
		CONTROLLED
	};
	
	private ballState currentState;
	
	public void changeBallState(ballState newState){
		currentState = newState;
	}
	
	public ballState getBallState()
	{
		return currentState;
	}
	
	public Ball()
	{
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
				if (value) this.Taken(value);
				Game.OnOwnerChanged(PreviousOwner, value);
			}
		}
	}

	public Unit PreviousOwner;

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

	new void Start()
	{
		onGroundFired = false;
		root = GameObject.Find(rootName).transform;
		base.Start();
	}

	public void AttachToRoot()
	{
		this.transform.parent = root;
	}

	const float epsilonOnGround = 0.4f;

    public void TeleportOnGround()
    {
        Vector3 pos = this.transform.position;
        pos.y = 0;
        this.transform.position = pos;
    }

    public bool isOnGround()
    {
        return this.transform.position.y <= epsilonOnGround;
    }

	public void Update()
	{
		if (Owner != null)
		{
			this.transform.localRotation = Quaternion.identity;
		}

		if (this.isOnGround())
		{
			if (!this.onGroundFired)
			{
				this.Game.BallOnGround(true);
			}

			this.transform.position = new Vector3(this.transform.position.x, epsilonOnGround - 0.1f, this.transform.position.z);

			this.onGroundFired = true;
			CircleDrop.SetActive(false);
		}
		else
		{
			if (this.onGroundFired)
			{
				this.Game.BallOnGround(false);
			}

			this.onGroundFired = false;
		}


		UpdatePass();
		UpdateDrop();
	}

	public void Drop(DropManager.TYPEOFDROP t)
	{
		drop = new DropManager(this, t);
		drop.setupDrop();
		timeOnDrop = 0;
		onGroundFired = false;

		Game.OnDrop();
	}

	public bool isDroping
	{
		get
		{
			return (timeOnDrop != -1);
		}
	}

	public DropManager.TYPEOFDROP? typeOfDrop
	{
		get
		{
			if (!isDroping)
				return null;

			if (drop == null)
				return null;

			return drop.typeOfDrop;
		}
	}

    public enum DropResult
    {
        NULL, GROUND, INTERCEPTED
    }

	public void UpdateDrop()
	{
		if (timeOnDrop != -1)
		{
			if (!this.isOnGround())
			{
				drop.doDrop(timeOnDrop);
				timeOnDrop += Time.deltaTime;
			}
			else
			{
				timeOnDrop = -1;
				this.rigidbody.isKinematic = true;
				this.Game.BallOnGround(true);
				drop.afterCollision = false;
				drop.timeOffset = 0.0f;
                this.Game.OnDropFinished(DropResult.GROUND);
			}
		}

		if (this.Owner != null && timeOnDrop != -1)
		{
			timeOnDrop = -1;
			CircleDrop.SetActive(false);
			drop.afterCollision = false;
            this.Game.OnDropFinished(DropResult.INTERCEPTED);
		}
	}

	public void Pass(Unit to)
	{
        if (this.Owner == null)
        {
            return;
        }

		Game.OnPass(this.Owner, to);
		int index = (this.Owner.Team == Game.instance.southTeam ? 0 : 1);
#if UNITY_EDITOR
		Game.instance.logTeam[index].WriteLine("--------------------");
		Game.instance.logTeam[index].WriteLine("PASS");
#endif
		passManager = new PassSystem(Game.southTeam.But.transform.position, Game.northTeam.But.transform.position, this.Owner, to, this);
		passManager.CalculatePass();
		timeOnPass = 0;
		NextOwner = to;
	}

    public enum PassResult
    {
        NULL, GROUND, MANAGED, OPPONENT
    }
    
	public void UpdatePass()
	{
		int index = (this.PreviousOwner.Team == Game.instance.southTeam ? 0 : 1);
		if (timeOnPass != -1)
		{
			if (!this.isOnGround())
			{
				if (passManager.oPassState == PassSystem.passState.SETUP)
				{
					passManager.oPassState = PassSystem.passState.ONPASS;
#if UNITY_EDITOR
					Game.instance.logTeam[index].WriteLine("Pass State : " + passManager.oPassState);
#endif
				}

				passManager.GetFrom().canCatchTheBall = false;
				//Time.timeScale = 0.1f;
				passManager.DoPass(timeOnPass);
				timeOnPass += Time.deltaTime;
			}
			else
			{
				if (passManager.oPassState == PassSystem.passState.ONPASS)
				{
					passManager.oPassState = PassSystem.passState.ONGROUND;
                    this.Game.OnPassFinished(PassResult.GROUND);
#if UNITY_EDITOR
					Game.instance.logTeam[index].WriteLine("Pass State : " + passManager.oPassState);
#endif
				}

				timeOnPass = -1;
			}
		}

		if (this.Owner != null && timeOnPass != -1)
		{
			if (passManager.oPassState == PassSystem.passState.ONPASS)
			{
				passManager.oPassState = PassSystem.passState.ONTARGET;
#if UNITY_EDITOR
				Game.instance.logTeam[index].WriteLine("Pass State : " + passManager.oPassState);
#endif
                if (this.Owner.Team == this.PreviousOwner.Team)
                {
                    this.Game.OnPassFinished(PassResult.MANAGED);
                }
                else
                {
                    this.Game.OnPassFinished(PassResult.OPPONENT);
                }


				passManager.GetFrom().canCatchTheBall = true;
				NextOwner = null;
			}

			timeOnPass = -1;
		}

		if (timeOnPass == -1 && passManager != null && passManager.oPassState != PassSystem.passState.NONE)
		{
			Time.timeScale = 1f;
			passManager.oPassState = PassSystem.passState.NONE;
#if UNITY_EDITOR
			Game.instance.logTeam[index].WriteLine("Pass State : " + passManager.oPassState + "\n");
#endif
			//Game.instance.logTeam[index].CloseFile();
		}

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
			v.y = epsilonOnGround;
		}

		this.AttachToRoot();
		this.transform.position = v;
		this.rigidbody.useGravity = true;
		this.rigidbody.isKinematic = false;
		this.rigidbody.velocity = Vector3.zero;
		this.transform.rotation = Quaternion.identity;
		this.Owner = null;
	}

	private void Taken(Unit u)
	{



		this.rigidbody.useGravity = false;
		this.rigidbody.isKinematic = true;
		this.transform.parent = u.BallPlaceHolderRight.transform;
		this.transform.localPosition = Vector3.zero;


	}

	public bool RightSide()
	{
		return this.transform.position.x < 0;
	}

	public List<Unit> scrumFieldUnits = new List<Unit>();
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
				if (!scrumFieldUnits.Contains(u))
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
		if (lastTackle == -1)
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
					if (scrumFieldUnits[i].Team == Game.southTeam)
						right++;
					else
						left++;
				}

				// TODO cte : 3 --> nb de joueurs de chaque equipe qui doivent etre dans la zone
				if (right >= 3 && left >= 3)
				{
					Game.OnScrum();
					//goScrum = true;
					//
				}
				else
				{
					//goScrum = false;
				}
			}
		}
	}

	public void ForDebugWindow()
	{
#if UNITY_EDITOR
		EditorGUILayout.Toggle("On ground", isOnGround());
		if (this.passManager != null)
		{
			EditorGUILayout.EnumMaskField("Pass state", this.passManager.oPassState);
		}
		else
		{
			EditorGUILayout.LabelField("Pass state", "null");
		}
		EditorGUILayout.ObjectField("Owner (unit)", this.Owner, typeof(Unit), true);
		EditorGUILayout.ObjectField("Owner (team)", this.Team, typeof(Team), true);
		EditorGUILayout.ObjectField("Ball", this, typeof(Ball), true);
#endif
	}
}
