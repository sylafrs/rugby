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
	public  float 		smoothTime = 0.3f;
	private Vector3 	velocity = Vector3.zero;
	
	public 	float		delay;
	public 	float		magnitudeGap;
	private float		actualDelay;
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

        Vector3 result = Camera.mainCamera.transform.position;

        if (target != null)
        {
            Vector3 targetPosition = target.TransformPoint(MaxfollowOffset);
            Vector3 offset = Camera.mainCamera.transform.position + MinfollowOffset;
            result = Vector3.SmoothDamp(offset, targetPosition, ref velocity, smoothTime);            
        }

        Vector3 delta = result - Camera.mainCamera.transform.position;
				
		//Debug.Log("diff magnitude: "+delta.magnitude);
		
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

    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        EditorGUILayout.LabelField("Current target", target == null ? "null" : target.name);
#endif
    }
}
