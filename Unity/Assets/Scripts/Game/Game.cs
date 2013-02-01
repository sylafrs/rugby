using UnityEngine;
using System.Collections;
using XInputDotNetPure;


/**
 * @class Game
 * @brief Classe principale du jeu
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[AddComponentMenu("Scripts/Game/Game")]
public class Game : MonoBehaviour {

    public GameSettings settings;
	public CameraManager cameraManager;

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
	
	
    public Gamer p1, p2;

    public Ball Ball;
    public GameLog Log;
    
	private gameState _gameState;
    private Team Owner;
	private bool btnIaReleased = true;
	
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
            this.left.OwnerChanged();
            this.right.OwnerChanged(); 
        }
    }

	public bool cpu = false;
	public KeyCode disableIAKey;
	public bool tweakMode;
   	
    private bool cameraLocked;
    
	public void Start ()
    {
        this.Log = this.gameObject.AddComponent<GameLog>();

        right.Game = this;
        left.Game = this;
        right.right = true;
        left.right = false;
        right.CreateUnits();
        left.CreateUnits();

        right.opponent = left;
        left.opponent = right;

        p1 = right.gameObject.AddComponent<Gamer>();
        p1.game = this;
        p1.team = right;
        p1.controlled = right[p1.team.nbUnits/2];
        p1.controlled.IndicateSelected(true);
        p1.inputs = settings.inputs;

        if (!cpu)
        {
            p2 = left.gameObject.AddComponent<Gamer>();
            p2.game = this;
            p2.team = left;
            p2.controlled = left[0];
            p2.inputs = settings.inputs2;
        }

        this.Owner = p1.controlled.Team;
        Ball.Game = this;
        Ball.transform.parent = p1.controlled.BallPlaceHolderRight.transform;
        Ball.transform.localPosition = Vector3.zero;
        Ball.Owner = p1.controlled;        
      
		this.cameraLocked = true;
    }

    void Update()
    {		
		GamePadState pad = GamePad.GetState(p1.playerIndex); 
		if (pad.IsConnected)
        {			
            if (!btnIaReleased && !InputSettingsXBOX.GetButton(this.settings.XboxController.enableIA, pad))
            {
				 btnIaReleased = true;
            }
        }
			
		if(pad.IsConnected) {
			if(btnIaReleased && InputSettingsXBOX.GetButton(this.settings.XboxController.enableIA, pad)) {
				btnIaReleased = false;				
				disableIA = !disableIA; 
			}
		} 
		
		else if (Input.GetKeyDown(disableIAKey))
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
	
	

    public void OwnerChanged(Unit before, Unit after)
    {		
		if (after != null)
        {
            if (after.Team != Owner)
            {
                Owner = after.Team;
				cameraManager.OnOwnerChanged();
            }

            // PATCH
            // p1.controlled = after;
            if (after.Team == right)
            {
                p1.controlled.IndicateSelected(false);
                p1.controlled = after;
                p1.controlled.IndicateSelected(true);
            }
            else if (p2 != null)
            {
                p2.controlled = after;
            }

            Log.Add("La balle est attrapee par l'equipe " + after.Team.Name);
        }
        
        this.left.OwnerChanged();
        this.right.OwnerChanged();       
    }

    /**
     * @author Sylvain Lafon
     * @brief Se déclenche quand il y a plaquage
     */
    public void EventTackle(Unit tackler, Unit tackled)
    {
        Ball.EventTackle(tackler, tackled);
    }
}
