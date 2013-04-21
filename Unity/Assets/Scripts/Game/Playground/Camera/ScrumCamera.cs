using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Camera/Scrum Camera")]
public class ScrumCamera : myMonoBehaviour {
	
	private CameraManager _cm;
	public CameraManager cameraManager { 
		get { return _cm; }
		set { _cm = value; } 
	}
		
	public void Activate() {		
		Vector3 pos = this.transform.position;
		pos.z = _cm.game.Ball.transform.position.z;
		this.transform.position = pos;
		
		this.transform.LookAt(_cm.game.Ball.transform);
	}
}
