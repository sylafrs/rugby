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
	
	public System.Action<TouchManager.Result, int> CallBack;
	public Game game;
	
	public Gamer gamerTouch {get; set;}
	public Gamer gamerIntercept {get; set;}

    public bool infiniteTime;
	
	public int choixTouche, choixInter;
	
	public int n;
	
	public float timeLeft;
	public float minTime;
	
	public bool randomTouch {get; set;}
	public bool randomIntercept {get; set;}
	
	public void OnEnable() {
		choixTouche = 0;
		choixInter = 0;
		timeLeft = minTime;
		
		n = Mathf.Min (game.settings.Inputs.touch.Length, game.settings.Inputs.interception.Length);
		
		if(randomTouch)
			choixTouche = Random.Range(0, n) + 1;
			
		if(randomIntercept)
			choixInter = Random.Range(0, n) + 1;
	}
	
	public void Update() {
		if(!infiniteTime)
            timeLeft -= Time.deltaTime;
		
		for(int i = 0; i < n; i++) {
			if(Input.GetKeyDown(game.settings.Inputs.touch[i].keyboard(gamerTouch.Team)) || (gamerTouch.XboxController.GetButtonDown(game.settings.Inputs.touch[i].xbox))) {
				choixTouche = i+1;
			}
			if(Input.GetKeyDown(game.settings.Inputs.interception[i].keyboard(gamerIntercept.Team)) || (gamerIntercept.XboxController.GetButtonDown(game.settings.Inputs.interception[i].xbox))) {
				choixInter = i+1;
			}
		}

        if (choixTouche != 0 && ((timeLeft < 0 && !infiniteTime) || choixInter != 0))
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
		if(choixTouche == choixInter){
			return Result.INTERCEPTION;	
		}
		return Result.TOUCH;
	}
	
	public void DoTouch() {
		
		MyDebug.Log("touche : " + choixTouche + " -- inter : " + choixInter);
		
		if(choixTouche == choixInter) {
			MyDebug.Log("interception");	
		}
		else {
			MyDebug.Log("reussite");	
		}
		
		this.enabled = false;
		
		if(CallBack != null)
			CallBack(this.GetResult(), choixTouche - 1);
	}
}
