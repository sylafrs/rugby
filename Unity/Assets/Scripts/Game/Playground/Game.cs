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
[
    AddComponentMenu("Scripts/Game/Game"),
    RequireComponent(typeof(IntroManager)),
    RequireComponent(typeof(scrumController)),
    RequireComponent(typeof(TackleManager)),
    RequireComponent(typeof(TouchManager)),
    RequireComponent(typeof(TransformationManager))
]
/*
 *
 * B
 *  E
 *   S
 *    T
 *   
 *      S
 *       C
 *        R
 *         I
 *          P
 *           T
 * 
 *             N
 *              A
 *               M
 *                E
 *                  
 *                  E
 *                   V
 *                    E
 *                     R
 * 
 */
public class Game : myMonoBehaviour {
	
	public enum State {
        NULL = 0,
        INTRODUCTION,
		PAUSED,
		PLAYING,
		TOUCH,
		SCRUM,
		TRANSFORMATION,
        TACKLE,
        END
	}

    private State _state = State.PAUSED;
    public State state
    {
        get
        {
            return _state;
        }
        set
        {
			State old = _state;
			_state = value;
            cameraManager.sm.event_GameStateChanged(old, value);             
        }
    }

    public XboxInputs xboxInputs;
    public GameSettings settings;
	public CameraManager cameraManager;
    public IntroManager introManager;

    public Renderer ScrumBloc;

    public GameObject limiteTerrainNordEst;
    public GameObject limiteTerrainSudOuest;

    public Team right;
    public Team left;

    public Team opponent(Team t)
    {
        if (t == right) return left;
        if (t == left) return right;
        return null;
    }
    
	//a state for the camera
	enum gameState{
		NORMAL,
		SCRUMING,
		THROW_IN,
		PASSING
	}
	
	
    public Gamer p1 {get; private set;}
	public Gamer p2 {get; private set;}

    public Ball Ball;
    
	private gameState _gameState;
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
            this.left.OnOwnerChanged();
            this.right.OnOwnerChanged(); 
        }
    }

	public bool cpu = false;
	public KeyCode disableIAKey;
	public bool tweakMode;
   	
    private bool cameraLocked;
    
	public Arbiter arbiter;
	
	public void Start ()
    {
		arbiter.Game = this;
		
        right.Game = this;
        left.Game = this;
        right.right = true;
        left.right = false;
        right.CreateUnits();
        left.CreateUnits();

        arbiter.StartPlacement();

        right.opponent = left;
        left.opponent = right;

        p1 = right.gameObject.AddComponent<Gamer>();
        p1.Game = this;
        p1.Team = right;
        p1.Controlled = right[p1.Team.nbUnits/2];
        p1.Controlled.IndicateSelected(true);
        p1.Inputs = settings.inputs;

        if (!cpu)
        {
            p2 = left.gameObject.AddComponent<Gamer>();
            p2.Game = this;
            p2.Team = left;
            p2.Controlled = left[0];
            p2.Controlled.IndicateSelected(true);
            p2.Inputs = settings.inputs2;
        }

        this.Owner = p1.Controlled.Team;
        Ball.Game = this;
        Ball.transform.parent = p1.Controlled.BallPlaceHolderRight.transform;
        Ball.transform.localPosition = Vector3.zero;
        Ball.Owner = p1.Controlled;        
      
		this.cameraLocked = true;
				       
        introManager.OnFinish = () => {
            
            this._disableIA = true;
			state = State.PLAYING;
			
            Thread t = new Thread(() => {
                Thread.Sleep((int)(settings.timeToSleepAfterIntro * 1000));
                this._disableIA = false;
            });

            t.Start();
            arbiter.OnStart();
        };

        state = State.INTRODUCTION;
        introManager.enabled = true;
    }
       
    void Update()
    {
        if (Input.GetKeyDown(p1.Inputs.enableIA.keyboard) || xboxInputs.controllers[(int)p1.playerIndex].GetButtonDown(p1.Inputs.enableIA.xbox))
        {
            disableIA = !disableIA;   
        }
    }
	
	/*
	 * @ author Maxens Dubois
	 */
	public void unlockCamera(){
		this.cameraLocked = false;
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	public bool getCameraLocked(){
		return this.cameraLocked;
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	public void lockCamera(){
		this.cameraLocked = true;
	}

    public void OnScrum()
    {
        arbiter.OnScrum();
    }
    
    public void OnDrop()
    {
        cameraManager.sm.event_Drop();
    }
	
	public void OnEssai() {
		arbiter.OnEssai();
	}

    public void OnPass(Unit from, Unit to)
    {
        cameraManager.sm.event_Pass(from, to);
    }

    public void OnDropTransformed(But but)
    {
        cameraManager.sm.event_DropTransformed(but);
        arbiter.OnDropTransformed(but);
    }

    public void OnOwnerChanged(Unit before, Unit after)
    {
        cameraManager.sm.event_NewOwner(before, after);

		if (after != null)
        {
            if (after.Team != Owner)
            {
                Owner = after.Team;				
            }

            // PATCH
            // p1.controlled = after;
            if (after.Team == right)
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
        
        this.left.OnOwnerChanged();
        this.right.OnOwnerChanged();       
    }

    public void OnSuper(Team team, SuperList super)
    {
        cameraManager.sm.event_Super(team, super);
    }

    /**
     * @author Sylvain Lafon
     * @brief Se déclenche quand il y a plaquage
     */
    public void OnTackle(Unit tackler, Unit tackled)
    {
        this.arbiter.OnTackle(tackler, tackled);
        this.cameraManager.sm.event_Tackle();       
    }

    public void BallOnGround(bool onGround)
    {
        cameraManager.sm.event_BallOnGround(onGround);
    }

    public void OnBallOut()
    {
        cameraManager.sm.event_BallOut();
        arbiter.OnBallOut();
    }

    public void Reset()
    {
        SceneReloader.Go();
    }
}
