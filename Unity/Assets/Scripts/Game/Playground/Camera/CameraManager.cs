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
	
	private Vector3 	velocity = Vector3.zero;
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
	private float 	zMinForBlue;
	private float 	zMaxForBlue;
    public  Team    flipedForTeam { get; private set; }
    public  Team 	TeamLooked { get { return flipedForTeam; } }
	
	public bool 	CancelNextFlip;
	private Action	ActionOnFlipFinish;
	public Action	OnNextIdealPosition {get;set;}
	
	// Use this for initialization
	void Start () {
		resetActualDelay();
		resetRotationDelay();
		isflipping = false;
		//isflipped= false;
		CancelNextFlip = false;
		ActionOnFlipFinish = null;
		flipedForTeam = game.southTeam;
		zMinForBlue	  = MinfollowOffset.z;
		zMaxForBlue	  = MaxfollowOffset.z;
	}
	
	void FixedUpdate(){
		
        if (target != null && Camera.mainCamera != null)
        {
			/*
			this.RotateCam();
			this.TranslateCam();
			*/
			
			if(isflipping == true){
				this.TranslateCam2();
				this.flipUpdate();
			}else{
				this.RotateCam();
				this.TranslateCam();
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
        EditorGUILayout.LabelField("Current target", target == null ? "null" : target.name);
        EditorGUILayout.LabelField("Current zoom", ((int)(this.zoom)).ToString());
#endif
    }
}
