using UnityEngine;
using System.Collections;

public class WiningPointsCutScene : MonoBehaviour {
	
	public Game	game;
	
	Animation 		anim;
	System.Action 	OnFinish;
	Team			teamWining;
	
	/// <summary>
	/// Increases the one point.
	/// </summary>
	/// <param name='test'>
	/// Test.
	/// </param>
	public void IncreaseOnePoint(){
		game.Referee.GivePoints(1,teamWining);
	}
	
	/// <summary>
	/// Starts the scene.
	/// </summary>
	public void StartScene(System.Action _cb, Team _team){
		this.enabled  = true;
		this.OnFinish = _cb;
		this.teamWining = _team;
	}
	
	/// <summary>
	/// Stops the scene.
	/// </summary>
	public void StopScene(){
		this.enabled = false;
		this.OnFinish();
	}
		
	void Awake(){
		this.enabled 	= false;
		this.anim 		= this.GetComponent<Animation>();
	}
	
	void OnEnable(){
		this.anim.Play("winingPoints", PlayMode.StopSameLayer);
	}
}
