using UnityEngine;
using System.Collections;

/**
 * @class rotateMe
 * @brief Fait une rotation sur un axe donné
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/Transformations/rotateMe")]
public class rotateMe : MonoBehaviour {

    float remainingDegres = 0;
    Vector3 axis;    
    float degresPerSecond;

    public float seconds = 1;

    public void rotate(Vector3 axis, float radians)
    {
        if (remainingDegres == 0 || axis == this.axis)
        {
            float degres = radians / 180 * Mathf.PI;
                   
            this.axis = axis;
            this.remainingDegres += degres;
            this.degresPerSecond = degres / seconds;
        }
    }

	void Update () {
        if (remainingDegres > 0)
        {
            float degres = degresPerSecond * Time.deltaTime;
            if (remainingDegres <= degres)
            {
                degres = remainingDegres;
            }

            this.transform.RotateAround(axis, degres);
            remainingDegres -= degres;
        }
	}
}
