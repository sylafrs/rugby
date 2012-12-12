using UnityEngine;
using System.Collections;

public delegate void callback();

public class GoTo : MonoBehaviour {

    bool sent = false;
    callback whenArrived;
    Transform target;

    public float speed;

	// Update is called once per frame
	void Update () {
	    if(sent) {
            Vector3 movement = target.position - this.transform.position;
            if (movement.magnitude > speed * Time.deltaTime)
            {
                movement = movement / movement.magnitude * speed * Time.deltaTime;
            }

            this.transform.position += movement;
            if (this.transform.position == target.position)
            {
                sent = false;
                this.transform.parent = target;
                if(whenArrived != null) whenArrived();
            }

        }
	}

    public void sendTo(Transform target, callback whenArrived) {        
        this.whenArrived = whenArrived;
        this.target = target;
        this.transform.parent = null;
        sent = true;
    }
}
