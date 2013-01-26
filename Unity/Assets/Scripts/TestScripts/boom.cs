using UnityEngine;
using System.Collections;

[AddComponentMenu("Test/forceOnKeyDown")]
public class boom : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}

    public Vector3 force = Vector3.zero;
    public KeyCode k;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(k))
        {
            this.rigidbody.AddForce(this.transform.forward * force.x + this.transform.up * force.y + this.transform.right * force.z);
        }
	}
}
