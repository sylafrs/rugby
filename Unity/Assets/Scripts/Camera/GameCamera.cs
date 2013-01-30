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
	
	public Ball Ball {
		get{
			return cameraManager.game.Ball;	
		}
		set {}
	}
	
	public Unit BallOwner {
		get{
			return Ball.Owner;	
		}
		set {}
	}
	
	void Start() {
		if(cameraManager == null) return;
		
		this.transform.rotation = Quaternion.Euler(rotation);
		this.transform.position = BallOwner.transform.position - offset;
	}
	
	// Update is called once per frame
	void Update () {
		if(cameraManager == null) return;
		
		if(tweakMode){
			this.transform.rotation = Quaternion.Euler(rotation);
		}
	 	Vector3 realCameraGap = new Vector3(
            offset.x * this.transform.forward.x,
            offset.y * this.transform.forward.y,
            -offset.z * this.transform.forward.z
        );

		Vector3 cam = this.transform.position;

        if(cameraManager.game.Ball.Owner){
          this.transform.position = BallOwner.transform.position + realCameraGap;
			//Camera.mainCamera.transform.LookAt(Ball.Owner.transform);
		}
        else
		{
          this.transform.position = cameraManager.game.Ball.transform.position + realCameraGap;
		    //Camera.mainCamera.transform.LookAt(Ball.transform);
		}

		Debug.DrawLine(cam, this.transform.position, Color.red, 100);
	}
	
	public void OwnerChanged() {	
		Camera.mainCamera.transform.position = BallOwner.transform.position + offset;
        Camera.mainCamera.GetComponent<rotateMe>().rotate(new Vector3(0, 1, 0), 180);	
	}
}
