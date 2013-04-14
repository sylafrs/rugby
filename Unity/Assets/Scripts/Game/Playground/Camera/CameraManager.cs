using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[AddComponentMenu("Scripts/Camera/CameraManager")]
public class CameraManager : myMonoBehaviour, Debugable {

    public Game game;
	public TouchCamera touchCamera;
	public GameCamera gameCamera;
	public ScrumCamera scrumCamera;
	public TransfoCamera transfoCamera;
	
	
	private Transform 	target;
	private Vector3 	velocity = Vector3.zero;
	private float[]		angleVelocity = new float[3];
	private float		angleVelocityX;
	private float		angleVelocityY;
	private float		angleVelocityZ;
	private float		actualDelay;
	
	public  float 		smoothTime 	= 0.3f;
	public  Vector3 	smoothAngle = new Vector3(0.3f, 0.3f, 0.3f);
	public 	float		delay;
	public 	float		magnitudeGap;
	public 	float		zoom;
	
	public 	Vector3		MaxfollowOffset;
	public 	Vector3		MinfollowOffset;
	

    public StateMachine sm;

	// Use this for initialization
	void Start () {

        sm.SetFirstState(new MainCameraState(sm, this));
		resetActualDelay();
		
		/*
       
        /*
		gameCamera.cameraManager = this;
		scrumCamera.cameraManager = this;
		*/
		
	}
	
	void FixedUpdate(){
		//sera gérer dans les states
		//this.setTarget(game.right[2].transform);
		//

		
		if (target != null)
        {
			Vector3 targetPosition = target.TransformPoint(MaxfollowOffset);
			Vector3 offset = Camera.mainCamera.transform.position+(MinfollowOffset)*zoom;
			Vector3 result = Vector3.SmoothDamp(offset, targetPosition, ref velocity, smoothTime);
			Vector3 delta  = result- Camera.mainCamera.transform.position;
			
			Vector3 angle = new Vector3(
				Mathf.SmoothDampAngle(Camera.mainCamera.transform.eulerAngles.x, target.eulerAngles.x, ref angleVelocity[0], smoothAngle.x),
				Mathf.SmoothDampAngle(Camera.mainCamera.transform.eulerAngles.y, target.eulerAngles.y, ref angleVelocity[1], smoothAngle.y),
				Mathf.SmoothDampAngle(Camera.mainCamera.transform.eulerAngles.z, target.eulerAngles.z, ref angleVelocity[2], smoothAngle.z)
				);
			Vector3 Tposition = target.position;
			float distance   = Vector3.Distance(Camera.mainCamera.transform.position, target.transform.position);
			Tposition += Quaternion.Euler(angle.x, angle.y, angle.z) * new Vector3(0, 0, -distance);
        	Camera.mainCamera.transform.LookAt(target);
			
		
			if( delta.magnitude > magnitudeGap){
				if(actualDelay >= delay){
					Camera.mainCamera.transform.position = result;
				}else{
					actualDelay += Time.deltaTime;
				}
			}else{
				resetActualDelay();
			}
		}
	}
	
	public void setTarget(Transform _t){
		target = _t;
		//resetActualDelay();
	}
	
	void resetActualDelay(){
		actualDelay = 0f;
	}
	
	public void OnOwnerChanged(Unit old, Unit current)
    {	
		//gameCamera.OnOwnerChanged();
        sm.event_NewOwner(old, current);
	}
	
	public void OnScrum(bool active) {
		
		scrumCamera.gameObject.SetActive(active);
		if(active) {
			scrumCamera.Activate();
		}
		else {
			gameCamera.ResetRotation();			
			if(game.Ball.Owner.Team == game.left) {
				game.cameraManager.gameCamera.transform.RotateAround(new Vector3(0, 1, 0), Mathf.Deg2Rad * 180);	
			}
		}
			
	}

    public void OnPass(Unit from, Unit to)
    {
        sm.event_Pass(from, to);
    }

    public void ballOnGround(bool onGround)
    {
        sm.event_BallOnGround(onGround);
    }   

    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        EditorGUILayout.LabelField("Current target", target == null ? "null" : target.name);
#endif
    }
}
