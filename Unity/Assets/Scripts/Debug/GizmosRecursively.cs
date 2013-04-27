using UnityEngine;
using System.Collections.Generic;

/**
  * @class GizmosRecursively
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
[AddComponentMenu("Scripts/Debug/Gizmos (recursively)")]
public class GizmosRecursively : MonoBehaviour {

	public bool active = true;

	public Color [] colorPattern = {
		Color.white,
		Color.red,
		Color.blue,
		Color.green	
	};
	
	public float sizePoint = 1;
	public float lengthForward = 5;
	
	public void OnDrawGizmos() {
		if (!active)
			return;
		
		DrawRecursively(this.transform, 0);
	}
	
	private void DrawRecursively(Transform t, int level) {
		
		DrawGizmos(t, level);
		
		int childCount = t.childCount;
		for(int i = 0; i < childCount; i++) {
			DrawRecursively(t.GetChild(i), level+1);	
		}
	}
	
	private void DrawGizmos(Transform t, int level) {
		Color c = colorPattern[level % colorPattern.Length];
		Gizmos.color = c;
		Gizmos.DrawCube(t.transform.position, Vector3.one * sizePoint);
		Gizmos.DrawLine(t.transform.position, t.transform.position + t.transform.forward * lengthForward);
	}
}
