using UnityEngine;
using System.Collections.Generic;

/**
  * @class TransfoCamera
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class TransfoCamera : MonoBehaviour {
	public But but;
	
	void OnEnable() {
		this.transform.LookAt(but.transform.FindChild("Transformation LookAt"));	
	}
}
