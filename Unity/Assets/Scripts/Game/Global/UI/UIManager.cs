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
	
	private Game game;

    public enum UIState{
		NULL,
		GameUI,
		ScrumUI,
		TouchUI,
		EndUI
	}
	public  UIState currentState{ get; set;}
	private float blueProgress;
	private float redProgress;
	
	public GameUI  gameUI;
	public ScrumUI scrumUI;
	public EndUI   endUI;
	public TouchUI touchUi;
	
	void Start () 
    {
		game 				= Game.instance;
       
		blueProgress = 0f;
		redProgress  = 0f;
		
		gameUI 	= new GameUI(game);
		scrumUI = new ScrumUI(game);
		endUI 	= new EndUI();//game);
		touchUi = new TouchUI(game);
		
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

    UIState previousState = UIState.NULL;

    void OnBeginGUI(UIState state)
    {
        if (state == UIState.GameUI)
        {
            game.settings.UI.GameUI.MainPanel.gameObject.SetActive(true);
        }
    }
    
    void OnEndGUI(UIState state)
    {
        if (state == UIState.GameUI)
        {
            game.settings.UI.GameUI.MainPanel.gameObject.SetActive(false);
        }
    }
	
	void OnGUI()
    {
        if (previousState != currentState)
        {
            if (previousState != UIState.NULL)
            {
                OnEndGUI(previousState);
            }
            
            if (currentState != UIState.NULL)
            {
                OnBeginGUI(currentState);
            }

            previousState = currentState;
        }

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
			case UIState.TouchUI:
			{		
				touchUi.DrawUI();
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
