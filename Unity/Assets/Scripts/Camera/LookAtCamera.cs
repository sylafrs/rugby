using UnityEngine;
using System.Collections.Generic;

/**
  * @class LookAtCamera
  * @brief Make the object constantly looking at the camera.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class LookAtCamera : MonoBehaviour {
	
	public Camera fixedCamera;
	public Vector3 up;
	
	public static Camera GetCamera() {
		Camera [] cams = Camera.allCameras;
		int i = 0;
		while(i < cams.Length) {
			if(cams[i].gameObject.activeSelf) {
				return cams[i];
			}
			
			i++;
		}
		
		return null;
	}
		
	void Update() {
		Camera c = fixedCamera;
		if(!c)
			c = GetCamera();
		if(c)
			this.transform.LookAt(c.transform, up);
	}
}
