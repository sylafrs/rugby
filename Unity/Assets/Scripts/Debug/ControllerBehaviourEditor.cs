using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
/*
[CustomEditor(typeof(ControllerBehaviour))]
public class ControllerBehaviourEditor : Editor {
	private ControllerBehaviour controller;
	private Color colorHitbox1 = Color.blue;
	private Color colorHitbox2 = Color.green;
	private Color colorHitbox3 = Color.red;
	private float radiusHitbox1 = 2;
	private float radiusHitbox2 = 4;
	private float radiusHitbox3 = 6;
	private bool display = false;

	void OnEnable()
	{
		this.controller = (ControllerBehaviour)this.target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.BeginHorizontal();
		this.display = EditorGUILayout.Toggle("Display", this.display);
		if (this.display)
		{
			this.colorHitbox1 = EditorGUILayout.ColorField(this.colorHitbox1);
			this.radiusHitbox1 = EditorGUILayout.FloatField(this.radiusHitbox1);
			this.colorHitbox2 = EditorGUILayout.ColorField(this.colorHitbox2);
			this.radiusHitbox2 = EditorGUILayout.FloatField(this.radiusHitbox2);
			this.colorHitbox3 = EditorGUILayout.ColorField(this.colorHitbox3);
			this.radiusHitbox3 = EditorGUILayout.FloatField(this.radiusHitbox3);
		}
		EditorGUILayout.EndHorizontal();
	}

	class Data
	{
		public Color Color { get; set; }
		public float Radius { get; set; }
	}

	void OnSceneGUI()
	{
		List<Data> elements = new List<Data>();
		if (this.display)
		{
			elements.Add(new Data { Color = this.colorHitbox1, Radius = this.radiusHitbox1 });
			elements.Add(new Data { Color = this.colorHitbox2, Radius = this.radiusHitbox2 });
			elements.Add(new Data { Color = this.colorHitbox3, Radius = this.radiusHitbox3 });
		}

		foreach (var d in elements.OrderByDescending(d => d.Radius))
		{
			Handles.color = d.Color;
			Handles.DrawSolidDisc(this.controller.transform.position, Vector3.up, d.Radius);
		}
		
		SceneView.RepaintAll();
	}

	public float getRadiusHaloBall()
	{
		return radiusHitbox1;
	}

	public float getRadiusHaloDown()
	{
		return radiusHitbox2;
	}

	public float getRadiusHelpAlly()
	{
		return radiusHitbox3;
	}
}
*/