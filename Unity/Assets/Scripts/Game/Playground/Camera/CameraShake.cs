using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour{
	
	float  
					ShakeAmount, 
					duration;
	System.Action	OnFinish;
	Vector3 		originPosition;
	Quaternion 		originRotation;
	
	
	
	public void Shake(float _shakeAmount, float _duration){
		this.originPosition = this.transform.position;
    	this.originRotation = this.transform.rotation;
		this.ShakeAmount 	= _shakeAmount;
		this.duration	 	= _duration;
		this.enabled 	 	= true;
		StartCoroutine(EndShake(this.duration));
	}
	
	IEnumerator EndShake(float duration) {
  		yield return new WaitForSeconds(duration);
		this.enabled = false;
		this.OnFinish();
	}
	
	void Start(){
		this.enabled = false;
	}
	
	void Update(){
    	transform.position = originPosition + Random.insideUnitSphere * ShakeAmount;
    	transform.rotation =  Quaternion(
	        originRotation.x + Random.Range(-ShakeAmount,ShakeAmount)*.2,
	        originRotation.y + Random.Range(-ShakeAmount,ShakeAmount)*.2,
	        originRotation.z + Random.Range(-ShakeAmount,ShakeAmount)*.2,
	        originRotation.w + Random.Range(-ShakeAmount,ShakeAmount)*.2);
	}
}