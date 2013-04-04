using UnityEngine;
using System.Collections.Generic;

public delegate void CallBack_transfo(bool transformed);

/**
  * @class TransformationManager
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class TransformationManager : MonoBehaviour {
	public CallBack_transfo CallBack;
	
	public Gamer gamer {get; set;}
	public Ball ball {get; set;}
	
	//public InputDirection direction;
	public InputTouch touch;
	
	private Quaternion initialRotation;
	
	private float angle = 0;
	public float angleSpeed;
	//public float angleSpeedKeyBoard;
	//public float angleSpeedReleasedKeyBoard;
	
	private float power = 0;
	public float powerSpeed;
	
	public Vector3 maxPower;
	public float maxAngle;
	
	private enum State {
		ANGLE,
		POWER,
		WAITING,
		FINISHED
	}
	
	private bool transformed;	
	private State state;
	
	public GameObject arrow;
	
	public void Start() {
		GameObject arr = GameObject.Instantiate(arrow) as GameObject;
		
		arr.transform.parent = this.transform;
		arr.transform.localPosition = Vector3.zero;
		arr.transform.localRotation = Quaternion.identity;
		arr.transform.localScale = Vector3.one;
		
		this.arrow = arr;
	}
	
	public void OnEnable() {
		angle = 0;
		power = 0;
		initialRotation = ball.Owner.transform.rotation;
		this.state = State.ANGLE;			
	}
	
	public void OnGUI() {
		GUILayout.Space(300);
		GUILayout.Label ("Transformation");
		GUILayout.Label ("State : " + state);
		GUILayout.Label ("Angle : " + angle);
		GUILayout.Label ("Power : " + power);
	}
	
	public void Update() {
		
		if(state == State.ANGLE) {
			/*if(gamer.XboxController.IsConnected) {		
				angle = gamer.XboxController.GetDirection(direction.xbox).x;
				
				if(gamer.XboxController.GetButtonDown(touch.xbox)) {
					state = State.POWER;	
				}
			}
			else {
				float r = direction.keyboard.GetRight();
				if(r == 0) {
					if(angle > -0.01 && angle < 0.01) {
						r = 0; 
						angle = 0;
					}
					else {
						if(angle > 0) 
							r = -angleSpeedReleasedKeyBoard;
						if(angle < 0) 
							r = angleSpeedReleasedKeyBoard;
					}
				}
				
				angle += (Time.deltaTime * angleSpeedKeyBoard * r);			
				if(angle < -1)
					angle = -1;
				if(angle > 1)
					angle = 1;
				
				if(Input.GetKeyDown(touch.keyboard)) {
					state = State.POWER;	
				}
			}*/
			
			angle += angleSpeed * Time.deltaTime;
			if(angle > maxAngle) {
				angle = maxAngle;
				angleSpeed *= -1;
			}
			if(angle < -maxAngle) {
				angle = -maxAngle;
				angleSpeed *= -1;
			}
			
			
			if(Input.GetKeyDown(touch.keyboard) || (gamer.XboxController.IsConnected && gamer.XboxController.GetButtonDown(touch.xbox))) {
				state = State.POWER;	
			}
			
			ball.Owner.transform.rotation = initialRotation * Quaternion.Euler(new Vector3(0, angle * maxAngle, 0));
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
			
			if((gamer.XboxController.IsConnected && gamer.XboxController.GetButtonUp(touch.xbox)) || Input.GetKeyUp(touch.keyboard)) {
				Launch();
			}
		}
		
		if(state == State.WAITING) {
			if(ball.transform.position.y < 0.3f) {
				transformed = false;
				Finish ();	
			}
		}		
	}
	
	public void OnLimit() {
		transformed = false;
		Finish ();
	}
	
	public void But() {
		transformed = true;
		Finish ();
	}
	
	public void Launch() {
		
		state = State.WAITING;	
					
		ball.transform.parent = null;
        ball.rigidbody.useGravity = true;
		ball.rigidbody.isKinematic = false;
    //  ball.rigidbody.AddForce(Owner.transform.forward * angle * maxAngle + Owner.transform.up * power * maxPower + Owner.transform.right * power * maxPower);
        
		ball.Owner.transform.rotation = initialRotation * Quaternion.Euler(new Vector3(0, angle * maxAngle, 0));
			
		Vector3 force = ball.Owner.transform.forward * power * maxPower.x + 
						ball.Owner.transform.right * power * maxPower.z +
						ball.Owner.transform.up * power * maxPower.y;
		
		ball.rigidbody.AddForce(force);
		
		//Debug.DrawRay(ball.transform.position, force, Color.red, 100);
		
		ball.Owner = null;
	}
	
	public void Finish() {
		state = State.FINISHED;
		this.enabled = false;
		if(CallBack != null) CallBack(transformed);
	}
}
