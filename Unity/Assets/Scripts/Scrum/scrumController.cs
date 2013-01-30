using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/*
 *@author Maxens Dubois 
 */
[AddComponentMenu("Scripts/Game/Scrum Controller")]
public class scrumController : MonoBehaviour {

	public Camera cam;
	
	private Game 	_game;
	private Ball 	_ball;
	private Gamer 	_p1;
	private Gamer 	_p2;
	private Team	_t1;
	private Team	_t2;
	
	private int  playerScore;
	private bool playerSpecial;
	private int cpuScore;
	private bool inScrum;
	private	int currentFrameWait;
	private int frameToGo;
	
	private bool btnScrumNormalReleased = true, btnScrumSpecialReleased = true;
	
	/**  **/
	public int scoreTarget 		= 1000;
	public int playerUp	   		= 15;
	public int specialLuck 		= 20;
	public int playerSpecialUp 	= 80;
	public int frameStart		= 30;
	
	public float rightGap = -27f;
	public float leftGap = 10f;
	
	private float zGap = 0f;
	private float offset = 0f;
	
	public float IAoffset = -0.05f;
	public float playerOffset = 0.5f;
	
	public KeyCode minigameButton;
	public KeyCode minigameSpecialButton;
		
	/*
 	 *@author Maxens Dubois 
 	 */
	void Start()
    {		
		_game 	= gameObject.GetComponent<Game>();
		_ball 	= _game.Ball;
		_p1 	= _game.p1;
		_p2	 	= _game.p2;
		_t1		= _game.left;
		_t2		= _game.right;
        Init();
	}
	
	/*
 	 *@author Sylvain Lafon
 	 */
    void Init()
    {
        playerScore = 0;
        cpuScore = 0;
		currentFrameWait = 0;
        inScrum = false;
        playerSpecial = false;
		offset = 0f;
		frameToGo = frameStart;
    }
	
	
    /*
 	 *@author Maxens Dubois 
 	 */
	void Update () {
		
		GamePadState pad = GamePad.GetState(_p1.playerIndex); 
		
        if (pad.IsConnected)
        {
            if (!InputSettingsXBOX.GetButton(_game.settings.XboxController.scrumNormal, pad))
            {
                btnScrumNormalReleased = true;
            }
            if (!InputSettingsXBOX.GetButton(_game.settings.XboxController.scrumExtra, pad))
            {
                btnScrumSpecialReleased = true;
            }
        }
    		
		
		if(!inScrum){
			if (_ball.getGoScrum())
			{
				inScrum = true;
				_ball.setGoScrum(false);
				
				
				_p1.stopMove();
				_game.disableIA = true;
				
				Vector3 pos = cam.transform.position;
				pos.z = _ball.transform.position.z;
				cam.transform.position = pos;

				cam.gameObject.SetActiveRecursively(true);
				cam.transform.LookAt(_ball.transform);
				
				//changeZpos(0f, 0f);
		    }
		}
		
		if(inScrum){
			
			//changeZpos(5f, 5f);
			
			currentFrameWait ++;
			playersInline(offset);	
			if(currentFrameWait > frameStart)
			{
				if(pad.IsConnected) {
					if(btnScrumNormalReleased && InputSettingsXBOX.GetButton(_game.settings.XboxController.scrumNormal, pad)) {
						btnScrumNormalReleased = false;
						
						playerUpScore(playerUp);
						if(Random.Range(1,specialLuck) == 1){
							playerSpecial = true;
						}
					}
				}
				else if (Input.GetKeyDown(minigameButton))
				{
					playerUpScore(playerUp);
					if(Random.Range(1,specialLuck) == 1){
						playerSpecial = true;
					}
			    }
				
				
				if(pad.IsConnected) {
					if(btnScrumSpecialReleased && InputSettingsXBOX.GetButton(_game.settings.XboxController.scrumExtra, pad)) {
						btnScrumSpecialReleased = false;
						
						if(playerSpecial){
							playerUpScore(playerSpecialUp);
							playerSpecial = false;
						}
					}
				}
				else if (Input.GetKeyDown(minigameSpecialButton))
				{
					if(playerSpecial){
						playerUpScore(playerSpecialUp);
						playerSpecial = false;
					}
			    }
				cpuScore += Random.Range(0,4);
				offset += IAoffset;
				if(cpuScore > scoreTarget){
					//cpu win
					Debug.Log("cpu win");
					endScrum();
				}
			}else{
				frameToGo --;
			}
		}
	}
	
	public bool isInScrum() {
		return inScrum;
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
		playerScore += up;
		if(playerScore > scoreTarget){
			//player Win !
			Debug.Log("player win");
			//Vector3 ballPos = new Vector3(0,0,_t1.transform.position.z+10);
			endScrum();
		}
		offset += playerOffset;
		//_t1.transform.Translate(new Vector3(0f,0f,1f));
		//_t2.transform.Translate(new Vector3(0f,0f,1f));
		//_ball.transform.Translate(new Vector3(0f,0f,0.5f));
	}
	
	/*
 	 *@author Maxens Dubois 
 	 */
	void endScrum(){
		inScrum = false;
		
		_p1.enableMove();
		_game.disableIA = false;
		cam.gameObject.SetActiveRecursively(false);
        Init();
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

                u1.GetNMA().stoppingDistance = 0;
                u1.GetNMA().SetDestination(new Vector3(tPos.x + x, 0, tPos.z+offset));
			}
			
			if(cap2 != u2){
                Vector3 tPos = cap2.transform.position;

                int dif = u2.Team.GetLineNumber(u2, cap2);
                float x = 3 * dif;

                u2.GetNMA().stoppingDistance = 0;
                u2.GetNMA().SetDestination(new Vector3(tPos.x + x, 0, tPos.z+offset));
			}
		}       
	}
		
	/*
 	 *@author Maxens Dubois 
 	 */
	void OnGUI(){
		if(inScrum){
			GUI.Label(new Rect(0, 0, 150, 150),  "Player score : "+playerScore);
			GUI.Label(new Rect(0, 50, 150, 150),  "Player Special : "+playerSpecial);
			GUI.Label(new Rect(0, 100, 150, 150), "CPU score    : "+cpuScore);
			GUI.Label(new Rect(0, 150, 150, 150), "Frame top go    : "+frameToGo);
		}
	}
}
