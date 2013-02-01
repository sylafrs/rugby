using UnityEngine;
using System.Collections;

public class ScrumCamera : MonoBehaviour {
	
	private CameraManager _cm;
	public CameraManager cameraManager { 
		get { return _cm; }
		set { _cm = value; Start(); } 
	}

	private CameraManager _cm;
	public CameraManager cameraManager { 
		get { return _cm; }
		set { _cm = value; Start(); } 
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Activate() {
		/*
		Vector3 pos = this.transform.position;
		pos.z = _ball.transform.position.z;
		cam.transform.position = pos;
		cam.gameObject.SetActiveRecursively(true);
		cam.transform.LookAt(_ball.transform);
		*/
	}
}
