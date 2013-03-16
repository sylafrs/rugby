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
public class Game : myMonoBehaviour {

    public XboxInputs xboxInputs;
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
	
	
    public Gamer p1 {get; private set;}
	public Gamer p2 {get; private set;}

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
            p2.Inputs = settings.inputs2;
        }

        this.Owner = p1.Controlled.Team;
        Ball.Game = this;
        Ball.transform.parent = p1.Controlled.BallPlaceHolderRight.transform;
        Ball.transform.localPosition = Vector3.zero;
        Ball.Owner = p1.Controlled;        
      
		this.cameraLocked = true;
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
	
	

    public void OnOwnerChanged(Unit before, Unit after)
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
                p1.Controlled.IndicateSelected(false);
                p1.Controlled = after;
                p1.Controlled.IndicateSelected(true);
            }
            else if (p2 != null)
            {
                p2.Controlled = after;
            }

            Log.Add("La balle est attrapee par l'equipe " + after.Team.Name);
        }
        
        this.left.OnOwnerChanged();
        this.right.OnOwnerChanged();       
    }

    /**
     * @author Sylvain Lafon
     * @brief Se déclenche quand il y a plaquage
     */
    public void EventTackle(Unit tackler, Unit tackled)
    {
		if (tackled != Ball.Owner)
		{			
			Ball.EventTackle(tackler, tackled);
		}
    }
}
