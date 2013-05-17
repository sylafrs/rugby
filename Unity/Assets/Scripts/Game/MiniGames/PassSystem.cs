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
	//private Vector3 directionFromTarget;
	private float TimeUpdate;
	//private float BetaAngle;
	//private float GammaAngle;
	private Vector3 initialPosition;
	private float angle;
	private float initialVelocity;
	private float fromVelocity;
	private Vector3 butBleu;
	private Vector3 butRouge;
	private Vector3 butBleuRouge;
	private float multiplyDirection = 20f;
	
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
		/*
		Debug.DrawRay(this.target.transform.position, new Vector3(-1f, this.target.transform.position.y, 0), Color.yellow, 100f);
		Debug.DrawRay(this.target.transform.position, new Vector3(1f, this.target.transform.position.y, 0), Color.yellow, 100f);
		Debug.DrawRay(this.target.transform.position, new Vector3(0f, this.target.transform.position.y, 1f), Color.yellow, 100f);
		Debug.DrawRay(this.target.transform.position, new Vector3(0f, this.target.transform.position.y, -1f), Color.yellow, 100f);
		 * */
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
			//directionFromToTarget();
			calculateRelativePosition();
			calculateRelativeDirection();
			
			if (!passValidity())
			{
				CorrectTrajectory();
				CorrectPosition();
			}
			
			multiplyRelativeDirection();
			/*
			Debug.DrawRay(relativePosition, new Vector3(-1f, relativePosition.y, 0), Color.yellow, 100f);
			Debug.DrawRay(relativePosition, new Vector3(1f, relativePosition.y, 0), Color.yellow, 100f);
			Debug.DrawRay(relativePosition, new Vector3(0f, relativePosition.y, 1f), Color.yellow, 100f);
			Debug.DrawRay(relativePosition, new Vector3(0f, relativePosition.y, -1f), Color.yellow, 100f);
			*/
			ball.transform.parent = null;
			ball.rigidbody.isKinematic = false;
			ball.rigidbody.useGravity = false;
			ball.Owner = null;
			
			//Vector3 tmp = new Vector3(target.transform.position.x, 1.0f, target.transform.position.z);
			//Vector3 tmp2 = new Vector3(relativePosition.x, 5.0f, relativePosition.z);

			// BetaAngle = Vector3.Angle(Vector3.right, relativeDirection) * Mathf.PI / 180;
			// GammaAngle = Vector3.Angle(Vector3.right, from.transform.forward) * Mathf.PI / 180;

			this.magnitude = calculateMagnitude(from.transform.position, relativePosition);
			angle = Mathf.Deg2Rad * 25.0f;

			target.Order = Order.OrderMove(relativePosition, Order.TYPE_DEPLACEMENT.SPRINT);
			ball.NextOwner = target;

			Order.TYPE_POSITION typePosition = target.Team.PositionInMap(target);
			//MyDebug.Log("pos in map : " + typePosition);
			foreach (Unit u in target.Team)
			{
				if (u != target)
				{
					//u.Order = Order.OrderOffensiveSide(target, new Vector3(ball.Game.settings.Vheight, 0, ball.Game.settings.Vwidth), target.Team.right, typePosition);
						//u.Order = Order.OrderSupport(owner, new Vector3(Game.settings.Vheight, 0, Game.settings.Vwidth), right);
				}
			}
		}
	}
	
	/*
	 * TODO : keep in mind that the curve on Y in the pass depends of the passSpeed
	 */
	public void DoPass(float t)
	{
		ball.transform.position = new Vector3(relativeDirection.x * 1.5f * t + initialPosition.x,
			-0.5f * 9.81f * t * t + velocityPass * Mathf.Sin(angle) * t + initialPosition.y,
			relativeDirection.z * 1.5f * t + initialPosition.z);
	}
	
	/*
	 * Correct the relativeDirection to send at the initial position of the target
	 * TODO : Tweak the passSpeed or the NMA velocity max if this case is too much present;
	 * */
	private void CorrectTrajectory()
	{
		if (from.transform.position.x > target.transform.position.x)
			relativeDirection = -Vector3.Cross(butBleuRouge, Vector3.up).normalized;
		else
			relativeDirection = Vector3.Cross(butBleuRouge, Vector3.up).normalized;
		//Debug.DrawRay(ball.Owner.transform.position, relativeDirection*100f, Color.cyan, 100f);
	}

	private void CorrectPosition()
	{
		relativePosition = ball.transform.position + relativeDirection * velocityPass * magnitude / target.nma.velocity.magnitude;
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
	/*private void directionFromToTarget()
	{
		directionFromTarget = target.transform.position - from.transform.position;
	}*/

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
		relativeDirection = (relativePosition - ball.transform.position).normalized;
	}

	private void multiplyRelativeDirection()
	{
		relativeDirection = relativeDirection * multiplyDirection;
	}

	/**
	 * Dot product between my relative direction of the pass and forward to know if the direction calculate is good
	 * return true if the direction is behind the forward else return false
	 */
	private bool passValidity()
	{
		//Debug.DrawRay(ball.Owner.transform.position, relativeDirection * 100f, Color.red, 100f);
		//Debug.DrawRay(target.transform.position, target.transform.forward * 100f, Color.red, 100f);
		butBleuRouge = butBleu - butRouge;
		return Vector3.Dot(relativeDirection, butBleuRouge) > 0 ? true : false;
	}
}
