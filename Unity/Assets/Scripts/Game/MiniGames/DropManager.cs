using UnityEngine;
using System.Collections;

public class DropManager {

	Ball ball;
	Unit owner;
	TYPEOFDROP type;
	Vector3 ownerDirection;
	Vector3 initPos;
	float angleX;
	float acceleration;

	public enum TYPEOFDROP
	{
		UPANDUNDER,
		KICK
	}

    public TYPEOFDROP typeOfDrop
    {
        get
        {
            return type;
        }
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
		
		acceleration = ball.accelerationDrop > 0f? -ball.accelerationDrop : ball.accelerationDrop;
		
		CalculateCircle();
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
												acceleration * 9.81f * t * t + ball.multiplierDropKick.x * Mathf.Sin(Mathf.Deg2Rad * Game.instance.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallFlyingState.BallDropState.angleDropKick) * t + initPos.y,
												(ownerDirection.z * ball.multiplierDropKick.y + Mathf.Sin(angleX)) * t + initPos.z);
	}

	private void doUpAndUnder(float t)
	{
		ball.transform.position = new Vector3( (ownerDirection.x * ball.multiplierDropUpAndUnder.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x,
												acceleration * 9.81f * t * t + ball.multiplierDropUpAndUnder.x * Mathf.Sin(Mathf.Deg2Rad * ball.angleDropUpAndUnder) * t + initPos.y,
												(ownerDirection.z * ball.multiplierDropUpAndUnder.y + Mathf.Sin(angleX)) * t + initPos.z);
	}
	
	private void CalculateCircle()
	{
		switch (type)
		{
			case TYPEOFDROP.KICK: drawCircle(ball.multiplierDropKick, Game.instance.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallFlyingState.BallDropState.angleDropKick);
				break;
			case TYPEOFDROP.UPANDUNDER: drawCircle(ball.multiplierDropUpAndUnder, ball.angleDropUpAndUnder);
				break;
			default: break;
		}
	}
	
	private void drawCircle(Vector2 multiplier, float angle)
	{
		float a = acceleration * 9.81f;
		float b = multiplier.x * Mathf.Sin(Mathf.Deg2Rad * angle);
		float c = initPos.y - 0.4f;
		float delta = b*b-4*a*c;
		
		float t = 0f;
		if (delta >= 0)
		{
			t = Mathf.Max((-b + Mathf.Sqrt(delta))/(2*a),(-b - Mathf.Sqrt(delta))/(2*a));
		}
		t = (t > 0 ? t : 0f);
		
		Vector3 pos;
		pos.x = (ownerDirection.x * multiplier.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x;
		pos.y = 0.4f;
		pos.z = (ownerDirection.z * multiplier.y + Mathf.Sin(angleX)) * t + initPos.z;
		
		ball.CircleDrop.transform.position = pos;
		ball.CircleDrop.SetActive(true);
	}
	
}
