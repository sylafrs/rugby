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
		transform.position = originPosition;
		transform.rotation =  new Quaternion(
	        originRotation.x,
	        originRotation.y,
	        originRotation.z,
	        originRotation.w);
		this.enabled = false;
	}
	
	void Start(){
		this.enabled = false;
	}
	
	void Update(){
		this.originPosition = this.transform.position;
    	this.originRotation = this.transform.rotation;
    	transform.position = originPosition + Random.insideUnitSphere * ShakeAmount;
    	transform.rotation =  new Quaternion(
	        originRotation.x + Random.Range(-ShakeAmount,ShakeAmount)*.02f,
	        originRotation.y + Random.Range(-ShakeAmount,ShakeAmount)*.02f,
	        originRotation.z + Random.Range(-ShakeAmount,ShakeAmount)*.02f,
	        originRotation.w + Random.Range(-ShakeAmount,ShakeAmount)*.02f);
	}
}