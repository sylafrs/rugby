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
    public  bool    infiniteTime;                               // Has the player an infinite ammount of time to choose ?   (tweak)

    public System.Action<TouchManager.Result, int> CallBack;    // Action to compute after the touch                        (parameter)

	public  Gamer   gamerTouch      { get; set; }               // The gamer who pass                                       (parameter)
	public  Gamer   gamerIntercept  { get; set; }               // The gamer who tries to take the ball                     (parameter)
    public  float   minTime         { get; set; }               // The minimum time the intercepter has                     (parameter)
      
    public  int     touchChoice     { get; private set; }       // Choice of gamerTouch                                     (variable, readonly)
    public  int     interChoice     { get; private set; }       // Choice of gamerIntercept                                 (variable, readonly)
    public  float   timeLeft        { get; private set; }       // The minimum time remaining                               (variable, readonly)
    public  int     nTouches        { get; private set; }       // The number of buttons allowed                            (variable, readonly)

	public void OnEnable() {
        game = Game.instance;

        touchChoice = 0;
        interChoice = 0;
		timeLeft = minTime;

        nTouches = game.settings.Inputs.touch.Length;

        if (gamerTouch == null)
            touchChoice = Random.Range(0, nTouches) + 1;
			
		if(gamerIntercept == null)
            interChoice = Random.Range(0, nTouches) + 1;
	}

	public void Update() {
		if(!infiniteTime)
            timeLeft -= Time.deltaTime;

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

        if (touchChoice != 0 && ((timeLeft < 0 && !infiniteTime) || interChoice != 0))
        {
            DoTouch();
        }        
	}
		
	public enum Result {
		TOUCH,
		INTERCEPTION,
		PLAYING
	}
	
	public Result GetResult() {
		if(this.enabled) {
			return Result.PLAYING;	
		}
        if (touchChoice == interChoice)
        {
			return Result.INTERCEPTION;	
		}
		return Result.TOUCH;
	}
	
	public void DoTouch() {
			
		this.enabled = false;
		
		if(CallBack != null)
			CallBack(this.GetResult(), touchChoice - 1);
	}
}
