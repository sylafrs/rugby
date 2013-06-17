using UnityEngine;
using System.Collections;

public class WiningPointsCutScene : MonoBehaviour {
	
	public Game	game;
	Animation anim;
	
	/// <summary>
	/// Increases the one point.
	/// </summary>
	/// <param name='test'>
	/// Test.
	/// </param>
	public void IncreaseOnePoint(float test){
		Debug.Log("Point test "+test);
	}
	
	/// <summary>
	/// Starts the scene.
	/// </summary>
	public void StartScene(){
		this.enabled = true;
	}
	
	/// <summary>
	/// Stops the scene.
	/// </summary>
	public void StopScene(){
		this.enabled = false;
	}
		
	void Awake(){
		this.enabled 	= false;
		this.anim 		= this.GetComponent<Animation>();
	}
	
	void OnEnable(){
		this.anim.Play("winingPoints", PlayMode.StopSameLayer);
	}
}
