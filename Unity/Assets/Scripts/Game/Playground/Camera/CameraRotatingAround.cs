using UnityEngine;
using System.Collections;

public class CameraRotatingAround : MonoBehaviour {
	
	Transform 	center,
				focus;
	Vector3	  	axis;
	
	public void StartTimedRotation(Transform _center, Vector3 _axis){
		StartRotation(_center, _axis);
	}
	
	public void StartEndlessRotation(Transform _center, Vector3 _axis){
		StartRotation(_center, _axis);
	}
	
	void StartRotation(Transform _center, Vector3 _axis){
		this.center = _center;
		this.axis	= _axis;
	}
	
	void Start () {
		this.enabled = false;
	}
	
	void Update () {
		Camera.mainCamera.transform.LookAt(focus);
	}
}
