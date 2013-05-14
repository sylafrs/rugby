using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System;

[AddComponentMenu("Scripts/Camera/CameraManager")]
public class CameraManager : myMonoBehaviour, Debugable {

    public Game game;
	public GameCamera gameCamera;
		
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
            Debug.Log("Target Changed for "+value);            
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
	private bool    isflipped;
	private float 	zMinForBlue;
	private float 	zMaxForBlue;
	public Team	flipedForTeam;
	public bool 	CancelNextFlip;
	private Action	ActionOnFlipFinish;
	public Action	OnNextIdealPosition {get;set;}

    public StateMachine sm;

	// Use this for initialization
	void Start () {

        sm.SetFirstState(new MainCameraState(sm, this));
		resetActualDelay();
		resetRotationDelay();
		isflipping = false;
		isflipped= false;
		CancelNextFlip = false;
		ActionOnFlipFinish = null;
		flipedForTeam = game.right;
		zMinForBlue	  = MinfollowOffset.z;
		zMaxForBlue	  = MaxfollowOffset.z;
		
		/*
       
        /*
		gameCamera.cameraManager = this;
		scrumCamera.cameraManager = this;
		*/
		
	}
	
	void FixedUpdate(){
		
		
		if(isflipping != true)
		{
		
	        if (target != null && Camera.mainCamera != null)
	        {
				Vector3 MinfollowOffset2 = new Vector3();
				Vector3 MaxfollowOffset2 = new Vector3();
				
				if(this.isflipped == true){
					//Debug.Log ("flip");
					//z is flipped
					
					//MinfollowOffset.z *= -1;
					//MaxfollowOffset.z *= -1;
					
					//MinfollowOffset2 = new Vector3(MinfollowOffset.x, MinfollowOffset.y, -MinfollowOffset.z);
					//MaxfollowOffset2 = new Vector3(MaxfollowOffset.x, MaxfollowOffset.y, -MaxfollowOffset.z);
					
				}else{
					//MinfollowOffset2 = this.MinfollowOffset;
					//MinfollowOffset2 = this.MaxfollowOffset;
					//Debug.Log ("pas flip");
				}	
				
				Vector3 targetPosition = target.TransformPoint(MaxfollowOffset);
				
				Vector3 offset = Camera.mainCamera.transform.position+(MinfollowOffset)*zoom;
				
				Vector3 result = Vector3.SmoothDamp(offset, targetPosition, ref velocity, smoothTime);
				Vector3 delta  = result- Camera.mainCamera.transform.position;
	
				
				//rotation
				targetRotation = Quaternion.LookRotation(target.position - Camera.mainCamera.transform.position, Vector3.up);
				
				Vector3 euler =  Camera.mainCamera.transform.rotation.eulerAngles;
				Vector3 tarEuler = targetRotation.eulerAngles;
				
				Vector3 angle = new Vector3(
					Mathf.SmoothDampAngle(euler.x, tarEuler.x, ref angleVelocity[0], smoothAngle.x),
					Mathf.SmoothDampAngle(euler.y, tarEuler.y, ref angleVelocity[1], smoothAngle.y),
					Mathf.SmoothDampAngle(euler.z, tarEuler.z, ref angleVelocity[2], smoothAngle.z)
				);
				
				
				
				if(angle.magnitude > rotationMagnitudeGap){
					if(rotationCurrentDelay >= rotationDelay){
						Camera.mainCamera.transform.rotation = Quaternion.Euler(angle.x, angle.y, angle.z);
						//pas besoin, c'est déjà fait !
	        			//Camera.mainCamera.transform.LookAt(target);
					}
					else{
						rotationCurrentDelay += Time.deltaTime;
					}
				}else{
					resetRotationDelay();
				}
				
				if( delta.magnitude > magnitudeGap){
					if(actualDelay >= moveDelay){
						Camera.mainCamera.transform.position = result;
						
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
		
		
		}else{
			flipUpdate();
		}
		
		
		//if(isflipping) flipUpdate();
	}
	
	public void setTarget(Transform _t){
		target = _t;
		//resetActualDelay();
	}
	
	void resetActualDelay(){
		actualDelay = 0f;
	}
	
	void resetRotationDelay(){
		rotationCurrentDelay = 0f;
	}

    /*
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
     */
	
	//flipping camera
	private void flip(){
		flipInit(new Vector3(0,1,0), 180);
	}
	
	public void flipForTeam(Team _t, Action _cb)
	{
		/*
		Debug.Log("Fliped for Team "+flipedForTeam);
		Debug.Log("Flip for Team "+_t);
		Debug.Log("target of flip "+this.target);
		*/
		Debug.Log("Flip start ");
		
		this.ActionOnFlipFinish = _cb;
		if((isflipping == false) && (CancelNextFlip == false)){
			//on lance le flip seulement si c'est un team différente
			if(flipedForTeam != _t){
				flipedForTeam = _t;
				flip();
			}else{
				CancelNextFlip = false;
			}
		}else{
			if(CancelNextFlip){
				Debug.Log("Flip for trasnfo/touch ");
				//here beacause of touch or transfo
				if(_t == game.right){
					Debug.Log("Flip for blue");
					MinfollowOffset.z	  = zMinForBlue;
					MaxfollowOffset.z	  = zMaxForBlue;
				}
				if(_t == game.left){
					Debug.Log("Flip for red ");
					MinfollowOffset.z	  = zMinForBlue * -1;
					MaxfollowOffset.z	  = zMaxForBlue * -1;
				}
			}
		}
	}
	
	void flipInit(Vector3 axis, float angle){
		
		this.isflipping  			= true;
		this.flipAxis	 			= axis;
		this.flipAngle	 			= Mathf.Deg2Rad * angle;
		this.flipTime	 			= 0;
		this.flipLastAngle			= 0;
		this.flipWaiting			= 0;
    }
	
	void flipUpdate () 
	{				
        // Delay before flipping (seems to be added to normal delay)
		this.flipWaiting += Time.deltaTime;
		if(this.flipWaiting >= this.flipDelay){
			
            // Current time
			this.flipTime += Time.deltaTime;
			
            // Current state : 100%
			if(this.flipTime > this.flipDuration) 
                this.flipTime = this.flipDuration;
			
            // Get the angle for the current state
			float angleFromZero = Mathf.LerpAngle(0, this.flipAngle, this.flipTime/this.flipDuration);
			

            // Rotates the camera from his previous state to the current one
			if(target != null){
				Camera.mainCamera.transform.RotateAround(target.position, this.flipAxis, Mathf.Rad2Deg * (angleFromZero - flipLastAngle));
			}
			
            // This current state becomes the next previous one
            this.flipLastAngle = angleFromZero;
			
            // If the rotation is finished
            if (this.flipTime == this.flipDuration){
				flipEnd();
			}
		}
	}
	
	public void flipEnd(){
		this.MinfollowOffset.z *= -1;
		this.MaxfollowOffset.z *= -1;
		this.isflipping  		= false;
		this.ActionOnFlipFinish();
	}
	
	/*
	 * 
	 * 
	 * Destination  		: the position to translate to
	 * Delay				: when to do it
	 * BlackscreenDuration	: time of black
	 * Onfinish				: Action to do on finish
	 * 
	 */
	public void transalateWithFade(Vector3 destination,float delay,float fadeiInDuration, float fadeOutDuration,
		float blackScreenDuration, Action Onfinish){
		
		CameraFade.StartAlphaFade(Color.black,false, fadeiInDuration, delay, () => { 
			Camera.mainCamera.transform.Translate(destination); 
			CameraFade.StartAlphaFade(Color.black,true, fadeOutDuration, blackScreenDuration, () => {
				Onfinish();
			});
		});
	}
	
	public void transalateWithFade(Vector3 destination, Quaternion _rotation, float delay,float fadeiInDuration, float fadeOutDuration,
		float blackScreenDuration, Action Onfinish, Action OnFade){
		
		CameraFade.StartAlphaFade(Color.black,false, fadeiInDuration, delay, () => { 
			OnFade();
			Camera.mainCamera.transform.Translate(destination); 
			Camera.mainCamera.transform.rotation = _rotation;
			CameraFade.StartAlphaFade(Color.black,true, fadeOutDuration, blackScreenDuration, () => {
				Onfinish();
			});
		});
	}
	
	public void transalateToWithFade(Vector3 destination, Quaternion _rotation,float delay,float fadeiInDuration, float fadeOutDuration,
		float blackScreenDuration, Action Onfinish){
		
		CameraFade.StartAlphaFade(Color.black,false, fadeiInDuration, delay, () => { 
			Camera.mainCamera.transform.Translate(destination - Camera.mainCamera.transform.position, Space.World); 
			Camera.mainCamera.transform.rotation = _rotation;
			CameraFade.StartAlphaFade(Color.black,true, fadeOutDuration, blackScreenDuration, () => {
				Onfinish();
			});
		});
	}

    public void transalateToWithFade(Vector3 destination, Quaternion _rotation, float delay, float fadeiInDuration, float fadeOutDuration,
        float blackScreenDuration, Action Onfinish, Action OnFade)
    {

        CameraFade.StartAlphaFade(Color.black, false, fadeiInDuration, delay, () =>
        {
            Camera.mainCamera.transform.Translate(destination - Camera.mainCamera.transform.position, Space.World);
            Camera.mainCamera.transform.rotation = _rotation;
			OnFade();
            CameraFade.StartAlphaFade(Color.black, true, fadeOutDuration, blackScreenDuration, () =>
            {
                Onfinish();
            });
        });
    }

    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        EditorGUILayout.LabelField("Current target", target == null ? "null" : target.name);
#endif
    }
}
