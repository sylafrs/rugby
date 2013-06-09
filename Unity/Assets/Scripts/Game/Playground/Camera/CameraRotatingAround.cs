using UnityEngine;
using System.Collections;

public class CameraRotatingAround : MonoBehaviour {
	
	Transform 	center,
				focus,
				start;
	Vector3	  	axis;
	float		targetAngle,
				lastAngle,
				duration,
				timeElapsed;
	
	///start a timed rotation, from 0 to Target Angle in Duraction sec
	public void StartTimedRotation(Transform _center, Vector3 _axis, Transform _focus, Transform _start, float _angle, float _duration){
		this.duration 		= _duration;
		this.timeElapsed 	= 0f;
		this.lastAngle		= 0f;
		StartRotation(_center, _axis, _focus, _start, _angle);
	}
	
	///start an endless rotation, each frame
	public void StartEndlessRotation(Transform _center, Vector3 _axis, Transform _focus, Transform _start, float _angle){
		this.duration	   = Mathf.Infinity;
		StartRotation(_center, _axis, _focus, _start, _angle);
	}
	
	/// <summary>
	/// Stops the rotation.
	/// </summary>
	public void StopRotation(){
		this.enabled = false;
	}
	
	void StartRotation(Transform _center, Vector3 _axis, Transform _focus, Transform _start, float _angle){
		this.center 	 = _center;
		this.axis		 = _axis;
		this.focus		 = _focus;
		this.start		 = _start;
		this.targetAngle = _angle;
		this.transform.position = this.start.position;
		this.enabled 	= true;
	}
	
	void Start () {
		this.enabled = false;
	}
	
	void Update () {
		float angle = 0f;
		if(duration != Mathf.Infinity){
			if(this.timeElapsed <= this.duration){
				this.timeElapsed        += Time.deltaTime;
				float angleFromZero = Mathf.LerpAngle(0, this.targetAngle, this.timeElapsed/this.duration);
				Debug.Log("angle from zero : "+angleFromZero);
				angle = angleFromZero - this.lastAngle;
				this.lastAngle = angleFromZero;
			}else{
				this.StopRotation();
			}
		}else{
			angle = this.targetAngle;	
		}
		this.transform.RotateAround(this.center.position,this.axis,angle * Mathf.Deg2Rad * Time.deltaTime);
		this.transform.LookAt(this.focus);
	}
}
