using UnityEngine;
using System.Collections.Generic;

/**
  * @class AnimateScaleY
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class AnimateScaleY : MonoBehaviour {

    public float finalScale;
    public float timeToScale;
    private float timeRemaining;

    public void changeYScale(float y)
    {
        Vector3 tmp = this.transform.localScale;
        tmp.y = y;
        this.transform.localScale = tmp;
    }
    
    public void OnEnable()
    {
        changeYScale(0);
        timeRemaining = timeToScale;
    }

    public void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining > 0)
            {
                float percent = (timeToScale - timeRemaining) / timeToScale;
                changeYScale(percent * finalScale);
            }
            else
            {
                changeYScale(finalScale);
            }
        }
    }
}
