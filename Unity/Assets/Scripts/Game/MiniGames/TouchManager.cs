using UnityEngine;
using System.Collections.Generic;
using XInputDotNetPure;

/**
  * @class TouchManager
  * @brief Mini jeu de la touche.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
[AddComponentMenu("Scripts/MiniGames/Touch")]
public class TouchManager : myMonoBehaviour {
		
    private Game    game;                                       // The main class                                           (reference)
    
    public System.Action<TouchManager.Result, int> CallBack;    // Action to compute after the touch                        (parameter)

	public  Gamer   gamerTouch      { get; set; }               // The gamer who pass                                       (parameter)
	public  Gamer   gamerIntercept  { get; set; }               // The gamer who tries to take the ball                     (parameter)
    public  float   minTime         { get; set; }               // The minimum time the intercepter has                     (parameter)
      
    public  int     touchChoice     { get; private set; }       // Choice of gamerTouch                                     (variable, readonly)
    public  int     interChoice     { get; private set; }       // Choice of gamerIntercept                                 (variable, readonly)
    public  int     nTouches        { get; private set; }       // The number of buttons allowed                            (variable, readonly)


    public float timerResult;
    public float remainingTime;
    private Result res;

	public void OnEnable() {
        game = Game.instance;

        touchChoice = 0;
        interChoice = 0;
		
        nTouches = game.settings.Inputs.touch.Length;

        res = Result.PLAYING;

        if (gamerTouch == null)
            touchChoice = Random.Range(0, nTouches) + 1;
			
		if(gamerIntercept == null)
            interChoice = Random.Range(0, nTouches) + 1;
	}

	public void Update() {
        if (res == Result.PLAYING)
        {
            for (int i = 0; i < nTouches; i++)
            {
                if (gamerTouch != null)
                {
                    if (Input.GetKeyDown(game.settings.Inputs.touch[i].keyboard(gamerTouch.Team)) || (gamerTouch.XboxController.GetButtonDown(game.settings.Inputs.touch[i].xbox)))
                    {
                        touchChoice = i + 1;
                    }
                }

                if (gamerIntercept != null)
                {
                    if (Input.GetKeyDown(game.settings.Inputs.touch[i].keyboard(gamerIntercept.Team)) || (gamerIntercept.XboxController.GetButtonDown(game.settings.Inputs.touch[i].xbox)))
                    {
                        interChoice = i + 1;
                    }
                }
            }

            if (touchChoice != 0 && interChoice != 0)
            {
                res = (Result)(CompareTo((Choice)touchChoice, (Choice)interChoice) + 2);
                Debug.Log("End touch " + res);
                remainingTime = timerResult;
            }
        }

        if (res != Result.PLAYING)
        {
            //Debug.Log("rem : " + remainingTime);
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0)
            {
                if (res == Result.DRAW)
                {
                    Debug.Log("Rebooooot !");
                    this.OnEnable();
                }
                else
                {
                    Debug.Log("end");
                    this.enabled = false;

                    if (CallBack != null)
                        CallBack(res, touchChoice - 1);
                }
            }
        }
	}
		
	public enum Result {
        PLAYING,        // 0
		TOUCH,          // 1
        DRAW,           // 2
		INTERCEPTION    // 3
	}

    public enum Choice
    {
        NULL,
        PIERRE,
        FEUILLE,
        CISEAUX
    }

    public int CompareTo(Choice a, Choice b)
    {
        if (a == Choice.NULL || b == Choice.NULL)
            return 0;

        if (a == b)        
            return 0;        

        if (a == Choice.PIERRE)        
            return (b == Choice.FEUILLE) ? -1 : 1;        

        if (a == Choice.FEUILLE)        
            return (b == Choice.CISEAUX) ? -1 : 1;        

        //if (a == Choice.CISEAUX)        
            return (b == Choice.PIERRE) ? -1 : 1;        
    }
	
	public Result GetResult() {
        return res;
	}
}