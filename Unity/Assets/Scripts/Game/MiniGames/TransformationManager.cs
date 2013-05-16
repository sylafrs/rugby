using UnityEngine;
using System;
using System.Collections.Generic;

/**
  * @class TransformationManager
  * @brief Description.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
[AddComponentMenu("Scripts/MiniGames/Transformation")]
public class TransformationManager : myMonoBehaviour {
	public System.Action<TransformationManager.Result> CallBack;
	
	public Gamer gamer {get; set;}
	public Ball ball {get; set;}
	
	public InputTouch touch;
	
	private Quaternion initialRotation;
	
	private float angle = 0;
	public float angleSpeed;
	
	private float power = 0;
	public float powerSpeed;

    public float minPower;
	public float maxPower;
	public float maxAngle;
	
	public bool infiniteTime = true;
	public float timeAngle = 0;
	public float timePower = 0;
	
	private Vector3 pos;
	private Vector3 dir;
	private float timeInAir = 0f;
	
	private float remainingTime = 0;
	
	private enum State {
		ANGLE,
		POWER,
		WAITING,
		FINISHED
	}
	
	public enum Result {
		NONE,
		TRANSFORMED,	
		GROUND,
		LIMIT
	}
	
	private Result transformed;	
	private State state;
	
	public GameObject arrow;
	private GameObject myArrow;
    private GameObject myArrowPower;
		
	public void OnEnable() {
		angle = 0;
		power = 0;
		initialRotation = ball.Owner.transform.rotation;
		
		this.remainingTime = timeAngle;
		this.state = State.ANGLE;
		
		myArrow = GameObject.Instantiate(arrow) as GameObject;
		if(!myArrow)
			throw new UnityException("Error : missing arrow");
		
		myArrow.transform.parent = ball.Owner.transform;
		myArrow.transform.localPosition = Vector3.zero;
		myArrow.transform.localRotation = Quaternion.identity;

        Transform jaugePower = myArrow.transform.FindChild("Power");
        if (!jaugePower)
            throw new UnityException("Error : missing arrow -> power");

        myArrowPower = jaugePower.gameObject;
	}

    public GUIStyle timeStyle;
    public Rect timeRect;

    public void OnGUI()
    {
        if (this.state == State.ANGLE || this.state == State.POWER)
        {
            Rect rect = gameUIManager.screenRelativeRect(timeRect.x, timeRect.y, timeRect.width, timeRect.height);

            if (!infiniteTime)
                GUI.Label(rect, "Time : " + (int)remainingTime, timeStyle);
        }
    }
	
	public void Update() {
		
		if(state == State.ANGLE) {				
			angle += angleSpeed * Time.deltaTime;
			if(angle > maxAngle) {
				angle = maxAngle;
				angleSpeed *= -1;
			}
			if(angle < -maxAngle) {
				angle = -maxAngle;
				angleSpeed *= -1;
			}
			
			if(!infiniteTime) {
				remainingTime -= Time.deltaTime;	
			}				
				
			if(remainingTime < 0 || Input.GetKeyDown(touch.keyboard) || (gamer.XboxController.IsConnected && gamer.XboxController.GetButtonDown(touch.xbox))) {
				remainingTime = timePower;
				state = State.POWER;	
			}
			
			ball.Owner.transform.FindChild("Fleche(Clone)").rotation = initialRotation * Quaternion.Euler(new Vector3(0, angle, 0));
		}
		
		if(state == State.POWER) {
					
			power += powerSpeed * Time.deltaTime;
			if(power > 1) {
				power = 1;
				powerSpeed *= -1;
			}
			if(power < 0) {
				power = 0;
				powerSpeed *= -1;
			}

            Vector3 scale = myArrowPower.transform.localScale;
            scale.z = power;
            myArrowPower.transform.localScale = scale;
			
			if(!infiniteTime) {
				remainingTime -= Time.deltaTime;	
			}
			
			if(remainingTime < 0 || (gamer.XboxController.IsConnected && !gamer.XboxController.GetButton(touch.xbox)) || Input.GetKeyUp(touch.keyboard)) {
				Launch();
			}
			
			
		}
		
		if(state == State.WAITING) {
			if(ball.transform.position.y < 0.3f) {
                transformed = Result.GROUND;
				Finish ();	
			}
			else
			{
				timeInAir += Time.deltaTime;
				doTransfo( timeInAir );
			}
		}
	}
	
	public void OnLimit() {
		transformed = Result.LIMIT;
		Finish ();
	}
	
	public void But() {
		transformed = Result.TRANSFORMED;
		Finish ();
	}
	
	public Action OnLaunch;
	
	private void Launch() {
		
		state = State.WAITING;
        transformed = Result.NONE;
		
		GameObject.Destroy(myArrow);
					
		ball.transform.parent = null;
        ball.rigidbody.useGravity = false;
		ball.rigidbody.isKinematic = false;
        
		ball.Owner.transform.rotation = initialRotation * Quaternion.Euler(new Vector3(0, angle, 0));
				
		pos = ball.Owner.BallPlaceHolderTransformation.transform.position;
		dir = ball.Owner.transform.forward;
		
		Debug.Log("transformation : " + dir);
		//ball.rigidbody.AddForce(force);

		if(OnLaunch != null) OnLaunch();
				
		ball.Owner = null;
	}
	
	private void doTransfo(float t)
	{
		ball.transform.position = new Vector3( dir.x * maxPower * t + pos.x,
												-0.5f * 9.81f * t * t + minPower + ((maxPower - minPower) * Mathf.Sin(Mathf.Deg2Rad * power * 80f)) * t + pos.y,
												dir.z * maxPower * t + pos.z);
	}
	
	public void Finish() {
		state = State.FINISHED;
		timeInAir = 0f;
		this.enabled = false;
		if(CallBack != null) CallBack(transformed);
	}
}
