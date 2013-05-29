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
	
	//private float stepSpeed = 0.0183f;
	// / <summary>
	// / 100000
	/// </summary>
	public  float step = 0f;
	private void TranslateCam2()
	{	
		step += game.settings.Global.GlobalCamera.flipMovingStep;
		//Vector3 NewMin = new Vector3(MinfollowOffset.x, min
		//Vector3 min2 = new Vector3(MinfollowOffset.x, MinfollowOffset.y, MinfollowOffset.z * -1);
		//Vector3 offset = Camera.mainCamera.transform.position+(min2)*zoom;
		Vector3 targetPosition  = target.TransformPoint(MaxfollowOffset);
		
		
		Camera.mainCamera.transform.position = Vector3.MoveTowards(Camera.mainCamera.transform.position,
			targetPosition , step);
		
		Camera.mainCamera.transform.LookAt(target);
		
		//Vector3 targetPosition2  = target.TransformPoint(MaxfollowOffset);
		//Vector3 offset2 		 = Camera.mainCamera.transform.position+(MinfollowOffset)*zoom;
		//
		//Vector3 result 			= Vector3.SmoothDamp(offset2, targetPosition2, ref velocity, smoothTime);
		//Vector3 delta  			= result- Camera.mainCamera.transform.position;
		
		//Debug.Log("Delta : "+delta.magnitude);
		
		/*
		Camera.mainCamera.transform.position = Vector3.MoveTowards(Camera.mainCamera.transform.position,
			targetPosition + min2*5, step);
		*/
	}
	
	void flipInit(Vector3 axis, float angle)
	{
		this.isflipping  			= true;
		this.flipAxis	 			= axis;
		this.flipAngle	 			= Mathf.Deg2Rad * angle;
		this.flipTime	 			= 0;
		this.flipLastAngle			= 0;
		this.flipWaiting			= 0;
		this.step 					= 0f;
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
			//float angleFromZero = Mathf.LerpAngle(0, this.flipAngle, this.flipTime/this.flipDuration);
			float angleFromZero = Mathf.SmoothDampAngle(this.flipLastAngle, this.flipAngle, ref velocityFloat,game.settings.Global.GlobalCamera.flipSmoothTime);

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
		this.game.southTeam.Player.enableMove();
		this.game.northTeam.Player.enableMove();
	}
	
	private void flipZ(){
		Debug.Log("flipZ");
		this.MinfollowOffset.z *= -1;
		this.MaxfollowOffset.z *= -1;
	}
}

