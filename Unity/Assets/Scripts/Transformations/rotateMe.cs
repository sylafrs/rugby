using UnityEngine;
using System.Collections;

/**
 * @class rotateMe
 * @brief Fait une rotation sur un axe donné
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/Transformations/rotateMe")]
public class rotateMe : MonoBehaviour {

	Vector3 axis;
	float angle;
	float t;
	public  float seconds = 1;
	private float lastAngle;
	
	
	public void BeginRotation(Vector3 axis, float angle)
	{
		this.axis = axis;
		this.angle = Mathf.Deg2Rad * angle;
		t = 0;
		lastAngle = 0;
	}
	
	

	void Update () 
	{		
		t += Time.deltaTime;
		if(t > seconds)
			t = seconds;
		
		float angleFromZero = Mathf.LerpAngle(0, angle, t/seconds);
		this.transform.RotateAround(axis, angleFromZero - lastAngle);	
		lastAngle = angleFromZero;
	}
}
