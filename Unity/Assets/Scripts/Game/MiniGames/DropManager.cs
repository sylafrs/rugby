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
	public bool afterCollision = false;
	public float timeOffset = 0f;
	Vector3 directionAfterCollision;

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
		Vector3 pos1 = ball.transform.position;
		pos1.x += 0.5f;
		Vector3 pos2 = ball.transform.position;
		pos2.x -= 0.5f;
		if (Physics.Raycast(ball.transform.position, new Vector3(posBallEndDrop.x - ball.transform.position.x, ball.transform.position.y, posBallEndDrop.z - ball.transform.position.z), out hit, posBallEndDrop.magnitude, ball.poteauLayer.value) ||
			Physics.Raycast(pos1, new Vector3(posBallEndDrop.x - pos1.x, pos1.y, posBallEndDrop.z - pos1.z), out hit, posBallEndDrop.magnitude, ball.poteauLayer.value) ||
			Physics.Raycast(pos2, new Vector3(posBallEndDrop.x - pos2.x, pos2.y, posBallEndDrop.z - pos2.z), out hit, posBallEndDrop.magnitude, ball.poteauLayer.value))
		{
			hitPosGoalPost = hit.point;
			Debug.Log("RAYCAST : " + hit.collider.gameObject.name);
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
					if (Others.nearlyEqual(newPos.x, hitPosGoalPost.x, 0.01f) && Others.nearlyEqual(newPos.z, hitPosGoalPost.z, 0.01f))
					{
						
						if (!afterCollision)
						{
							timeOffset = t;
							initPos = newPos;
							if (Game.instance.rand.Next(100) > 50)
								directionAfterCollision.x = (float)Game.instance.rand.NextDouble();
							else
								directionAfterCollision.x = -(float)Game.instance.rand.NextDouble();

							directionAfterCollision.z = -(float)Game.instance.rand.NextDouble();
						}
						afterCollision = true;
						newPos = new Vector3((directionAfterCollision.x * ball.multiplierDropKick.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * (t - timeOffset) + initPos.x,
												acceleration * 9.81f * (t - timeOffset) * (t - timeOffset) + ball.multiplierDropKick.x * Mathf.Sin(Mathf.Deg2Rad * Game.instance.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallFlyingState.angleDropKick / 2) * (t - timeOffset) + initPos.y,
												(directionAfterCollision.z * ball.multiplierDropKick.y + Mathf.Sin(angleX)) * (t - timeOffset) + initPos.z);
						ball.transform.position = newPos;
					}
					else
					{
						if (afterCollision)
						{
							float toto = t - timeOffset;
							newPos = new Vector3((directionAfterCollision.x * ball.multiplierDropKick.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * (t - timeOffset) + initPos.x,
												acceleration * 9.81f * (t - timeOffset) * (t - timeOffset) + ball.multiplierDropKick.x * Mathf.Sin(Mathf.Deg2Rad * Game.instance.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallFlyingState.angleDropKick / 2) * (t - timeOffset) + initPos.y,
												(directionAfterCollision.z * ball.multiplierDropKick.y + Mathf.Sin(angleX)) * (t - timeOffset) + initPos.z);
							Debug.DrawRay(ball.transform.position, newPos - ball.transform.position, Color.red, 10f);
							ball.transform.position = newPos;
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
		Vector3 newPos = new Vector3((ownerDirection.x * ball.multiplierDropUpAndUnder.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * t + initPos.x,
												acceleration * 9.81f * t * t + ball.multiplierDropUpAndUnder.x * Mathf.Sin(Mathf.Deg2Rad * ball.angleDropUpAndUnder) * t + initPos.y,
												(ownerDirection.z * ball.multiplierDropUpAndUnder.y + Mathf.Sin(angleX)) * t + initPos.z);
		switch (res)
		{
			case RESULT.COLLISION:
				{
					if (Others.nearlyEqual(newPos.x, hitPosGoalPost.x, 0.01f) && Others.nearlyEqual(newPos.z, hitPosGoalPost.z, 0.01f))
					{

						if (!afterCollision)
						{
							timeOffset = t;
							initPos = newPos;
							ball.transform.forward = -ball.transform.forward;

							if (Game.instance.rand.Next(100) > 50)
								directionAfterCollision.x = (float)Game.instance.rand.NextDouble();
							else
								directionAfterCollision.x = -(float)Game.instance.rand.NextDouble();

							directionAfterCollision.z = -(float)Game.instance.rand.NextDouble();
						}
						afterCollision = true;
						newPos = new Vector3((directionAfterCollision.x * ball.multiplierDropUpAndUnder.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * (t - timeOffset) + initPos.x,
												acceleration * 9.81f * (t - timeOffset) * (t - timeOffset) + ball.multiplierDropUpAndUnder.x * Mathf.Sin(Mathf.Deg2Rad * ball.angleDropUpAndUnder) * (t - timeOffset) + initPos.y,
												(directionAfterCollision.z * ball.multiplierDropUpAndUnder.y + Mathf.Sin(angleX)) * (t - timeOffset) + initPos.z);
						ball.transform.position = newPos;
					}
					else
					{
						if (afterCollision)
						{
							newPos = new Vector3((directionAfterCollision.x * ball.multiplierDropUpAndUnder.y + (angleX >= 0f ? Mathf.Cos(angleX) : -Mathf.Cos(angleX))) * (t - timeOffset) + initPos.x,
												acceleration * 9.81f * (t - timeOffset) * (t - timeOffset) + ball.multiplierDropUpAndUnder.x * Mathf.Sin(Mathf.Deg2Rad * ball.angleDropUpAndUnder) * (t - timeOffset) + initPos.y,
												(directionAfterCollision.z * ball.multiplierDropUpAndUnder.y + Mathf.Sin(angleX)) * (t - timeOffset) + initPos.z);
							Debug.DrawRay(ball.transform.position, newPos - ball.transform.position, Color.red, 10f);
							ball.transform.position = newPos;
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
