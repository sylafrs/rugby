using UnityEngine;
using System.Collections;

public class rotateMe : MonoBehaviour {

    float stillToRotate = 0;
    Vector3 axis;
    float rotateAmount = 1;

    public void rotate(Vector3 axis, float angle)
    {
        if (stillToRotate == 0)
        {
            this.axis = axis;
            this.stillToRotate = angle / 180 * Mathf.PI;
        }
    }

	// Update is called once per frame
	void Update () {
        if (stillToRotate > 0)
        {
            float toRotate = rotateAmount;
            if (stillToRotate <= rotateAmount)
            {
                toRotate = stillToRotate;
            }

            this.transform.RotateAround(axis, toRotate);
            stillToRotate -= toRotate;
        }
	}
}
