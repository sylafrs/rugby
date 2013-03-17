using UnityEngine;
using System.Collections.Generic;

/**
  * @class TouchManager
  * @brief Mini jeu de la touche.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class TouchManager : MonoBehaviour {
	
	private int choixJ1;
	private int choixJ2;
	
	public int nChoix;
	
	private float timeLeft;
	public float minTime;
	
	public void OnEnable() {
		choixJ1 = 0;
		choixJ2 = 0;
		timeLeft = minTime;
	}
	
	public void OnGUI() {
		if(timeLeft > 0)
			GUILayout.Label("Choisissez une touche, il vous reste : " + ((int)timeLeft) + " secondes (minimum)");
		else
			GUILayout.Label("Choisissez une touche");
			
		GUILayout.BeginHorizontal();
			GUILayout.Label("J1 : ");
		
			for(int i = 0; i < nChoix; i++) {
				if(GUILayout.Button (i.ToString("D2"))) {
					choixJ1 = i+1;
				}
			}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
			GUILayout.Label("J2 : ");
		
			for(int i = 0; i < nChoix; i++) {
				if(GUILayout.Button (i.ToString("D2"))) {
					choixJ2 = i+1;
				}
			}
		GUILayout.EndHorizontal();
	}
	
	public void Update() {
		timeLeft -= Time.deltaTime;
		
		if(choixJ1 != 0 && timeLeft < 0) {
			DoTouch();	
		}
	}
	
	public void DoTouch() {
		
		Debug.Log ("j1 : " + choixJ1 + " -- j2 : " + choixJ2);
		
		if(choixJ1 == choixJ2) {
			Debug.Log ("J2 intercepte");	
		}
		else {
			Debug.Log ("J1 gagne la balle");	
		}
	}
}
