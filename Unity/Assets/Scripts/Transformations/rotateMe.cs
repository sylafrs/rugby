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
	public float seconds = 1;
	
	public void BeginRotation(Vector3 axis, float angle)
	{
		this.axis = axis;
		this.angle += Mathf.Deg2Rad * angle;
		t = 0;
	}

	void Update () 
	{		
		t += Time.deltaTime;
		if(t > seconds)
			t = seconds;
		
		this.transform.RotateAround(axis, Mathf.LerpAngle(0, angle, t/seconds));	
	}
}
