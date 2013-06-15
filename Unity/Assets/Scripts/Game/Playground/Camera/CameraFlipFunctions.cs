using UnityEngine;
using System.Collections;
using System;

public partial class CameraManager{
	
	/// <summary>
	/// Flips for team.
	/// </summary>
	/// <param name='_t'>
	/// _t.
	/// </param>
	/// <param name='_cb'>
	/// _cb.
	/// </param>
	public void flipForTeam(Team _t, Action _cb)
	{	
		this.ActionOnFlipFinish = _cb;
		if((isflipping == false) && (CancelNextFlip == false)){
			//on lance le flip seulement si c'est un team différente
			if(flipedForTeam != _t){
				flipedForTeam = _t;
				flip();
			}
		}else{
			if(CancelNextFlip){
				
				//here beacause of touch or transfo
				if(_t == game.southTeam){
					
					MinfollowOffset.z	  = zMinForBlue;
					MaxfollowOffset.z	  = zMaxForBlue;
				}
				if(_t == game.northTeam){
					
					MinfollowOffset.z	  = zMinForBlue * -1;
					MaxfollowOffset.z	  = zMaxForBlue * -1;
				}
				flipedForTeam = _t;
				CancelNextFlip = false;
			}
		}
	}
	
	void flip (){
		float length = 0.6f;
		this.CameraRotatingAroundComponent.StartTimedRotation(
			target,
			new Vector3(0,1,0),
			target,
			Camera.mainCamera.transform,
			180,
			length,
			0.3f);
		//this.game.southTeam.Player.stopMove();
		//this.game.northTeam.Player.stopMove();
		StartCoroutine(Callback(length));
	}
	
	IEnumerator Callback(float duration) {
  		yield return new WaitForSeconds(duration);
		this.flipZ();
		this.ChangeCameraState(CameraState.FOLLOWING);
		//this.game.southTeam.Player.enableMove();
		//this.game.northTeam.Player.enableMove();
	}
	
	
	
	void flipZ(){
		this.MinfollowOffset.z *= -1;
		this.MaxfollowOffset.z *= -1;
	}
}

