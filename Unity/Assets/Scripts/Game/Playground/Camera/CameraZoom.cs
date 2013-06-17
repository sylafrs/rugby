using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
	
	float			zoomPower,
					smoothTime,
					duration,
					magnitudeGap,
					originalFOWValue,
					velocity,
					timeElapsed;
	System.Action 	OnFinish; 
	
	/// <summary>
	/// Starts the zoom in.
	/// </summary>
	/// <param name='_zoomIn'>
	/// _zoom in.
	/// </param>
	/// <param name='_smoothTime'>
	/// _smooth time.
	/// </param>
	public void StartZoomIn(float _zoomPower, float _smoothTime, float _duration){
		this.zoomPower 		= Camera.mainCamera.fieldOfView - _zoomPower;
		this.StartZoom(_smoothTime, _duration);
	}
	
	/// <summary>
	/// Starts the zoom out.
	/// </summary>
	/// <param name='_zoomtOut'>
	/// _zoomt out.
	/// </param>
	/// <param name='_smoothTime'>
	/// _smooth time.
	/// </param>
	public void StartZoomOut(float _zoomPower, float _smoothTime, float _duration){
		this.zoomPower 		= Camera.mainCamera.fieldOfView + _zoomPower;
		this.StartZoom(_smoothTime, _duration);
	}
	
	/// <summary>
	/// Stops the zoom.
	/// </summary>
	public void StopZoom(){
		this.enabled = false;
	}
	
	/// <summary>
	/// Zooms to origin.
	/// </summary>
	/// <param name='_smoothTime'>
	/// _smooth time.
	/// </param>
	/// <param name='_duration'>
	/// _duration.
	/// </param>
	public void ZoomToOrigin(float _smoothTime, float _duration){
		float delta = this.originalFOWValue - Camera.mainCamera.fieldOfView;
		if(delta != 0){
			if(delta > 0){
				this.StartZoomOut(delta, _smoothTime, _duration);
			}else{
				this.StartZoomIn(Mathf.Abs(delta), _smoothTime, _duration);
			}
		}
	}
	
	void StartZoom(float _smoothTime, float _duration){
		this.duration 		= _duration;
		this.timeElapsed	= 0f;
		this.smoothTime 	= _smoothTime;
		this.enabled 		= true;
	}
	
	void Awake(){
		this.originalFOWValue 	= Camera.mainCamera.fieldOfView;
		this.enabled 			= false;
	}
	
	void FixedUpdate () {
		this.timeElapsed += Time.deltaTime;
		if(this.timeElapsed <= this.duration){
			Camera.mainCamera.fieldOfView = Mathf.SmoothDamp(Camera.mainCamera.fieldOfView,this.zoomPower,ref this.velocity,this.smoothTime);
		}else{
			this.StopZoom();
		}
	}
}
