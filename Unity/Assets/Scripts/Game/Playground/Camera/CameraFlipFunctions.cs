using UnityEngine;
using System.Collections;
using System;

public partial class CameraManager{
	
	//flipping camera
	private void flip(){
		flipInit(new Vector3(0,1,0), 180);
	}
	
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
	
	void flipInit(Vector3 axis, float angle)
	{
		this.isflipping  			= true;
		this.flipAxis	 			= axis;
		this.flipAngle	 			= Mathf.Deg2Rad * angle;
		this.flipTime	 			= 0;
		this.flipLastAngle			= 0;
		this.flipWaiting			= 0;
		this.flipZ();
		this.game.southTeam.Player.stopMove();
		this.game.northTeam.Player.stopMove();
    }
	
	void flipUpdate () 
	{				
        // Delay before flipping (seems to be added to normal delay)
		this.flipWaiting += Time.deltaTime;
		if(this.flipWaiting >= this.flipDelay){
			
            // Current time
			this.flipTime += Time.deltaTime;
			
            // Current state : 100%
			if(this.flipTime > this.flipDuration) 
                this.flipTime = this.flipDuration;
			
            // Get the angle for the current state
			float angleFromZero = Mathf.LerpAngle(0, this.flipAngle, this.flipTime/this.flipDuration);

            // Rotates the camera from his previous state to the current one
			if(target != null){
				this.gameCamera.transform.RotateAround(target.position, this.flipAxis, Mathf.Rad2Deg * (angleFromZero - flipLastAngle));
			}
			
            // This current state becomes the next previous one
            this.flipLastAngle = angleFromZero;
			
            // If the rotation is finished
            if (this.flipTime == this.flipDuration){
				flipEnd();
			}
		}
	}
	
	public void flipEnd(){
		this.MinfollowOffset.z *= -1;
		this.MaxfollowOffset.z *= -1;
		this.isflipping  		= false;
		this.ActionOnFlipFinish();
		this.flipZ();
		this.game.southTeam.Player.enableMove();
		this.game.northTeam.Player.enableMove();
	}
	
	private void flipZ(){
		this.MinfollowOffset.z *= -1;
		this.MaxfollowOffset.z *= -1;
	}
}

