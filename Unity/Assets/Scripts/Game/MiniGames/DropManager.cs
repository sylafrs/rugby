using UnityEngine;
using System.Collections;

public class DropManager {

	Ball ball;
	Unit owner;
	TYPEOFDROP type;
	Vector3 ownerDirection;
	Vector3 initPos;
	float angleX;

	public enum TYPEOFDROP
	{
		UPANDUNDER,
		KICK
	}

	public DropManager(Ball b, TYPEOFDROP t)
	{
		ball = b;
		type = t;
	}

	public void setupDrop()
	{
		float randAngle = Random.Range(- ball.randomLimitAngle, ball.randomLimitAngle);
		if (ball != null)
		{
			owner = ball.Owner;
			ball.transform.parent = null;
			ball.rigidbody.isKinematic = false;
			ball.rigidbody.useGravity = false;
			
			ownerDirection = ball.Owner.transform.forward;
			ball.Owner = null;
			
			angleX = Mathf.Deg2Rad * randAngle;
			
			ball.transform.position = owner.BallPlaceHolderDrop.transform.position;
			initPos = ball.transform.position;
			
			owner.canCatchTheBall = false;
		}
	}
	
	// Update is called once per frame
	public void doDrop (float t) {
		switch (type)
		{
			case TYPEOFDROP.KICK: doKick(t);
				break;
			case TYPEOFDROP.UPANDUNDER: doUpAndUnder(t);
				break;
			default: break;
		}

	}

	private void doKick(float t)
	{

		ball.transform.position = new Vector3( (ownerDirection.x * ball.multiplierDropKick.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x,
												-0.75f * 9.81f * t * t + ball.multiplierDropKick.x * Mathf.Sin(Mathf.Deg2Rad * ball.angleDropKick) * t + initPos.y,
												(ownerDirection.z * ball.multiplierDropKick.y + Mathf.Sin(angleX)) * t + initPos.z);
	}

	private void doUpAndUnder(float t)
	{
		Vector3 pos = ball.transform.position;
		ball.transform.position = new Vector3( (ownerDirection.x * ball.multiplierDropUpAndUnder.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x,
												-0.75f * 9.81f * t * t + ball.multiplierDropUpAndUnder.x * Mathf.Sin(Mathf.Deg2Rad * ball.angleDropUpAndUnder) * t + initPos.y,
												(ownerDirection.z * ball.multiplierDropUpAndUnder.y + Mathf.Sin(angleX)) * t + initPos.z);
	}
}
