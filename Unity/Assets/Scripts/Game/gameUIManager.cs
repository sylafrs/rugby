using UnityEngine;
using System.Collections;


/*
 *@author Maxens Dubois 
 */
public class gameUIManager : MonoBehaviour {
	
	
	private Game _game;
	private Team red;
	private Team blue;
	
	public float gameTime;
	
	private float  timeElapsed;
	private bool   over;

	void Start () {
		_game 		= gameObject.GetComponent<Game>();
		red 		= GameObject.FindGameObjectWithTag("redTeam").GetComponent<Team>();
		blue 		= GameObject.FindGameObjectWithTag("blueTeam").GetComponent<Team>();
		timeElapsed = 0f;
		over		= false;
	}
	
	void Update(){
		timeElapsed += Time.deltaTime;
		
		if(timeElapsed > gameTime){
			over = true;
			Debug.Log("Time out !");
			_game.unlockCamera();
			//stuff sur la caméra
		}
	}
	
	void OnGUI(){
		int offset = 200;
		if(!over){
			GUI.Label(new Rect(0+offset, 0, 150+offset, 150),  "Blue : "+blue.nbPoints);
			GUI.Label(new Rect(150+offset, 0, 150+offset, 150),  "Time : "+timeElapsed);
			GUI.Label(new Rect(300+offset, 0, 150+offset, 150), "Red    : "+red.nbPoints);
		}else{
			string result = "";
			if(blue.nbPoints > red.nbPoints){
				result = "You loose ...";
			}else if (red.nbPoints > blue.nbPoints){
				result = "You win !";
			}else{
				result = "Draw !";
			}
			GUI.Label(new Rect(0+offset, 0+offset, 150+offset, 150+offset), result);
		}
	}
}
