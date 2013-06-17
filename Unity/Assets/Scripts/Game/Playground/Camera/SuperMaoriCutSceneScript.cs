using UnityEngine;
using System.Collections;

public class SuperMaoriCutSceneScript : MonoBehaviour {
	
	float 	length,
			timeElapsed;
	SuperCutsceneStateSettings settings;
	
	public CameraManager 	cam;
	public Game				game;
	
	/// <summary>
	/// Starts the cut scene.
	/// </summary>
	/// <param name='_length'>
	/// _length.
	/// </param>
	public void StartCutScene(float _length){
		this.length			= _length;
		this.timeElapsed 	= 0f;
		this.enabled		= true;
	}
	
	/// <summary>
	/// Stops the cut scene.
	/// </summary>
	/// <returns>
	/// The cut scene.
	/// </returns>
	public void StopCutScene(){
		this.enabled = false;
	}
	
	void Awake(){
		settings = cam.game.settings.GameStates.MainState.PlayingState.WaitingState.superCutsceneState;
		this.enabled 		= false;
	}
	
	void OnEnable() {
		cam.CameraZoomComponent.StartZoomIn(20,0.3f,0.3f);
		cam.CameraRotatingAroundComponent.StartTimedRotation(
			game.Ball.Owner.transform, 
			settings.rotationAxis, 
			game.Ball.Owner.transform, 
			Camera.mainCamera.transform,
			180,
			0.3f,
			0.1f);
		StartCoroutine(Shake(this.length/2));
		StartCoroutine(Rotate2(this.length-0.5f));

        Debug.Log("SUPER MAORI");

        AudioSource src = this.game.Ball.Owner.audio;
        src.clip = this.game.refs.sounds.SuperNorth;
        src.Play();
	}
	
	IEnumerator Shake(float duration) {
  		yield return new WaitForSeconds(duration);
		cam.CameraShakeComponent.Shake(0.6f,0.3f);
	}
	
	IEnumerator Rotate2(float duration) {
  		yield return new WaitForSeconds(duration);
		cam.CameraZoomComponent.ZoomToOrigin(0.3f,0.3f);
		cam.CameraRotatingAroundComponent.StartTimedRotation(
			game.Ball.Owner.transform, 
			settings.rotationAxis, 
			game.Ball.Owner.transform, 
			Camera.mainCamera.transform,
			180,
			0.3f,
			0.1f);
	}
	
	void FixedUpdate () {
		this.timeElapsed += Time.deltaTime;
		if(this.timeElapsed >= this.length){
			this.StopCutScene();
		}
	}
}
