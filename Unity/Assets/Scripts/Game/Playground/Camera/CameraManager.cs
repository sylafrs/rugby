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
	public  Team	flipedForTeam;
    public  Team 	TeamLooked { get { return flipedForTeam; } }
	
	public bool 	CancelNextFlip;
	private Action	ActionOnFlipFinish;
	public Action	OnNextIdealPosition {get;set;}

	//public StateMachine sm;

	// Use this for initialization
	void Start () {

      //  sm.SetFirstState(new MainGameState(sm, this, this.game));
		resetActualDelay();
		resetRotationDelay();
		isflipping = false;
		isflipped= false;
		CancelNextFlip = false;
		ActionOnFlipFinish = null;
		flipedForTeam = game.southTeam;
		zMinForBlue	  = MinfollowOffset.z;
		zMaxForBlue	  = MaxfollowOffset.z;
	}
	
	void FixedUpdate(){
		
        if (target != null && Camera.mainCamera != null)
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
			
			if(isflipping == true){
				flipUpdate();
				//manque petite translation
				
			}else{
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
			
			Vector3 targetPosition = target.TransformPoint(MaxfollowOffset);
			Vector3 offset = Camera.mainCamera.transform.position+(MinfollowOffset)*zoom;
			Vector3 result = Vector3.SmoothDamp(offset, targetPosition, ref velocity, smoothTime);
			Vector3 delta  = result- Camera.mainCamera.transform.position;
	
			
			
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
	
	//flipping camera
	private void flip(){
		flipInit(new Vector3(0,1,0), 180);
	}
	
	public void flipForTeam(Team _t, Action _cb)
	{	
		this.ActionOnFlipFinish = _cb;
		if((isflipping == false) && (CancelNextFlip == false)){
			//on lance le flip seulement si c'est un team différente
			if(flipedForTeam != _t){
				flipedForTeam = _t;
				flip();
			}
		}else{
			if(CancelNextFlip){
				
				//here beacause of touch or transfo
				if(_t == game.southTeam){
					
					MinfollowOffset.z	  = zMinForBlue;
					MaxfollowOffset.z	  = zMaxForBlue;
				}
				if(_t == game.northTeam){
					
					MinfollowOffset.z	  = zMinForBlue * -1;
					MaxfollowOffset.z	  = zMaxForBlue * -1;
				}
				flipedForTeam = _t;
				CancelNextFlip = false;
			}
		}
	}
	
	void flipInit(Vector3 axis, float angle)
	{
		this.isflipping  			= true;
		this.flipAxis	 			= axis;
		this.flipAngle	 			= Mathf.Deg2Rad * angle;
		this.flipTime	 			= 0;
		this.flipLastAngle			= 0;
		this.flipWaiting			= 0;
		this.flipZ();
		this.game.southTeam.Player.stopMove();
		this.game.northTeam.Player.stopMove();
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
		this.flipZ();
		this.game.southTeam.Player.enableMove();
		this.game.northTeam.Player.enableMove();
	}
	
	private void flipZ(){
		this.MinfollowOffset.z *= -1;
		this.MaxfollowOffset.z *= -1;
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
	
	/*
	public void OnIntroLaunch(){
		sm.SendMessage("OnIntroLaunch");
	}
	*/
	
    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        EditorGUILayout.LabelField("Current target", target == null ? "null" : target.name);
        EditorGUILayout.LabelField("Current zoom", ((int)(this.zoom)).ToString());
#endif
    }
}
