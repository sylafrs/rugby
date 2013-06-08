using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
	
	float			zoom,
					smoothTime,
					magnitudeGap;
	Vector3 		originPosition,
					velocity;
	CameraManager	cameraManager;
	
	
	public void StartZoom(float _zoom, float _smoothTime){
		this.smoothTime		= _smoothTime;
		this.originPosition = this.transform.position;
		this.zoom 			= _zoom;
		this.magnitudeGap   = 0.1f;
		this.enabled 		= true;
	}
	
	void Start () {
		this.cameraManager = GameObject.Find("Managers").GetComponent<CameraManager>();
		this.enabled = false;
	}
	
	void LateUpdate () {
		
		Vector3 targetPosition  = this.cameraManager.publicTarget.TransformPoint(this.cameraManager.MaxfollowOffset);
		Vector3 offset 			= this.transform.position+(cameraManager.MinfollowOffset)*this.zoom;
		Vector3 result 			= Vector3.SmoothDamp(offset, targetPosition, ref velocity, this.smoothTime);
		Vector3 delta  			= result- this.transform.position;
		
		Debug.Log("Delta : "+delta.magnitude);
		if( delta.magnitude > this.magnitudeGap){
			transform.position = result;
			Debug.Log("Has zoomed");
		}else{
			this.enabled = false;
			Debug.Log("Has stop zooming");
		}
	}
}
