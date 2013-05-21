using UnityEngine;
using System.Collections;
using XInputDotNetPure;
#if UNITY_EDITOR
	using UnityEditor;
#endif

/*
 *@author Maxens Dubois, Lafon Sylvain
 */
[AddComponentMenu("Scripts/Game/UI Manager")]
public class UIManager : myMonoBehaviour, Debugable {
	
<<<<<<< HEAD
	private Game _game;
	//private scrumController _scrumController;
=======
	private Game game;
	private ScrumController scrumController;
>>>>>>> 4ab3596edb92e06739582691b69e637199e3fb72
	public enum UIState{
		NULL,
		GameUI,
		ScrumUI,
		EndUI
	}
	public  UIState currentState{ get; set;}
	private float blueProgress;
	private float redProgress;
	
	public GameUI  gameUI;
	public ScrumUI scrumUI;
	public EndUI   endUI;
	
	void Start () 
    {
		game 				= Game.instance;
       
		blueProgress = 0f;
		redProgress  = 0f;
		
		gameUI 	= new GameUI(game);
		scrumUI = new ScrumUI(game);
		endUI 	= new EndUI(game);
		
		currentState = UIState.NULL;
	}
	
	void Update()
    {
        Gamer.initGamerId();					
		UpdateSuperProgress();
	}
	
	void UpdateSuperProgress(){

		float blueCurrent = (float)game.southTeam.SuperGaugeValue;
		float redCurrent  = (float)game.northTeam.SuperGaugeValue;
		float max		  = (float)game.settings.Global.Super.superGaugeMaximum;
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

    public static Rect screenRelativeRect(Rect r)
    {
        return screenRelativeRect(r.x, r.y, r.width, r.height);
    }
	
	void OnGUI()
    {
        switch(currentState)
		{
			case UIState.GameUI:
			{
				gameUI.DrawUI( blueProgress,  redProgress);
				break;
			}
			case UIState.EndUI:
			{
				endUI.DrawUI();
				break;
			}
			case UIState.ScrumUI:
			{		
				scrumUI.DrawUI();
				break;
			}
			default:
			{
				break;
			}
		}
	}
	public void ForDebugWindow(){
		#if UNITY_EDITOR
		EditorGUILayout.LabelField("UI State : " + this.currentState.ToString());
		#endif
	}
}
