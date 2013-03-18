using UnityEngine;
using System.Collections;
using UnityEditor;

/*
[CustomEditor(typeof(Ball))]
public class Tackle : Editor
{
	Ball ball;
	public float coneSize = 2f;
	public Color col;
	
	void OnEnable()
	{
		ball = (Ball)target;
	}
	
	void OnSceneGUI ()
	{
		Quaternion tmp = new Quaternion(0,0,ball.AngleOfFOV,1);
		Handles.ConeCap (0, ball.Owner.transform.position + new Vector3 (-5, 0, 0), ball.Owner.transform.rotation, coneSize);
		Handles.color = Color.green;
		Handles.ConeCap (0, ball.Owner.transform.position + new Vector3 (0, -5, 0), ball.Owner.transform.rotation, coneSize);
		Handles.color = Color.blue;
		Handles.ConeCap (0, ball.Owner.transform.position + new Vector3 (0, 0, -5), ball.Owner.transform.rotation, coneSize);
		
		
		Handles.color = EditorGUILayout.ColorField(ball.DiscTackle, GUILayout.Width(200));
		Handles.DrawSolidDisc(ball.Owner.transform.position, Vector3.up, ball.sizeOfTackleArea);
	}
}
*/
