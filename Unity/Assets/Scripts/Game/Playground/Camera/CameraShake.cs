using UnityEngine;
using System.Collections;
using System;

public partial class CameraManager{
	
	private float ShakeAmount;
	
	public void Shake(float _shakeAmount){
		this.ShakeAmount = _shakeAmount;
		this.ChangeCameraState(CameraState.SHAKING);
	}
	
	private void ShakeUpdate(){
		Debug.Log("Wow Shaking");	
	}
}