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
	
	public 	Vector3		MaxfollowOffset;
	public 	Vector3		MinfollowOffset;
	

    public StateMachine sm;

	// Use this for initialization
	void Start () {

        sm.SetFirstState(new MainCameraState(sm, this));
       
        /*
		gameCamera.cameraManager = this;
		scrumCamera.cameraManager = this;
		*/
		
	}
	
	void Update(){
		this.setTarget(game.right[0].transform);
		//Debug.Log(" tar "+target.position);
		Vector3 targetPosition = target.TransformPoint(MaxfollowOffset);
		Vector3 offset = Camera.mainCamera.transform.position+MinfollowOffset;
		Camera.mainCamera.transform.position = Vector3.SmoothDamp(offset, targetPosition, ref velocity, smoothTime);
	}
	
	public void setTarget(Transform _t){
		target = _t;
	}
	
	public void OnOwnerChanged()
    {	
		//gameCamera.OnOwnerChanged();
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
        EditorGUILayout.LabelField("Current target", target.name);
#endif
    }
}
