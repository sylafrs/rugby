using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/FeedBack (à ranger)")]
public class feedback : myMonoBehaviour {
	
	public GameObject player;
	private Color colorSave;
	
	void Start(){
		colorSave = player.renderer.material.color;
	}
			
	void MakeEffect(){
		//player.transform ...
		player.renderer.material.color = Color.red;
		player.particleSystem.Play();
	}
	
	void DeMakeEffect(){
		//player.transform ...
		player.particleSystem.Stop();
		player.renderer.material.color = colorSave;
	}
	
	 void OnGUI() {
   
        if (GUI.Button(new Rect(10, 10, 100, 50),"Make Effect")){
			MakeEffect();
		}
        
        if (GUI.Button(new Rect(10, 70, 100, 30),"Demake Effect")){
			DeMakeEffect();
		}
	}
}
