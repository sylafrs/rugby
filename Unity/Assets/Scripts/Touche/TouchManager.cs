using UnityEngine;
using System.Collections.Generic;
using XInputDotNetPure;

/**
  * @class TouchManager
  * @brief Mini jeu de la touche.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class TouchManager : MonoBehaviour {
	
	public Gamer gamerTouch;
	public Gamer gamerIntercept;
	
	private int choixTouche;
	private int choixInter;

	public InputTouch [] touche;
	public InputTouch [] interception;
	private int n;
	
	private float timeLeft;
	public float minTime;
	
	public void OnEnable() {
		choixTouche = 0;
		choixInter = 0;
		timeLeft = minTime;
		n = Mathf.Min (touche.Length, interception.Length);
	}
		
	public void OnGUI() {
		
		Color c = GUI.color;
				
		if(timeLeft > 0)
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
	
	public void Update() {
		timeLeft -= Time.deltaTime;
		
		for(int i = 0; i < n; i++) {
			if(Input.GetKeyDown(touche[i].keyboard) || (gamerTouch && gamerTouch.XboxController.GetButtonDown(touche[i].xbox))) {
				choixTouche = i+1;
			}
			if(Input.GetKeyDown(interception[i].keyboard) || (gamerIntercept && gamerIntercept.XboxController.GetButtonDown(interception[i].xbox))) {
				choixInter = i+1;
			}
		}
		
		if(choixTouche != 0 && timeLeft < 0) {
			DoTouch();	
		}
	}
	
	public void DoTouch() {
		
		Debug.Log ("touche : " + choixTouche + " -- inter : " + choixInter);
		
		if(choixTouche == choixInter) {
			Debug.Log ("interception");	
		}
		else {
			Debug.Log ("reussite");	
		}
		
		this.enabled = false;
	}
}
