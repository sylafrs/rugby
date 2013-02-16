using UnityEngine;
using System.Collections;

public class ScrumCamera : myMonoBehaviour {
	
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
		
		
		Vector3 pos = this.transform.position;
		pos.z = _cm.game.Ball.transform.position.z;
		this.transform.position = pos;
		
		this.transform.LookAt(_cm.game.Ball.transform);
	}
}
