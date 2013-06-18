#define USE_FIXEDUPDATE

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System;

[AddComponentMenu("Scripts/Camera/CameraManager")]
public partial class CameraManager : myMonoBehaviour, Debugable {

    public Game game;

    public Camera gameCamera;
	//private Transform 	target;
	
	private Transform 	savedTarget;
	
	private Transform _target = null;
	private Transform target
    {
        get
        {
            return _target;
        }
        set
        {
			_target = value;
                        
        }
    }
	public Transform publicTarget;
	
	private Vector3 	velocity = Vector3.zero;
	//private float		velocityFloat = 0f;
	private float[]		angleVelocity = new float[3];
	private float		angleVelocityX;
	private float		angleVelocityY;
	private float		angleVelocityZ;
	private float		actualDelay;
	private Quaternion	targetRotation;
	public  float		rotationDelay;
	private float 		rotationCurrentDelay;
			
	public  float 		smoothTime 	= 0.3f;
	public  Vector3 	smoothAngle = new Vector3(0.3f, 0.3f, 0.3f);
	public 	float		moveDelay;
	public 	float		magnitudeGap;
	public  float 		rotationMagnitudeGap;
	public 	float		zoom;
	
	public 	Vector3		MaxfollowOffset;
	public 	Vector3		MinfollowOffset;
	
	//flipping when team change
	Vector3 		flipAxis;
	float 			flipAngle;
	float 			flipTime;
	
	public  float 	flipDuration = 1;
	public  float 	flipDelay;
	
	private	bool	isflipping;
	private float 	flipLastAngle;
	private float 	flipWaiting;
	//private bool    isflipped;
	public float 	zMinForBlue;
	public float 	zMaxForBlue;
    public  Team    flipedForTeam { get; private set; }
    public  Team 	TeamLooked { get { return flipedForTeam; } }
	
	public bool 	CancelNextFlip;
	//private Action	ActionOnFlipFinish;
	public Action	OnNextIdealPosition {get;set;}
	
	public enum CameraState
	{
		//FREE 		: no target, no zoom, no move
		FREE,
		//FOLLOWING : target, zoom, translation to follow
		FOLLOWING,
		//FLIPPING  : rotation, zoom, transaltion to get closer
		FLIPPING,
		//SHAKING 	: Shake !
		SHAKING,
		//ONLYZOOM  : zoom to target
		ONLYZOOM
	};
	private CameraState currentCameraState;
	
	//Cam component
	public CameraShake 			CameraShakeComponent;
	public CameraZoom  			CameraZoomComponent;
	public CameraRotatingAround CameraRotatingAroundComponent;
	
	public SuperMaoriCutSceneScript	SuperMaoriCutSceneComponent;
	public SuperJapaneseCutSceneScript SuperJapaneseCutSceneComponent;
	public WiningPointsCutScene WiningPointsCutSceneComponent;
	
	void Awake(){
		Camera camera 			= Camera.mainCamera;
		GameObject camObject	= camera.gameObject;
			
		//add components
		camObject.AddComponent<CameraShake>();
		camObject.AddComponent<CameraZoom>();
		camObject.AddComponent<CameraRotatingAround>();
		
		//get Component
		this.CameraShakeComponent 			= camera.GetComponent<CameraShake>();
		this.CameraZoomComponent  			= camera.GetComponent<CameraZoom>();
		this.CameraRotatingAroundComponent  = camera.GetComponent<CameraRotatingAround>();
		this.SuperMaoriCutSceneComponent	= camera.GetComponent<SuperMaoriCutSceneScript>();
		this.SuperJapaneseCutSceneComponent	= camera.GetComponent<SuperJapaneseCutSceneScript>();
		this.WiningPointsCutSceneComponent	= camera.GetComponent<WiningPointsCutScene>();
	}
	
	// Use this for initialization
	void Start () {
		resetActualDelay();
		resetRotationDelay();
		
		isflipping          = false;
		CancelNextFlip      = false;
		//ActionOnFlipFinish  = null;
		flipedForTeam       = game.southTeam;
		zMinForBlue	        = MinfollowOffset.z;
		zMaxForBlue	  		= MaxfollowOffset.z;
		currentCameraState  = CameraState.FREE;
	}
	
	public void ChangeCameraState(CameraState newState)
	{
		this.currentCameraState = newState;
	}
	
	void FixedUpdate(){
		this.publicTarget = this.target;
		switch (currentCameraState)
		{
			case CameraState.FREE:
			{
				//nothing ! Someone is moving the camera somewhere !
				break;
			}
			case CameraState.FLIPPING:
			{
				break;	
			}
			case CameraState.FOLLOWING:
			{
				this.RotateCam();
				this.TranslateCam();
				break;
			}
			case CameraState.ONLYZOOM:
			{
				break;
			}
			case CameraState.SHAKING:
			{
				break;
			}
			default :
			{
				break;
			}
		}
	}		
	
	private void TranslateCam()
	{
		Vector3 targetPosition  = target.TransformPoint(MaxfollowOffset);
		Vector3 offset 			= Camera.mainCamera.transform.position+(MinfollowOffset)*zoom;
		
		Vector3 result 			= Vector3.SmoothDamp(offset, targetPosition, ref velocity, smoothTime);
		Vector3 delta  			= result- Camera.mainCamera.transform.position;
		
		//Debug.Log("Delta : "+delta.magnitude);
		if( delta.magnitude > magnitudeGap){
			if(actualDelay >= moveDelay){
				Camera.mainCamera.transform.position = result;
			}else{
				actualDelay += Time.deltaTime;
			}
		}else{
			
			if(OnNextIdealPosition != null) 
			{
				OnNextIdealPosition();
				OnNextIdealPosition = null;
			}
			resetActualDelay();
		}
	}
	
	private void TranslateCam3()
	{
		Vector3 targetPosition  = target.position;
		Vector3 offset 			= Camera.mainCamera.transform.position+(MinfollowOffset)*zoom;
		
		Vector3 result 			= Vector3.SmoothDamp(offset, targetPosition, ref velocity, smoothTime);
		Vector3 delta  			= result- Camera.mainCamera.transform.position;
		
		//Debug.Log("Delta : "+delta.magnitude);
		if( delta.magnitude > magnitudeGap){
			if(actualDelay >= moveDelay){
				Camera.mainCamera.transform.position = result;
			}else{
				actualDelay += Time.deltaTime;
			}
		}else{
			
			if(OnNextIdealPosition != null) 
			{
				OnNextIdealPosition();
				OnNextIdealPosition = null;
			}
			resetActualDelay();
		}
	}
	
	private void RotateCam()
	{
		//rotation
		targetRotation = Quaternion.LookRotation(target.position - Camera.mainCamera.transform.position, Vector3.up);
		
		Vector3 euler =  Camera.mainCamera.transform.rotation.eulerAngles;
		Vector3 tarEuler = targetRotation.eulerAngles;
		
		Vector3 angle = new Vector3(
			Mathf.SmoothDampAngle(euler.x, tarEuler.x, ref angleVelocity[0], smoothAngle.x),
			Mathf.SmoothDampAngle(euler.y, tarEuler.y, ref angleVelocity[1], smoothAngle.y),
			Mathf.SmoothDampAngle(euler.z, tarEuler.z, ref angleVelocity[2], smoothAngle.z)
		);
		
			
		//}else{
		//on refait une rotation histoire de bien cadrer le joueur de toute façon
		if(angle.magnitude > rotationMagnitudeGap){
			if(rotationCurrentDelay >= rotationDelay){
				Camera.mainCamera.transform.rotation = Quaternion.Euler(angle.x, angle.y, angle.z);
			}
			else{
				rotationCurrentDelay += Time.deltaTime;
			}
		}else{
			resetRotationDelay();
		}
	}
	
	
	/*
	void FixedUpdate(){
		
        if (target != null && this.gameCamera != null)
        {
			
			//rotation
			targetRotation = Quaternion.LookRotation(target.position - this.gameCamera.transform.position, Vector3.up);
			
			Vector3 euler =  this.gameCamera.transform.rotation.eulerAngles;
			Vector3 tarEuler = targetRotation.eulerAngles;
			
			Vector3 angle = new Vector3(
				Mathf.SmoothDampAngle(euler.x, tarEuler.x, ref angleVelocity[0], smoothAngle.x),
				Mathf.SmoothDampAngle(euler.y, tarEuler.y, ref angleVelocity[1], smoothAngle.y),
				Mathf.SmoothDampAngle(euler.z, tarEuler.z, ref angleVelocity[2], smoothAngle.z)
			);
			
			if(isflipping == true){
				flipUpdate();
				//manque petite translation
				
			}else{
				if(angle.magnitude > rotationMagnitudeGap){
					if(rotationCurrentDelay >= rotationDelay){
						this.gameCamera.transform.rotation = Quaternion.Euler(angle.x, angle.y, angle.z);
					}
					else{
						rotationCurrentDelay += Time.deltaTime;
					}
				}else{
					resetRotationDelay();
				}
			}
			
			Vector3 targetPosition = target.TransformPoint(MaxfollowOffset);
			Vector3 offset = this.gameCamera.transform.position+(MinfollowOffset)*zoom;
			Vector3 result = Vector3.SmoothDamp(offset, targetPosition, ref velocity, smoothTime);
			Vector3 delta  = result- this.gameCamera.transform.position;
	
			
			
			if( delta.magnitude > magnitudeGap){
				if(actualDelay >= moveDelay){
					this.gameCamera.transform.position = result;
				}else{
					actualDelay += Time.deltaTime;
				}
			}else{
				
				if(OnNextIdealPosition != null) {
					OnNextIdealPosition();
					OnNextIdealPosition = null;
				}
				resetActualDelay();
			}		
			
		}
	}
	
	*/
	void resetActualDelay(){
		actualDelay = 0f;
	}
	
	void resetRotationDelay(){
		rotationCurrentDelay = 0f;
	}
	
	public void setTarget(Transform _t){
		target = _t;
		//resetActualDelay();
	}
	
	public void LoadParameters(CamSettings settings)
	{
		this.zoom = settings.zoom;
		Transform	target = null;
		Camera.mainCamera.nearClipPlane = settings.nearClip;
		switch (settings.target)
		{
			case CameraTargetList.BALL:
			{
				target = game.Ball.transform;
				break;
			}
			case CameraTargetList.OWNER:
			{
				if(game.Ball.Owner == null)
				{
					target = game.Ball.PreviousOwner.transform;
				}
				else
				{
					target = game.Ball.Owner.transform;
				}
				break;
			}
		}
		
		this.setTarget(target);
	}
	
    public void ForDebugWindow()
    {
#if UNITY_EDITOR
		EditorGUILayout.LabelField("Current state", this.currentCameraState.ToString());
        EditorGUILayout.LabelField("Current follow target", target == null ? "null" : target.name);
        EditorGUILayout.LabelField("Current zoom", ((int)(this.zoom)).ToString());
#endif
    }
}
