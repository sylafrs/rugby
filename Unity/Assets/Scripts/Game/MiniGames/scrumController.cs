using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/*
 *@author Maxens Dubois 
 */
[AddComponentMenu("Scripts/MiniGames/Scrum"),
	RequireComponent(typeof(Game))]
public class scrumController : myMonoBehaviour {

	public Camera cam;
	
	private Game 	_game;
	private Ball 	_ball;
	private Gamer 	_p1;
	private Gamer 	_p2;
	private Team	_t1;
	private Team	_t2;
	
	private float  playerScore;
	private bool playerSpecial;
	private float cpuScore;
	//private bool inScrum;
	private	int currentFrameWait;
	private int frameToGo;
	
	
	/** tweak session **/
	public int scoreTarget 		= 1000;
	public int playerUp	   		= 15;
	public float cpuUp			= 0.5f;
	public int specialLuck 		= 20;
	public int playerSpecialUp 	= 80;
	public int frameStart		= 30;
	
	public System.Action<Team> callback;
	
	public float rightGap = -27f;
	public float leftGap = 10f;
	
	//private float zGap = 0f;
	private float offset = 0f;
	
	public float IAoffset = -0.05f;
	public float playerOffset = 0.5f;
	
	public int timer = 10;
	public float timeRemaining {get; private set;}
		
	/*
 	 *@author Maxens Dubois 
 	 */
	void Start()
    {		
		_game 	= gameObject.GetComponent<Game>();
		_ball 	= _game.Ball;
		_p1 	= _game.p1;
		_p2	 	= _game.p2;
		_t1		= _game.right;
		_t2		= _game.left; 
	}

    void OnEnable()
    {
		Start();
        playerScore = 1;
        cpuScore = 1;
        currentFrameWait = 0;
        //inScrum = false;
        playerSpecial = false;
        offset = 0f;
        frameToGo = frameStart;
        timeRemaining = timer;
        _p1.stopMove();
        _game.disableIA = true;
    }

    void EndScrum()
    {
        this.enabled = false;
        _p1.enableMove();
        _game.disableIA = false;
     
        if (playerScore < cpuScore)
        {
            if (callback != null) callback(_t2);
        }
        else
        {
            if (callback != null) callback(_t1);
        }
    }
	
    /*
 	 *@author Maxens Dubois 
 	 */
	void Update () {		
		timeRemaining -= Time.deltaTime;
		if(timeRemaining < 0) {
			timeRemaining = 0;	
		}

        if (timeRemaining == 0 && playerScore != cpuScore)
        {
            EndScrum();
        }
        else
        {
            currentFrameWait++;
            playersInline(offset);
            if (currentFrameWait > frameStart)
            {                	
                // Up cpu score linearly
		        cpuScore += cpuUp*Time.deltaTime;
		        offset += IAoffset;			

                // SI Appui normal
                if (Input.GetKeyDown(_game.p1.Inputs.scrumNormal.keyboard) || _game.p1.XboxController.GetButtonDown(_game.p1.Inputs.scrumNormal.xbox))
                {
                    playerUpScore(playerUp);
                    if (Random.Range(1, specialLuck) == 1)
                    {
                        playerSpecial = true;
                    }
                }

                // SI Appui extra
                if (Input.GetKeyDown(_game.p1.Inputs.scrumExtra.keyboard) || _game.p1.XboxController.GetButtonDown(_game.p1.Inputs.scrumExtra.xbox))
                {
                    if (playerSpecial)
                    {
                        playerUpScore(playerSpecialUp);
                        playerSpecial = false;
                    }
                }
            }
            else
            {
                frameToGo--;
            }
        }
	}
		
	/*
 	 *@author Maxens Dubois 
 	 */
	void changeZpos(float rightGap, float leftGap){				
		Vector3 pos = _t2.transform.position;
		pos.z = _ball.transform.position.z + rightGap;
		_t2.transform.position = pos;
		
		pos = _t1.transform.position;
		pos.z = _ball.transform.position.z + leftGap;
		_t1.transform.position = pos;
	}
	
	/*
 	 *@author Maxens Dubois 
 	 */
	void playerUpScore(int up){
		playerScore += up*Time.deltaTime;		
		offset += playerOffset;	
	}
	
    /*
 	 *@author Maxens Dubois 
 	 */
	void playersInline(float offset){
			
		//Debug.Log("ligne !");
		
		Unit cap1 = _t1[0];
		Unit cap2 = _t2[0];
				
		for(int i = 0; i < _t1.nbUnits; i++){
			Unit u1 = _t1[i];
			Unit u2 = _t2[i];
			
			if(cap1 != u1){
                Vector3 tPos = cap1.transform.position;

                int dif = u1.Team.GetLineNumber(u1, cap1);
                float x = 3 * dif;

                u1.nma.stoppingDistance = 0;
                u1.nma.SetDestination(new Vector3(tPos.x + x, 0, tPos.z+offset));
			}
			
			if(cap2 != u2){
                Vector3 tPos = cap2.transform.position;

                int dif = u2.Team.GetLineNumber(u2, cap2);
                float x = 3 * dif;

                u2.nma.stoppingDistance = 0;
                u2.nma.SetDestination(new Vector3(tPos.x + x, 0, tPos.z+offset));
			}
		}       
	}
		
	
	/*
	 * Needed for GUI
	 * maxens dubois
	 */
	public float GetPlayerScore(){
		return playerScore;
	}
	
	public float GetCpuScore(){
		return cpuScore;
	}
	
	public bool HasPlayerSpecial(){
		return playerSpecial;
	}
	
	public int GetFrameToGo(){
		return frameToGo;
	}
}
