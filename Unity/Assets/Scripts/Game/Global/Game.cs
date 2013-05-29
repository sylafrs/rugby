using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System.Threading;

/**
 * @class Game
 * @brief Classe principale du jeu
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[AddComponentMenu("Scripts/Game/Game")]
public class Game : myMonoBehaviour {

    public bool UseFlorianIA = true;
	public bool alwaysScrum = false;

    public GamePlaySettings settings;
    public GameReferences refs;

    private static Game _instance;
    public static Game instance
    {
        get
        {
            if (_instance == null)
            {
                return _instance = (GameObject.FindObjectOfType(typeof(Game)) as Game);
            }

            return _instance;
        }
    }
    
    public Ball Ball { get { return refs.gameObjects.ball; } }
    private Gamer p1, p2;
	private Team Owner;
	
    private bool _disableIA = false;
    public bool disableIA
    {
        get
        {
            return _disableIA;
        }
        set
        {
            _disableIA = value;
            this.northTeam.OnOwnerChanged();
            this.southTeam.OnOwnerChanged(); 
        }
    }

	public float largeurTerrain;
	public float section;
	public float xNE;
	public float xSO;

	public System.Random rand = new System.Random();

    public Team northTeam { get { return refs.north; } }
    public Team southTeam { get { return refs.south; } }
    public Referee Referee { get { return refs.Referee; } }
	
	public void Start ()
    {
        this.refs.xboxInputs.CheckNone();

        this.northTeam.game = this;
        this.southTeam.game = this;
        this.northTeam.south = false;
        this.southTeam.south = true;
        this.northTeam.CreateUnits();
        this.southTeam.CreateUnits();
       
        this.Referee.game = this;
        this.Referee.StartPlacement();
        
        this.northTeam.opponent = southTeam;
        this.southTeam.opponent = northTeam;

        // A changer de place
        //this.northTeam.captain.unitAnimator.AddEvent("SuperEffect", () =>
        //{
        //    this.northTeam.Super.LaunchFeedback();        
        //    return false;
        //}, UnitAnimator.SuperState, this.northTeam.captain.unitAnimator.TIME_SUPER_FX);

        // A changer de place
        this.southTeam.captain.unitAnimator.AddEvent("SuperEffect", () =>
        {
            this.southTeam.Super.LaunchFeedback();
            return false;
        }, UnitAnimator.SuperState, this.southTeam.captain.unitAnimator.TIME_SUPER_FX);
        
        this.refs.xboxInputs.Start();

        this.p1 = new Gamer(refs.south);
        this.p2 = new Gamer(refs.north);

        this.Owner = p1.Controlled.Team;
        this.Ball.Game = this;
        this.Ball.transform.parent = p1.Controlled.BallPlaceHolderRight.transform;
        this.Ball.transform.localPosition = Vector3.zero;
        this.Ball.Owner = p1.Controlled;
		
		if(alwaysScrum)
        	((GameObject.FindObjectOfType(typeof(ScrumField)) as ScrumField).collider as SphereCollider).radius = 100;
		
        this.refs.managers.intro.OnFinish = () =>
        {
            this._disableIA = true;                  
            this.Referee.OnStart();
			this.refs.stateMachine.event_OnStartSignal();
        };

        this.refs.stateMachine.SetFirstState(
            new MainState(this.refs.stateMachine, this.refs.managers.camera, this)
        );

        this.refs.managers.intro.enabled = true;

		xNE = refs.positions.limiteTerrainNordEst.transform.position.x;
		xSO = refs.positions.limiteTerrainSudOuest.transform.position.x;
		largeurTerrain = Mathf.Abs(xNE - xSO);
		section = largeurTerrain / 7f;
    }

    void Update()
    {
        p1.newFrame();
        p2.newFrame();
    }

	public void OnGameEnd(){
		this.refs.stateMachine.event_OnEndSignal();
	}
       	
    public void OnScrum()
    {
        this.refs.stateMachine.event_Scrum();
        Referee.OnScrum();        
    }
    
    public void OnDrop()
    {
        this.refs.stateMachine.event_Drop();
	}
	
	public void OnTouch(Touche t)
	{        
		this.refs.stateMachine.event_OnTouch(t);
	}
	
	public void OnTry(Zone z) {
        this.refs.stateMachine.event_Try(z);
	}

    public void OnConversionShot()
    {
        this.refs.stateMachine.event_ConversionShot();
    }

    public void OnPass(Unit from, Unit to)
    {        
        this.refs.stateMachine.event_Pass(from, to);        
    }

    public void OnConversion(But but)
    {
        this.refs.stateMachine.event_Conversion(but);
    }

    public void OnOwnerChanged(Unit before, Unit after)
    {
        this.refs.stateMachine.event_NewOwner(before, after);

		if (after != null)
        {
            if (after.Team != Owner)
            {
                Owner = after.Team;				
            }

            // PATCH
            // p1.controlled = after;
            if (after.Team == southTeam)
            {
                p1.Controlled.IndicateSelected(false);
                p1.Controlled = after;
                p1.Controlled.IndicateSelected(true);
            }
            else if (p2 != null)
            {
                p2.Controlled.IndicateSelected(false);
                p2.Controlled = after;
                p2.Controlled.IndicateSelected(true);
            }
        }
        
        this.northTeam.OnOwnerChanged();
        this.southTeam.OnOwnerChanged();       
    }

    public void OnSuper(Team team, SuperList super)
    {
        if(team.captain.unitAnimator)
            team.captain.unitAnimator.PrepareSuper();

        this.refs.stateMachine.event_Super(team, super);
    }

    /**
     * @author Sylvain Lafon
     * @brief Se déclenche quand il y a plaquage
     */
    public void OnTackle(Unit tackler, Unit tackled)
    {
        if (tackled) // < A virer plus tard et utiliser OnDodgeSuccess ?
        {
            this.refs.stateMachine.event_Tackle();
        }

        this.Referee.OnTackle(tackler, tackled);  
    }

    public void BallOnGround(bool onGround)
    {
        this.refs.stateMachine.event_BallOnGround(onGround);
    }

    public void OnBallOut()
    {
        this.refs.stateMachine.event_BallOut();
        Referee.OnBallOut();
    }

    public void Reset()
    {
        SceneReloader.Go();
    }

    public void OnDodge(Unit u)
    {
        this.refs.stateMachine.event_Dodge(u);
    }

    // Pour plus tard ?
    public void OnDodgeSuccess()
    {

    }

    public void OnDodgeFinished(Unit u)
    {
        this.refs.stateMachine.event_DodgeFinished(u);
    }

    public void OnResumeSignal(float time)
    {
        this.refs.stateMachine.event_OnResumeSignal(time);
    }

	/*
	 * Cette fonction me retourne le nombre de zone d'écart entre deux positions d'objets.
	 * Si le retour est négatif, alors "other" est à gauche de "referent"
	 * Si le retour est positif, alors "other" est à droite de "referent"
	 **/
	public int compareZoneInMap(Order.TYPE_POSITION referent, Order.TYPE_POSITION other)
	{
		return (int)referent - (int)other;
	}

	public int compareZoneInMap(GameObject referent, GameObject other)
	{
		return (int)PositionInMap(referent) - (int)PositionInMap(other);
	}

	public Order.TYPE_POSITION PositionInMap(GameObject obj)
	{

		if (obj.transform.position.x >= xSO && obj.transform.position.x < xSO + section)
			return Order.TYPE_POSITION.EXTRA_LEFT;
		else if (obj.transform.position.x >= xSO + section && obj.transform.position.x < xSO + 2 * section)
			return Order.TYPE_POSITION.LEFT;
		else if (obj.transform.position.x <= xNE && obj.transform.position.x > xNE - section)
			return Order.TYPE_POSITION.EXTRA_RIGHT;
		else if (obj.transform.position.x <= xNE - section && obj.transform.position.x > xNE - 2 * section)
			return Order.TYPE_POSITION.RIGHT;
		else if (obj.transform.position.x >= xSO + 2 * section && obj.transform.position.x < xSO + 3 * section)
			return Order.TYPE_POSITION.MIDDLE_LEFT;
		else if (obj.transform.position.x <= xNE - 2 * section && obj.transform.position.x > xNE - 3 * section)
			return Order.TYPE_POSITION.MIDDLE_RIGHT;
		return Order.TYPE_POSITION.MIDDLE;
	}
}
