using UnityEngine;
using System.Collections;

/*
 * @author Sylvain Lafon
 */
public class GameCamera : MonoBehaviour {
	
	private CameraManager _cm;
	public CameraManager cameraManager { 
		get { return _cm; }
		set { _cm = value; Start(); } 
	}
	
	public Vector3 	offset;
	public Vector3 	rotation;
	public bool		tweakMode;
	
	
	void Start() {
		if(cameraManager == null) return;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(cameraManager == null) return;
			
	 	Vector3 realCameraGap = new Vector3(
            offset.x * this.transform.forward.x,
            offset.y * this.transform.forward.y,
            -offset.z * this.transform.forward.z
        );

		Vector3 cam = this.transform.position;

        if(cameraManager.game.Ball.Owner){
          this.transform.position = cameraManager.game.Ball.Owner.transform.position + realCameraGap;
			//Camera.mainCamera.transform.LookAt(Ball.Owner.transform);
		}
        else
		{
          this.transform.position = cameraManager.game.Ball.transform.position + realCameraGap;
		    //Camera.mainCamera.transform.LookAt(Ball.transform);
		}

		Debug.DrawLine(cam, this.transform.position, Color.red, 100);
	}
	
	public void OnOwnerChange() {
		this.transform.position = cameraManager.game.Ball.Owner.transform.position + offset;
        this.GetComponent<rotateMe>().BeginRotation(new Vector3(0, 1, 0), 180);
	}
}
