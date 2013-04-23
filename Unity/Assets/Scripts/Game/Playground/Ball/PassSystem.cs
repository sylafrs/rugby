using UnityEngine;
using System.Collections;

/**
 * @class Pass
 * @brief Doing a pass
 * @author Guilleminot Florian
 */
public class PassSystem {
	
	// Variables
	private Unit from;
	private Unit target;
	private Ball ball;
	private float velocityPass;
	private float magnitude;
	private Vector3 relativePosition;
	private Vector3 relativeDirection;
	private Vector3 directionFromTarget;
	private float TimeUpdate;
	private float BetaAngle;
	private float GammaAngle;
	private Vector3 initialPosition;
	private float angle;
	private float initialVelocity;
	private float fromVelocity;
	private Vector3 butBleu;
	private Vector3 butRouge;
	
	public PassSystem(Vector3 b1, Vector3 b2) {
		Init(b1, b2, null, null, null);	
	}
	
	public PassSystem(Vector3 b1, Vector3 b2, Unit from) {
		Init(b1, b2, from, null, null);	
	}
	
	public PassSystem(Vector3 b1, Vector3 b2, Unit from, Unit target) {
		Init(b1, b2, from, target, null);	
	}

	// Constructor
	public PassSystem(Vector3 b1, Vector3 b2, Unit from, Unit target, Ball ball)
	{
		Init(b1, b2, from, target, ball);	
	}
		
	private void Init(Vector3 b1, Vector3 b2, Unit from, Unit target, Ball ball) {
		this.from = from;
		this.target = target;
		this.ball = ball;
		this.initialPosition = ball.transform.position;
		this.butBleu = b1;
		this.butRouge = b2;
	}

	// Getter/Setter
	public Unit GetFrom()
	{
		return from;
	}
	public void SetFrom(Unit from)
	{
		this.from = from;
	}
	public Unit GetTarget()
	{
		return target;
	}
	public void SetTarget(Unit target)
	{
		this.target = target;
	}

	// Methods

	public void CalculatePass()
	{
		if (from && target && ball)
		{
			velocityPass = this.ball.passSpeed;
			this.magnitude = calculateMagnitude(from.transform.position, target.transform.position);
			directionFromToTarget();
			calculateRelativePosition();
			calculateRelativeDirection();
			
			if (!passValidity())
			{
				CorrectTrajectory();
			}
			
			ball.transform.parent = null;
			ball.rigidbody.isKinematic = false;
			ball.rigidbody.useGravity = false;
			ball.Owner = null;
			
			Vector3 tmp = new Vector3(target.transform.position.x, 1.0f, target.transform.position.z);
			Vector3 tmp2 = new Vector3(relativePosition.x, 5.0f, relativePosition.z);
			//Debug.DrawLine(ball.transform.position, relativePosition, Color.cyan, 100);
			//Debug.DrawRay(ball.transform.position, relativeDirection, Color.yellow, 100);

			BetaAngle = Vector3.Angle(Vector3.right, relativeDirection) * Mathf.PI / 180;
			GammaAngle = Vector3.Angle(Vector3.right, from.transform.forward) * Mathf.PI / 180;

			this.magnitude = calculateMagnitude(from.transform.position, relativePosition);
			angle = 25.0f * Mathf.PI / 180;
		}
	}
	
	/*
	 * TODO : keep in mind that the curve on Y in the pass depends of the passSpeed or the NMA velocity
	 */
	public void DoPass(float t)
	{
		//Debug.Log(t);
		//Debug.Log(initialVelocity);

		ball.transform.position = new Vector3(relativeDirection.x * t + initialPosition.x,
			-0.5f * 9.81f * t * t + /*from.nma.velocity.magnitude*/ velocityPass * Mathf.Sin(angle) * t + initialPosition.y, 
			relativeDirection.z * t + initialPosition.z);
	}
	
	/*
	 * Correct the relativeDirection to send at the initial position of the target
	 * TODO : Tweak the passSpeed or the NMA velocity max if this case is too much present;
	 * */
	public void CorrectTrajectory()
	{
		Debug.LogWarning("CorrectTrajectory : Tweak the passSpeed or the NMA velocity max if this case is too much present");
		relativePosition = target.transform.position;
		calculateRelativeDirection();
		target.Order = Order.OrderMove(relativeDirection, Order.TYPE_DEPLACEMENT.SPRINT);
	}

	/*
	 * @brief distance initial between two players
	 */
	private float calculateMagnitude(Vector3 from, Vector3 to)
	{
		return (to - from).magnitude;
	}

	/*
	 * @brief direction between the ball and the target
	 */
	private void directionFromToTarget()
	{
		directionFromTarget = target.transform.position - from.transform.position;
	}

	/*
	 * @brief relative position of the target
	 */
	private void calculateRelativePosition()
	{
		relativePosition =  target.transform.position + target.transform.forward * target.nma.velocity.magnitude * magnitude / velocityPass;
	}

	/*
	 * @brief relative direction between the ball and the relative position of the target
	 */
	private void calculateRelativeDirection()
	{
		relativeDirection = relativePosition - ball.transform.position;
	}

	/**
	 * Dot product between my relative direction of the pass and forward to know if the direction calculate is good
	 * return true if the direction is behind the forward else return false
	 */
	private bool passValidity()
	{
		Vector3 tmpDir = butBleu - butRouge;
		// Debug.Log("direction : " + relativeDirection + " par rapport à : " + tmpDir +
		// 	" donne le scalaire : " + Vector3.Dot(relativeDirection, tmpDir));
		return Vector3.Dot(relativeDirection, tmpDir) > 0 ? true : false;
	}
}
