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
	
	public Gamer gamerTouch {get; set;}
	public Gamer gamerIntercept {get; set;}

    public bool infiniteTime;
	
	private int choixTouche;
	private int choixInter;

	public InputTouch [] touche;
	public InputTouch [] interception;
	private int n;
	
	private float timeLeft;
	public float minTime;
	
	public bool randomTouch {get; set;}
	public bool randomIntercept {get; set;}
	
	public void OnEnable() {
		choixTouche = 0;
		choixInter = 0;
		timeLeft = minTime;
		n = Mathf.Min (touche.Length, interception.Length);
		
		if(randomTouch)
			choixTouche = Random.Range(0, n) + 1;
			
		if(randomIntercept)
			choixInter = Random.Range(0, n) + 1;
	}
	
	public bool hideGui = true;
	public void OnGUI() {
		if(hideGui) {
			Color c = GUI.color;
			
			GUILayout.Space(300);
					
			if(timeLeft > 0 && !infiniteTime)
				GUILayout.Label("Choisissez une touche, il vous reste : " + ((int)timeLeft) + " secondes (minimum)");
			else
				GUILayout.Label("Choisissez une touche");
				
			GUILayout.BeginHorizontal();
				GUILayout.Label("J1 : ");
			
				for(int i = 0; i < n; i++) {
					GUI.color = (choixTouche == i+1) ? Color.red : c;
					if(GUILayout.Button (touche[i].xbox.ToString(), GUILayout.MinWidth(100))) {
						choixTouche = i+1;
					}
				}
			GUILayout.EndHorizontal();
			
			GUI.color = c;
			
			GUILayout.BeginHorizontal();
				GUILayout.Label("J2 : ");
			
				for(int i = 0; i < n; i++) {
					GUI.color = (choixInter == i+1) ? Color.red : c;
					if(GUILayout.Button (interception[i].xbox.ToString(), GUILayout.MinWidth(100))) {
						choixInter = i+1;
					}
				}
			GUILayout.EndHorizontal();
			
			GUI.color = c;
		}
	}
	
	public void Update() {
		if(!infiniteTime)
            timeLeft -= Time.deltaTime;
		
		for(int i = 0; i < n; i++) {
			if(Input.GetKeyDown(touche[i].keyboard(gamerTouch.Team)) || (gamerTouch != null && gamerTouch.XboxController.GetButtonDown(touche[i].xbox))) {
				choixTouche = i+1;
			}
			if(Input.GetKeyDown(interception[i].keyboard(gamerIntercept.Team)) || (gamerIntercept != null && gamerIntercept.XboxController.GetButtonDown(interception[i].xbox))) {
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
		
		
		
		if(choixTouche == choixInter) {
				
		}
		else {
				
		}
		
		this.enabled = false;
		
		if(CallBack != null)
			CallBack(this.GetResult(), choixTouche - 1);
	}
}
