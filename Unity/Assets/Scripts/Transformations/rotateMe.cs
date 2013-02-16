using UnityEngine;
using System.Collections;

/**
 * @class rotateMe
 * @brief Fait une rotation sur un axe donné
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/Transformations/rotateMe")]
public class rotateMe : myMonoBehaviour {

	Vector3 axis;
	float angle;
	float t;
	
	public float duration = 1;
	public float delay;
	
	private float lastAngle;
	private float waiting;
	private int counter;
	
	public void BeginRotation(Vector3 axis, float angle)
	{
		this.axis = axis;
		this.angle = Mathf.Deg2Rad * angle;
		t = 0;
		lastAngle = 0;
		waiting = 0;
		counter = 0;
	}
	
	
	
	void Update () 
	{		
		waiting += Time.deltaTime;
		counter ++;
		if(waiting >= delay/100){
			t += Time.deltaTime;
			if(t > duration)
				t = duration;
			
			float angleFromZero = Mathf.LerpAngle(0, angle, t/duration);
			this.transform.RotateAround(axis, angleFromZero- lastAngle);	
			lastAngle = angleFromZero;
		}
	}
}
