using UnityEngine;
using System.Collections;
using System;

public partial class CameraManager{
	
	
	private float  ShakeAmount;
	private Action OnFinish;
	
	public void Shake(float _shakeAmount){
		this.ShakeAmount = _shakeAmount;
		this.ChangeCameraState(CameraState.SHAKING);
	}
	
	private void ShakeUpdate(){
		//float quakeAmt = Random.value*jiggleAmt*2 - jiggleAmt;
		
		//Random.value
		
  		Vector3 pp = transform.position;
  		pp.y+= ShakeAmount; // can also add to x and/or z
  		transform.position = pp;
	}
	
	IEnumerator jiggleCam2(float duration) {
  		yield return new WaitForSeconds(duration);
 		ShakeAmount = 0;
		this.ChangeCameraState(CameraState.FREE);
		OnFinish();
	}
}