using UnityEngine;
using System.Collections.Generic;

/**
  * @class MySlider
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class MySlider : MonoBehaviour {
    public UITexture bar;
    public UITexture empty;
    public UITexture full;
    public UITexture [] fullIndicators;

    public float fullScaleZ;
    public float emptyScaleZ;

    public float percent;

    public void Update()
    {
        if (bar != null)
        {
            Vector3 s = bar.transform.localScale;
            s.x = Mathf.Clamp01(percent) * fullScaleZ;
            bar.transform.localScale = s;
        }

        if (full != null)
        {
            full.gameObject.SetActive(Mathf.Clamp01(percent) == 1);
        }

        if (fullIndicators != null)
        {
            foreach (UITexture fullIndicator in fullIndicators)
            {
                if (fullIndicator != null)
                {
                    fullIndicator.gameObject.SetActive(Mathf.Clamp01(percent) == 1);
                }
            }
        }

        if (empty != null)
        {
            Vector3 s = empty.transform.localScale;
            s.x = Mathf.Clamp01(1 - percent) * emptyScaleZ;
            empty.transform.localScale = s;
        }
    }
}
