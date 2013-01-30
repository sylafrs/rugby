using UnityEngine;
using System.Collections;

/**
 * @class rotateMe
 * @brief Fait une rotation sur un axe donné
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/Transformations/rotateMe")]
public class rotateMe : MonoBehaviour {

	/*public enum POSITIONCAMERA
	{
		FREE, //libre
		AHEAD, //devant
		BEHIND, //derrière
		LEFT, //gauche
		RIGHT //droite
	}*/

	float remainingDegres = 0;
	Vector3 axis;
	//Vector2
	float degresPerSecond;
	//Vector3 center;

	public float seconds = 1;
	//public float distanceCameraBall = 3.0f;

	//public void rotate(Vector3 center, float degres)
	public void rotate(Vector3 axis, float degres)
	{
		if (remainingDegres == 0 || axis == this.axis)
		//if (remainingDegres == 0 || center == this.center)
		{
			//this.center = center;
			this.axis = axis;
			this.remainingDegres += degres;
			this.degresPerSecond = degres / seconds;
		}
	}

	void Update () 
	{
		if (remainingDegres > 0)
		{			
			float degres = degresPerSecond * Time.deltaTime;
			if (remainingDegres <= degres)
			{
				degres = remainingDegres;
			}

			this.transform.RotateAround(axis, degres / 180f * Mathf.PI);
			//this.ChangePosition(POSITIONCAMERA.BEHIND, degres * Mathf.PI / 180f);
			remainingDegres -= degres;
		}
	}
	/*
	public void ChangePosition(POSITIONCAMERA position, float radians)
	{
		switch (position)
		{
			case POSITIONCAMERA.FREE : 
				break;
			case POSITIONCAMERA.AHEAD : 
				break;
			case POSITIONCAMERA.BEHIND :
				this.transform.position = new Vector3(center.x + Mathf.Cos(radians), this.transform.position.y, center.z + Mathf.Sin(radians));
				break;
			case POSITIONCAMERA.LEFT : 
				break;
			case POSITIONCAMERA.RIGHT: 
				break;
			default:
				Debug.LogError("ERROR ChangePosition in rotateMe.cs");
				break;
		}
		//Camera.mainCamera.transform.LookAt(this.transform);
	}
	 */
}
