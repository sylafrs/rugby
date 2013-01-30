using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	
	public Game game;
	public GameCamera gameCamera;
	public ScrumCamera scrumCamera;

	// Use this for initialization
	void Start () {
		gameCamera.cameraManager = this;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
