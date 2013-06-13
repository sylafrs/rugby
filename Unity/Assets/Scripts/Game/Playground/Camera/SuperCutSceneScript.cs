using UnityEngine;
using System.Collections;

public class SuperCutSceneScript : MonoBehaviour {
	
	public float length;
	
	float timeElapsed;
	
	void Awake(){
		this.enabled = false;
		this.timeElapsed = 0f;
	}
	
	void Start () {
	}
	
	void FixedUpdate () {
		timeElapsed += Time.deltaTime;
		
		//Mathf(timeElapsed - 1.8f) <= Time.fi;
		
		if(timeElapsed == 1.8f){
		}
	}
	
	void OnEditorGUI(){
	}
}
