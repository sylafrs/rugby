using UnityEngine;
using System.Collections;

/*
 *@author Maxens Dubois 
 */
[AddComponentMenu("Scripts/Game/Scrum Controller")]
public class scrumController : MonoBehaviour {

	public GameObject camPos;
	
	private Game _game;
	private Ball _ball;
	
	private int  playerScore;
	private bool playerSpecial;
	private int cpuScore;
	private bool inScrum;
	private	int currentFrameWait;
	private int frameToGo;
	
	/**  **/
	public int scoreTarget 		= 1000;
	public int playerUp	   		= 15;
	public int specialLuck 		= 20;
	public int playerSpecialUp 	= 80;
	public int frameStart		= 30;
		
	/*
 	 *@author Maxens Dubois 
 	 */
	void Start()
    {		
		_game = gameObject.GetComponent<Game>();
		_ball = _game.Ball;
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
		frameToGo = frameStart;
    }
	
    /*
 	 *@author Maxens Dubois 
 	 */
	void Update () {
		
		if(!inScrum){
			if (_ball.getGoScrum())
			{
				inScrum = true;
				_ball.setGoScrum(false);
				_game.unlockCamera();
				Camera.mainCamera.transform.position = camPos.transform.position;
				Camera.mainCamera.transform.Translate(new Vector3(5,0,0),Space.World);
				Camera.mainCamera.transform.Rotate(new Vector3(0,90,0),Space.World);
		    }
		}
		
		if(inScrum){
			
			currentFrameWait ++;
			
			if(currentFrameWait > frameStart)
			{
				if (Input.GetKeyDown(KeyCode.RightControl))
				{
					playerUpScore(playerUp);
					if(Random.Range(1,specialLuck) == 1){
						playerSpecial = true;
					}
			    }
				if (Input.GetKeyDown(KeyCode.LeftControl))
				{
					if(playerSpecial){
						playerUpScore(playerSpecialUp);
						playerSpecial = false;
					}
			    }
				cpuScore += Random.Range(0,4);
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
	
	/*
 	 *@author Maxens Dubois 
 	 */
	void playerUpScore(int up){
		playerScore += up;
		if(playerScore > scoreTarget){
			//player Win !
			Debug.Log("player win");
			endScrum();
		}
	}
	
	/*
 	 *@author Maxens Dubois 
 	 */
	void endScrum(){
		inScrum = false;
		_game.lockCamera();
		Camera.mainCamera.transform.Translate(new Vector3(-5,0,0),Space.World);
		Camera.mainCamera.transform.Rotate(new Vector3(0,-90,0),Space.World);
        Init();
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
