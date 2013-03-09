using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Ball))]
public class Tackle : Editor
{
	Ball ball;
	public float coneSize = 2f;
	
	void OnEnable()
	{
		ball = (Ball)target;
	}
	
	/*
	void OnSceneGUI ()
	{
		Handles.color = Color.red;
		Quaternion tmp = new Quaternion(0,0,ball.AngleOfFOV,0);
		Handles.ConeCap (0, ball.Owner.transform.position + new Vector3 (-5, 0, 0), ball.Owner.transform.rotation, coneSize);
		Handles.color = Color.green;
		Handles.ConeCap (0, ball.Owner.transform.position + new Vector3 (0, -5, 0), ball.Owner.transform.rotation, coneSize);
		Handles.color = Color.blue;
		Handles.ConeCap (0, ball.Owner.transform.position + new Vector3 (0, 0, -5), ball.Owner.transform.rotation, coneSize);
		
		//Handles.color = GUILayout.
		Handles.DrawSolidDisc(ball.Owner.transform.position, Vector3.up, 1f);
	}
	*/
}
