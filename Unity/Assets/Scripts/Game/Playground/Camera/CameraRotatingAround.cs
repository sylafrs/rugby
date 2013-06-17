using UnityEngine;
using System.Collections;

public class CameraRotatingAround : MonoBehaviour {
	
	Transform 		center,
					focus,
					start;
	Vector3	  		axis;
	float			targetAngle,
					lastAngle,
					duration,
					velocityFloat,
					timeElapsed,
					smooth;
	CameraManager	cameraManager;
	
	///start a timed rotation, from 0 to Target Angle in Duraction sec
	public void StartTimedRotation(Transform _center, Vector3 _axis, Transform _focus, Transform _start, float _angle, float _duration, float _smooth){
		this.duration 		= _duration;
		this.timeElapsed 	= 0f;
		this.lastAngle		= 0f;
		this.smooth		    = _smooth;
		this.targetAngle    = _angle * Mathf.Deg2Rad;
		StartRotation(_center, _axis, _focus, _start);
	}
	
	///start an endless rotation, each frame
	public void StartEndlessRotation(Transform _center, Vector3 _axis, Transform _focus, Transform _start, float _angle){
		this.duration	   = Mathf.Infinity;
		this.targetAngle   = _angle;
		this.smooth		   = 0;
		StartRotation(_center, _axis, _focus, _start);
	}
	
	/// <summary>
	/// Stops the rotation.
	/// </summary>
	public void StopRotation(){
		this.enabled = false;
	}
	
	void StartRotation(Transform _center, Vector3 _axis, Transform _focus, Transform _start){
		this.center 	 = _center;
		this.axis		 = _axis;
		this.focus		 = _focus;
		this.start		 = _start;
		this.transform.position = this.start.position;
		cameraManager.ChangeCameraState(CameraManager.CameraState.FREE);
		this.enabled 	= true;
	}
	
	void Awake(){
		this.cameraManager = GameObject.Find("Managers").GetComponent<CameraManager>();
		this.enabled = false;
	}
	
	void FixedUpdate () {
		float angle = 0f;
		if(duration != Mathf.Infinity){
			if(this.timeElapsed <= this.duration){
				this.timeElapsed        += Time.deltaTime;
				float angleFromZero = Mathf.SmoothDampAngle(this.lastAngle, this.targetAngle, ref this.velocityFloat,this.smooth);
				angle = (angleFromZero - this.lastAngle) * Mathf.Rad2Deg;
				this.lastAngle = angleFromZero;
			}else{
				this.StopRotation();
			}
		}else{
			angle = this.targetAngle;	
		}
		this.transform.RotateAround(this.center.position,this.axis,angle);
		this.transform.LookAt(this.focus);
	}
}