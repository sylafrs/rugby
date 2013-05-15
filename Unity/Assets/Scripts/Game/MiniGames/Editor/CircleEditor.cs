using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CircleSystem))]
public class CircleEditor : Editor {
	
	CircleSystem circle;
	Color circleColor = Color.yellow;
	
	void OnEnable()
	{
		circle = (CircleSystem)target;
	}
	
	void OnSceneGUI ()
	{
		if (circle)
		{
			Handles.color = circleColor;
			Handles.DrawSolidDisc(circle.transform.position, Vector3.up, Mathf.Sqrt(circle.raySquare));
		}
	}

	public override void OnInspectorGUI()
	{
		EditorGUIUtility.LookLikeInspector();
		DrawDefaultInspector();
		
		circleColor = EditorGUILayout.ColorField(circleColor, GUILayout.Width(200));
	}
}
