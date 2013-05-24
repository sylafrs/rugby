using UnityEngine;
using System.Collections;

public class DropManager
{

	Ball ball;
	Unit owner;
	TYPEOFDROP type;
	Vector3 ownerDirection;
	Vector3 initPos;
	float angleX;
	float acceleration;
	RESULT res;
	Vector3 hitPosGoalPost;
	bool afterCollision = false;

	public enum TYPEOFDROP
	{
		UPANDUNDER,
		KICK
	}

	public enum RESULT
	{
		SUCCESS,
		COLLISION
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
		Vector3 posBallEndDrop;

		float randAngle = Random.Range(-ball.randomLimitAngle, ball.randomLimitAngle);
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

		acceleration = ball.accelerationDrop > 0f ? -ball.accelerationDrop : ball.accelerationDrop;
		switch (type)
		{
			case TYPEOFDROP.KICK:
				posBallEndDrop = positionBallEndDrop(ball.multiplierDropKick, Game.instance.settings.GameStates
																		  .MainState
																		  .PlayingState
																		  .MainGameState
																		  .RunningState
																		  .BallFreeState
																		  .BallFlyingState
																		  .angleDropKick);
				break;
			case TYPEOFDROP.UPANDUNDER:
				posBallEndDrop = positionBallEndDrop(ball.multiplierDropUpAndUnder, ball.angleDropUpAndUnder);
				break;
			default:
				posBallEndDrop = Vector3.zero;
				break;
		}
		collisionGoalPost(posBallEndDrop);
		if (hitPosGoalPost == Vector3.zero)
		{
			res = RESULT.SUCCESS;
			drawCircle(posBallEndDrop);
		}
		else
		{
			res = RESULT.COLLISION;
			Debug.Log("pos collision " + hitPosGoalPost);
		}
	}

	public bool collisionGoalPost(Vector3 posBallEndDrop)
	{
		RaycastHit hit;
		Debug.DrawRay(ball.transform.position, ownerDirection * posBallEndDrop.magnitude, Color.yellow, 10f);
		if (Physics.Raycast(ball.transform.position, new Vector3(posBallEndDrop.x - ball.transform.position.x, ball.transform.position.y, posBallEndDrop.z - ball.transform.position.z), out hit, posBallEndDrop.magnitude, 1 << owner.game.northTeam.But.gameObject.layer))
		{
			hitPosGoalPost = hit.point;
			return true;
		}
		hitPosGoalPost = Vector3.zero;
		return false;
	}

	// Update is called once per frame
	public void doDrop(float t)
	{
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
		Vector3 newPos = new Vector3((ownerDirection.x * ball.multiplierDropKick.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x,
												acceleration * 9.81f * t * t + ball.multiplierDropKick.x * Mathf.Sin(Mathf.Deg2Rad * Game.instance.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallFlyingState.angleDropKick) * t + initPos.y,
												(ownerDirection.z * ball.multiplierDropKick.y + Mathf.Sin(angleX)) * t + initPos.z);
		switch (res)
		{
			case RESULT.COLLISION:
				{
					Debug.DrawRay(new Vector3(hitPosGoalPost.x, 0f, hitPosGoalPost.z), Vector3.up*100f, Color.red, 10f);
					if (Others.nearlyEqual(newPos.x, hitPosGoalPost.x, 0.01f) && Others.nearlyEqual(newPos.z, hitPosGoalPost.z, 0.01f))
					{
						if (!afterCollision)
							initPos = ball.transform.position;
						afterCollision = true;
						Debug.Log("COLLISION");
						ball.transform.position = new Vector3((-ownerDirection.x * ball.multiplierDropKick.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x,
												acceleration * 9.81f * t * t + ball.multiplierDropKick.x*2 * Mathf.Sin(Mathf.Deg2Rad * Game.instance.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallFlyingState.angleDropKick) * t + initPos.y,
												(-ownerDirection.z * ball.multiplierDropKick.y + Mathf.Sin(angleX)) * t + initPos.z);
					}
					else
					{
						if (afterCollision)
						{
							ball.transform.position = new Vector3((-ownerDirection.x * ball.multiplierDropKick.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x,
												acceleration * 9.81f * t * t + ball.multiplierDropKick.x * 2 * Mathf.Sin(Mathf.Deg2Rad * Game.instance.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallFlyingState.angleDropKick) * t + initPos.y,
												(-ownerDirection.z * ball.multiplierDropKick.y + Mathf.Sin(angleX)) * t + initPos.z);
						}
						else
						{
							ball.transform.position = newPos;
						}
					}
					break;
				}
			case RESULT.SUCCESS:
			default:
				{
					ball.transform.position = newPos;
					break;
				}
		}
		
	}

	private void doUpAndUnder(float t)
	{
		ball.transform.position = new Vector3((ownerDirection.x * ball.multiplierDropUpAndUnder.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x,
												acceleration * 9.81f * t * t + ball.multiplierDropUpAndUnder.x * Mathf.Sin(Mathf.Deg2Rad * ball.angleDropUpAndUnder) * t + initPos.y,
												(ownerDirection.z * ball.multiplierDropUpAndUnder.y + Mathf.Sin(angleX)) * t + initPos.z);
	}

	private Vector3 positionBallEndDrop(Vector2 multiplier, float angle)
	{
		Vector3 pos;
		float a = acceleration * 9.81f;
		float b = multiplier.x * Mathf.Sin(Mathf.Deg2Rad * angle);
		float c = initPos.y - 0.4f;
		float delta = b * b - 4 * a * c;

		float t = 0f;
		if (delta >= 0)
		{
			t = Mathf.Max((-b + Mathf.Sqrt(delta)) / (2 * a), (-b - Mathf.Sqrt(delta)) / (2 * a));
		}
		t = (t > 0 ? t : 0f);

		pos.x = (ownerDirection.x * multiplier.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x;
		pos.y = 0.4f;
		pos.z = (ownerDirection.z * multiplier.y + Mathf.Sin(angleX)) * t + initPos.z;

		return pos;
	}

	private void drawCircle(Vector3 pos)
	{
		ball.CircleDrop.transform.position = pos;
		ball.CircleDrop.SetActive(true);
	}

}
