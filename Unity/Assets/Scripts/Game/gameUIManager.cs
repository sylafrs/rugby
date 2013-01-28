using UnityEngine;
using System.Collections;


/*
 *@author Maxens Dubois, Lafon Sylvain
 */
[AddComponentMenu("Scripts/Game/UI Manager"), RequireComponent(typeof(Game))]
public class gameUIManager : MonoBehaviour {
	
	private Game _game;
	
	public float gameTime;
	public KeyCode resetKey;
	
	private float  timeElapsed;
	private bool   over;
	
	

	void Start () 
    {
		_game 		= gameObject.GetComponent<Game>();
        timeElapsed = 0f;
		over		= false;
	}
	
	void Update()
    {
		timeElapsed += Time.deltaTime;
		
		if(timeElapsed > gameTime){
			over = true;
			Debug.Log("Time out !");
			_game.unlockCamera();
			//stuff sur la caméra
		}
		
		if(Input.GetKeyDown(resetKey)){
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	
	void OnGUI()
    {
		int offset = 200;
		if(!over)
        {
			GUI.Label(new Rect(0+offset, 0, 150+offset, 150),  _game.right.Name + " : " + _game.right.nbPoints);
			GUI.Label(new Rect(150+offset, 0, 150+offset, 150),  "Time : "+timeElapsed);
            GUI.Label(new Rect(300 + offset, 0, 150 + offset, 150), _game.left.Name + " : " + _game.left.nbPoints);
		}
        else
        {
			string result = "";
            if (_game.right.nbPoints < _game.left.nbPoints)
            {
				result = "You loose ...";
            }
            else if (_game.left.nbPoints < _game.right.nbPoints)
            {
				result = "You win !";
			}
            else
            {
				result = "Draw !";
			}
			result += " Press the button to restart !";
			GUIStyle style = new GUIStyle();
			style.fontSize = 30;
			GUI.Label(new Rect(200+offset, 0+offset, 150+offset, 150), result, style);
			if(GUI.Button(new Rect(200+offset, 50+offset, 250+offset, 100),"restart"))
				Application.LoadLevel(Application.loadedLevel);
		}
	}
}
