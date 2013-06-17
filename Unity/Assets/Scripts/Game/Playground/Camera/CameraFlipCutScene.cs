using UnityEngine;
using System.Collections;

public class CameraFlipCutScene : MonoBehaviour {
	
	public CameraManager 	cam;
	public Game				game;
	
	bool cancelNextFlip;
	Team flipedForTeam;
	
	/// <summary>
	/// Flips for team.
	/// </summary>
	/// <param name='_t'>
	/// _t.
	/// </param>
	/// <param name='_cb'>
	/// _cb.
	/// </param>
	public void FlipForTeam(Team _t)
	{	
		if(this.cancelNextFlip == false){
			if(this.flipedForTeam != _t){
				flipedForTeam = _t;
				this.enabled = true;
			}
		}else{
			if(this.cancelNextFlip){
				if(_t == game.southTeam){
					cam.MinfollowOffset.z	  = cam.zMinForBlue;
					cam.MaxfollowOffset.z	  = cam.zMaxForBlue;
				}
				if(_t == game.northTeam){
					cam.MinfollowOffset.z	  = cam.zMinForBlue * -1;
					cam.MaxfollowOffset.z	  = cam.zMaxForBlue * -1;
				}
				this.flipedForTeam = _t;
				this.cancelNextFlip = false;
			}
		}
	}
	
	void FlipZ(){
		cam.MinfollowOffset.z *= -1;
		cam.MaxfollowOffset.z *= -1;
	}
	
	void Awake(){
		this.cancelNextFlip = false;
		this.enabled 		= false;
		this.flipedForTeam  = game.southTeam;
	}
	
	void Start () {
		float length = 0.9f;
		cam.CameraRotatingAroundComponent.StartTimedRotation(
			cam.publicTarget,
			new Vector3(0,1,0),
			cam.publicTarget,
			Camera.mainCamera.transform,
			180,
			length,
			0.2f);
		StartCoroutine(End(length));
	}
	
	IEnumerator End(float duration) {
  		yield return new WaitForSeconds(duration);
		this.FlipZ();
		this.enabled = false;
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
	}
}
