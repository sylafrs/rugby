using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/*
 *@author Maxens Dubois, Lafon Sylvain
 */
[AddComponentMenu("Scripts/Game/UI Manager"), RequireComponent(typeof(Game))]
public class GameUIManager : myMonoBehaviour {
	
	private Game _game;
	private scrumController _scrumController;
	
	public bool hideGameUi;
	public bool hideEndUi;
	public bool hideScrumUi;
	
	public KeyCode resetKey;
	
	//for super bars and th scrum bar
	public Texture2D emptyBar;
	public Texture2D blueBar;
	public Texture2D redBar;
	public Texture2D LBButton;
	
	public float quotientMin = 0.5f;
	public float quotientMax = 1.5f;
	
	private float blueProgress;
	private float redProgress;
	
	//GUI custom
	public GUIStyle superOkTextStyle;
	public GUIStyle gameTimeTextStyle;
	public GUIStyle gameScoreTextStyle;
	public GUIStyle timeBeforeScrumStyle;
	
	public float ScrumBarMaxDelta = 1.5f;
	
	public float timeBoxWidthPercentage   = 10;
	public float timeBoxHeightPercentage  = 5;
	public float timeBoxXPercentage		= 50;
	public float timeBoxYPercentage		= 10;
	
	public float blueGaugeBoxWidthPercentage   = 25;
	public float blueGaugeBoxHeightPercentage  = 10;
	public float blueGaugeBoxXPercentage		= 22.5f;
	public float blueGaugeBoxYPercentage		= 10;
	
	public float redGaugeBoxWidthPercentage   = 25;
	public float redGaugeBoxHeightPercentage  = 10;
	public float redGaugeBoxXPercentage		= 77.5f;
	public float redGaugeBoxYPercentage		= 10;
	
	public float scoreBoxWidthPercentage  = 20;
	public float scoreBoxHeightPercentage = 15;
	public float scoreBoxXPercentage = 50;
	public float scoreBoxYPercentage = 10;
	
	public float scrumBarBoxWidthPercentage = 50;
	public float scrumBarBoxHeightPercentage = 16;
	public float scrumBarBoxXPercentage = 50;
	public float scrumBarBoxYPercentage = 50;
	
	public float scrumSpecialBoxWidthPercentage = 50;
	public float scrumSpecialBoxHeightPercentage = 16;
	public float scrumSpecialBoxXPercentage = 50;
	public float scrumSpecialBoxYPercentage = 66;
	
	public float scrumTimeBoxWidthPercentage = 50;
	public float scrumTimeBoxHeightPercentage = 16;
	public float scrumTimeBoxXPercentage = 50;
	public float scrumTimeBoxYPercentage = 34;
	
	void Start () 
    {
		_game 		= gameObject.GetComponent<Game>();
		_scrumController = gameObject.GetComponent<scrumController>();
       
		blueProgress = 0f;
		redProgress  = 0f;
		
		hideAllUi();
	}
	
	void hideAllUi(){
		hideEndUi   = true;
		hideScrumUi = true;
		hideGameUi	= true;
	}
	
	void Update()
    {
		/*
        if (this._game.state == Game.State.INTRODUCTION)
        {
            return;
        }
        */

		GamePadState pad = GamePad.GetState(_game.p1.playerIndex);
        
        Gamer.initGamerId();					
		UpdateSuperProgress();
	}
	
	void UpdateSuperProgress(){

		float blueCurrent = (float)_game.right.SuperGaugeValue;
		float redCurrent  = (float)_game.left.SuperGaugeValue;
		float max		  = (float)_game.settings.super.superGaugeMaximum;
		blueProgress = Mathf.Clamp01(blueCurrent/max);
		redProgress  = Mathf.Clamp01(redCurrent/max);
	}
	
	/**
	  * @brief Fabrique un rectangle en fonction des dimensions de l'écran.
	  * @param x, y
	  * 	Position du rectangle (pourcentage)
	  * @param w, h
	  * 	Taille du rectangle (pourcentage)
	  * @return Rectangle contenant la position et la taille en pixels.
	  * @author Sylvain LAFON
	  */
	public static Rect screenRelativeRect(float x, float y, float w, float h) {
		
		float H = Screen.height / 100f;
		float W = Screen.width / 100f;

		return new Rect(x * W, y * H, w * W, h * H);	
	}
	
	void OnGUI()
    {
        if (!hideGameUi)
        {
			int offset = 200;
			
			//time box
			Rect timeBox = screenRelativeRect(timeBoxXPercentage- timeBoxWidthPercentage/2, 
				timeBoxYPercentage - timeBoxHeightPercentage/2, 
				timeBoxWidthPercentage, timeBoxHeightPercentage);
			
			//score box
			Rect scoreBox = screenRelativeRect(scoreBoxXPercentage - scoreBoxWidthPercentage/2,
				scoreBoxYPercentage - scoreBoxHeightPercentage/2, 
				scoreBoxWidthPercentage, scoreBoxHeightPercentage);
			
			//super gauges
			Rect blueGaugeBox = screenRelativeRect(blueGaugeBoxXPercentage - blueGaugeBoxWidthPercentage/2,
				blueGaugeBoxYPercentage - blueGaugeBoxHeightPercentage/2, 
				blueGaugeBoxWidthPercentage, blueGaugeBoxHeightPercentage);
	
			Rect blueProgressGaugeBox = screenRelativeRect(blueGaugeBoxXPercentage - blueGaugeBoxWidthPercentage/2,
				blueGaugeBoxYPercentage - blueGaugeBoxHeightPercentage/2, 
				blueGaugeBoxWidthPercentage * blueProgress,
				blueGaugeBoxHeightPercentage);
	
	        Rect redGaugeBox = screenRelativeRect(redGaugeBoxXPercentage - redGaugeBoxWidthPercentage / 2,
	            redGaugeBoxYPercentage - redGaugeBoxHeightPercentage / 2,
	            redGaugeBoxWidthPercentage, redGaugeBoxHeightPercentage);
	
			Rect redprogressGaugeBox = screenRelativeRect(redGaugeBoxXPercentage - redGaugeBoxWidthPercentage/2,
				redGaugeBoxYPercentage - redGaugeBoxHeightPercentage/2, 
				redGaugeBoxWidthPercentage * redProgress, 
				redGaugeBoxHeightPercentage);
			
			//scrum bars
			Rect scrumBarBox = screenRelativeRect(scrumBarBoxXPercentage - scrumBarBoxWidthPercentage/2,
				scrumBarBoxYPercentage - scrumBarBoxHeightPercentage/2, 
				scrumBarBoxWidthPercentage, scrumBarBoxHeightPercentage);
	
			Rect scrumRedBarBox = screenRelativeRect(scrumBarBoxXPercentage - scrumBarBoxWidthPercentage/2,
				scrumBarBoxYPercentage - scrumBarBoxHeightPercentage/2, 
				scrumBarBoxWidthPercentage, scrumBarBoxHeightPercentage);
			
			//scrum special
			Rect scrumSpecialBox = screenRelativeRect(scrumSpecialBoxXPercentage - scrumSpecialBoxWidthPercentage/2,
				scrumSpecialBoxYPercentage - scrumSpecialBoxHeightPercentage/2, 
				scrumSpecialBoxWidthPercentage, scrumSpecialBoxHeightPercentage);
			
			//Time before Scrum
			Rect scrumTimeBox = screenRelativeRect(scrumTimeBoxXPercentage - scrumTimeBoxWidthPercentage/2,
				scrumTimeBoxYPercentage - scrumTimeBoxHeightPercentage/2, 
				scrumTimeBoxWidthPercentage, scrumTimeBoxHeightPercentage);
			
			//player on left Box
			float playerLeftBoxWidth  = 25;
			float playerLeftBoxHeight = 10;	
			Rect playerLeftBox = screenRelativeRect(5 - playerLeftBoxWidth/2, 0 + playerLeftBoxHeight/2, playerLeftBoxWidth, playerLeftBoxHeight);
		
		//if(_game.state != Game.State.END)
        //{
			
			//superbars
			//blue 
			GUI.DrawTexture(blueGaugeBox, emptyBar);
			GUI.DrawTexture(blueProgressGaugeBox, blueBar);
			if(blueProgress == 1f)GUI.Label(blueGaugeBox, "SUPER READY !",superOkTextStyle);
			
			
			//red
			GUI.DrawTexture(redGaugeBox, emptyBar);
			GUI.DrawTexture(redprogressGaugeBox, redBar);
			if(redProgress == 1f)GUI.Label(redGaugeBox, "SUPER READY !",superOkTextStyle);
			
			
			//score
			GUI.Label(scoreBox, _game.right.nbPoints+"  -  "+_game.left.nbPoints,gameScoreTextStyle);
			
			//time
			GUI.Label(timeBox,  "Time : "+(int)_game.arbiter.IngameTime, gameTimeTextStyle);
			
		}
			//Gui du scrum
	/*
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
			
		//}
		/*
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
		*/
	}
}
