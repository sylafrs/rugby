using UnityEngine;
using System.Collections;

/*
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/Camera/Game camera (main)")]
public class GameCamera : myMonoBehaviour {
	
	private CameraManager _cm;
	public CameraManager cameraManager { 
		get { return _cm; }
		set { _cm = value; Start(); } 
	}
	
	public Game Game {
		get {
			return cameraManager.game;	
		}
		set {}
	}
	
	public Ball Ball {
		get{
			return Game.Ball;	
		}
		set {}
	}
		
	public Unit BallOwner {
		get{
			return Ball.Owner;	
		}
		set {}
	}
	
	public Vector3 	offset;
	public Vector3 	rotation;
	public bool		tweakMode;
	
	public GameObject gameCamera;
	
	void Start() {
		if(cameraManager == null) {
			return;
		}
		
		ResetRotation();
		
		//déplacer la transfor de ce GO en Local selon l'offset
		this.gameCamera.transform.localPosition = offset;
		//this.transform.root
	}
	
	// Update is called once per frame
	void Update () {
		if(cameraManager == null || Game == null) return;
				
		if(tweakMode){
			ResetRotation();
			this.gameCamera.transform.localPosition = offset;
		}

		Vector3 cam = this.transform.position;
		
		//this.gameCamera.transform.LookAt(Ball.transform.position);

   		if(cameraManager.game.Ball.Owner){
      		this.transform.position = BallOwner.transform.position;
		}
   		else
		{
     		this.transform.position = cameraManager.game.Ball.transform.position;
		}
		
		// Debug.DrawLine(cam, this.transform.position, Color.red, 100);
	}
	
	public void OnOwnerChanged() {
		//lancer la rotation du pivot
		//this.transform.position = cameraManager.game.Ball.Owner.transform.position;
		
		if(Game.state != Game.State.PLAYING) 
			return;
		
        this.GetComponent<rotateMe>().BeginRotation(new Vector3(0, 1, 0), 180);
	}
	
	public void ResetRotation() {
		this.transform.rotation = Quaternion.Euler(rotation);
	}
}
