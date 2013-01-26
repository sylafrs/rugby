using UnityEngine;
using System.Collections;

/*
 *@author Maxens Dubois 
 */
public class scrumController : MonoBehaviour {

	public GameObject camPos;
	
	private Game _game;
	
	private int  playerScore;
	private bool playerSpecial;
	private int cpuScore;
	private bool inScrum;
	
	/** a tweaker **/
	public int scoreTarget 		= 1000;
	public int playerUp	   		= 15;
	public int specialLuck 		= 20;
	public int playerSpecialUp 	= 80;
	
	/*
 	 *@author Maxens Dubois 
 	 */
	void Start(){
		playerScore  = 0;
		cpuScore	 = 0;
		inScrum		 = false;
		playerSpecial = false;
		_game = gameObject.GetComponent<Game>();
	}
	
    /*
 	 *@author Maxens Dubois 
 	 */
	void Update () {
		
		if(!inScrum){
			if (Input.GetKeyDown(KeyCode.M))
			{
				inScrum = true;
				//Debug.Log("lock : "+_game.getCameraLocked());
				_game.unlockCamera();
				//delock la caméra, puis la positionner
				Camera.mainCamera.transform.position = camPos.transform.position;
				Camera.mainCamera.transform.Translate(new Vector3(5,0,0),Space.World);
				Camera.mainCamera.transform.Rotate(new Vector3(0,90,0),Space.World);
		    }
		}
		
		if(inScrum){	
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
		}
	}
	
	void playerUpScore(int up){
		playerScore += up;
		if(playerScore > scoreTarget){
			//player Win !
			Debug.Log("player win");
			endScrum();
		}
	}
	
	void endScrum(){
		inScrum = false;
		_game.lockCamera();
		Camera.mainCamera.transform.Translate(new Vector3(-5,0,0),Space.World);
		Camera.mainCamera.transform.Rotate(new Vector3(0,-90,0),Space.World);
	}
	
	void OnGUI(){
		if(inScrum){
			GUI.Label(new Rect(0, 0, 150, 150),  "Player score : "+playerScore);
			GUI.Label(new Rect(0, 50, 150, 150),  "Player Special : "+playerSpecial);
			GUI.Label(new Rect(0, 100, 150, 150), "CPU score    : "+cpuScore);
		}
	}
}
