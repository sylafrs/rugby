using UnityEngine;
using System.Collections;

public class SuperJapaneseCutSceneScript : MonoBehaviour {
	
	float 	length,
			timeElapsed;
	SuperCutsceneStateSettings settings;
	GameObject	unitObject;
	Unit		unit;
	
	public CameraManager 	cam;
	public Game				game;
	
	/// <summary>
	/// Starts the cut scene.
	/// </summary>
	/// <param name='_length'>
	/// _length.
	/// </param>
	public void StartCutScene(float _length, GameObject _go, Unit _unit){
		this.unit			= _unit;
		this.unitObject 	= _go.transform.Find("SuperCamera").gameObject;
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
	
	void OnEnable () {
		//cam.CameraZoomComponent.StartZoomIn(20,0.3f,0.3f);
		/*
		cam.CameraRotatingAroundComponent.StartTimedRotation(
			game.Ball.Owner.transform, 
			settings.rotationAxis, 
			game.Ball.Owner.transform, 
			Camera.mainCamera.transform,
			180,
			0.3f,
			0.1f);
			*/
		ActivateCameraPrefab(this.unit);
		game.refs.managers.ui.currentState = UIManager.UIState.NULL;
		StartCoroutine(Shake(this.length/2 +0.5f));
        //StartCoroutine(Rotate2(this.length - 0.5f));
	}
	
	void ActivateCameraPrefab(Unit _u){
		if(!this.unitObject)
			Debug.Log("Miam");
		this.unitObject.SetActive(true);
		this.unitObject.GetComponent<Animation>().Play();
		StartCoroutine(DesactivateGameObject(3.6f));
	}
	
	IEnumerator DesactivateGameObject(float _duration){
		yield return new WaitForSeconds(_duration);
		game.refs.managers.ui.currentState = UIManager.UIState.GameUI;
		this.unitObject.SetActive(false);
	}
	
	IEnumerator Shake(float duration) {
  		yield return new WaitForSeconds(duration);
		//cam.CameraShakeComponent.Shake(0.6f,0.3f);
		this.unitObject.GetComponent<CameraShake>().Shake(0.6f,0.3f);
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
