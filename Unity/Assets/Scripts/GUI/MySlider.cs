using UnityEngine;
using System.Collections.Generic;

/**
  * @class MySlider
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class MySlider : MonoBehaviour {
    public UITexture full;
    public float fullScaleZ;

    public float percent;

    public void Update()
    {
        Vector3 s = full.transform.localScale;
        s.x = Mathf.Clamp01(percent) * fullScaleZ;
        full.transform.localScale = s;
    }
}
