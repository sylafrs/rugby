using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Camera/CameraManager")]
public class CameraManager : myMonoBehaviour {
	
	public Game game {get; set;}
	public TouchCamera touchCamera;
	public GameCamera gameCamera;
	public ScrumCamera scrumCamera;

	// Use this for initialization
	void Start () {
		gameCamera.cameraManager = this;
		scrumCamera.cameraManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnOwnerChanged()
    {	
		gameCamera.OnOwnerChanged();
	}
	
	public void OnScrum(bool active) {
		
		scrumCamera.gameObject.SetActive(active);
		if(active)
			scrumCamera.Activate();
			
	}
}
