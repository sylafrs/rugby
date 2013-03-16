using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/*
 *@author Maxens Dubois, Lafon Sylvain
 */
[AddComponentMenu("Scripts/Game/UI Manager"), RequireComponent(typeof(Game))]
public class gameUIManager : myMonoBehaviour {
	
	private Game _game;
	
	public float gameTime;
	public KeyCode resetKey;
	
	//for super bars
	public Texture2D emptyBar;
	public Texture2D blueBar;
	public Texture2D redBar;
	private float blueProgress;
	private float redProgress;
	
	
	public GUIStyle superOkTextStyle;
	
	private float  timeElapsed;
	private bool   over;
	
	private bool btnResetReleased = true;

	void Start () 
    {
		_game 		= gameObject.GetComponent<Game>();
        timeElapsed = 0f;
		over		= false;
		
		blueProgress = 0f;
		redProgress  = 0f;
	}
	
	void Update()
    {
		GamePadState pad = GamePad.GetState(_game.p1.playerIndex); 
		
        if (pad.IsConnected)
        {
            if (!InputSettingsXBOX.GetButton(_game.settings.XboxController.reset, pad))
            {
                btnResetReleased = true;
            }
            if (!InputSettingsXBOX.GetButton(_game.settings.XboxController.reset, pad))
            {
                btnResetReleased = true;
            }
        }
		
		timeElapsed += Time.deltaTime;
		
		if(timeElapsed > gameTime){
			over = true;
			Debug.Log("Time out !");
			_game.unlockCamera();
			//stuff sur la caméra
		}

        Gamer.initGamerId();		
		
		if(pad.IsConnected){
			if(btnResetReleased && InputSettingsXBOX.GetButton(_game.settings.XboxController.reset, pad)){
				btnResetReleased = false;
				Application.LoadLevel(Application.loadedLevel);
			}
		}
		else if(Input.GetKeyDown(resetKey)){
           	Application.LoadLevel(Application.loadedLevel);
		}
		
		UpdateSuperProgress();
	}
	
	void UpdateSuperProgress(){

		float blueCurrent = (float)_game.right.SuperGaugeValue;
		float redCurrent  = (float)_game.left.SuperGaugeValue;
		float max		  = (float)_game.settings.super.superGaugeMaximum;
		blueProgress = Mathf.Clamp01(blueCurrent/max);
		redProgress  = Mathf.Clamp01(redCurrent/max);
	}
	
	void OnGUI()
    {
		int offset		 = 100;
		
		if(!over)
        {
			//blue 
			GUI.Label(new Rect(0+offset, 0, 150+offset, 150),  _game.right.Name + " : " + _game.right.nbPoints);
			GUI.DrawTexture(new Rect(0+offset, 20, 150+offset, 50), emptyBar);
			GUI.DrawTexture(new Rect(0+offset, 20, (150+offset)* blueProgress, 50), blueBar);
			if(blueProgress == 1f)GUI.Label(new Rect(20+offset, 33, 150+offset, 150), "OK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",superOkTextStyle);
			
			
			//red
			GUI.Label(new Rect(400 + offset, 0, 150 + offset, 150), _game.left.Name + " : " + _game.left.nbPoints);
			GUI.DrawTexture(new Rect(400 + offset, 20, 150 + offset, 50), emptyBar);
			GUI.DrawTexture(new Rect(400+offset, 20, (150+offset)* redProgress, 50), redBar);
			if(redProgress == 1f)GUI.Label(new Rect(420+offset, 33, 150+offset, 150), "OK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",superOkTextStyle);
			
			
			//time
			GUI.Label(new Rect(270+offset, 0, 150+offset, 150),  "Time : "+timeElapsed);
			
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
