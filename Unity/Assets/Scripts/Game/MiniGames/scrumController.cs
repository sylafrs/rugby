using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/*
 *@author Maxens Dubois 
 */
[AddComponentMenu("Scripts/MiniGames/Scrum"),
	RequireComponent(typeof(Game))]
public class scrumController : myMonoBehaviour {
    
	private Game 	game;
		
	private float  playerScore;
	private bool playerSpecial;
	private float cpuScore;
	//private bool inScrum;
	private	int currentFrameWait;
	private int frameToGo;
	
	
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

    void Start()
    {
        this.game = gameObject.GetComponent<Game>();
    }


	/*	
	

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
			
	void changeZpos(float rightGap, float leftGap){				
		Vector3 pos = _t2.transform.position;
		pos.z = _ball.transform.position.z + rightGap;
		_t2.transform.position = pos;
		
		pos = _t1.transform.position;
		pos.z = _ball.transform.position.z + leftGap;
		_t1.transform.position = pos;
	}
	
	void playerUpScore(int up){
		playerScore += up*Time.deltaTime;		
		offset += playerOffset;	
	}
	
	void playersInline(float offset){
			
		//MyDebug.Log("ligne !");
		
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

    
    */
}

/*          //Gui du scrum
			if(_scrumController.enabled){
				float playerScore = (float)_scrumController.GetPlayerScore();
				float cpuScore 	  = (float)_scrumController.GetCpuScore();
				int frameToGo	  = _scrumController.GetFrameToGo();
				bool hasSpecial   = _scrumController.HasPlayerSpecial();
			
				
				//chrono
				string toGo;
				if(frameToGo > 0){
					toGo = frameToGo+" to go ...";
				}else{
					toGo = "--- GO ("+ (int)_scrumController.timeRemaining +") ---";
				}
				GUI.Label(scrumTimeBox,toGo,timeBeforeScrumStyle);
				
				//bar
				float quotient = playerScore/cpuScore;
				if(quotient < quotientMin) quotient = quotientMin;
				if(quotient > quotientMax) quotient = quotientMax;
				float blueScrumProgress = quotient - quotientMin; 
				
				Rect scrumBlueBarBox = screenRelativeRect(scrumBarBoxXPercentage - scrumBarBoxWidthPercentage/2,
					scrumBarBoxYPercentage - scrumBarBoxHeightPercentage/2, 
					scrumBarBoxWidthPercentage*blueScrumProgress, 
					scrumBarBoxHeightPercentage);

				GUI.DrawTexture(scrumBarBox, emptyBar);
				GUI.DrawTexture(scrumRedBarBox, redBar);
				GUI.DrawTexture(scrumBlueBarBox,blueBar);
				
				//special
				if(hasSpecial)GUI.DrawTexture(scrumSpecialBox, LBButton,ScaleMode.ScaleToFit);
				
				//debug
				
				GUI.Label(new Rect(0, 0, 150, 150),  "Player score : "+playerScore);
				GUI.Label(new Rect(0, 50, 150, 150),  "Player Special : "+hasSpecial);
				GUI.Label(new Rect(0, 100, 150, 150), "CPU score    : "+cpuScore);
				GUI.Label(new Rect(0, 150, 150, 150), "Frame top go    : "+frameToGo);
				
			}
 */
