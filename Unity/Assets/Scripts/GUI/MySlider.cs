using UnityEngine;
using System.Collections.Generic;

/**
  * @class MySlider
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class MySlider : MonoBehaviour
{
    public UITexture bar;
    public UITexture empty;
    public UITexture full;
    public UITexture[] fullIndicators;

    public float fullScaleZ;
    public float emptyScaleZ;

    public float percent;

    public void Update()
    {
        float p = Mathf.Clamp01(percent);

        if (bar != null)
        {
            Vector3 s = bar.transform.localScale;
            s.x = p * fullScaleZ;
            bar.transform.localScale = s;

            if (bar.pivot == UIWidget.Pivot.Left)
            {
                bar.uvRect = new Rect(0, 0, p, 1);
            }
            if (empty.pivot == UIWidget.Pivot.Right)
            {
                bar.uvRect = new Rect(1 - p, 0, p, 1);
            }
        }

        if (full != null)
        {
            full.gameObject.SetActive(p == 1);
        }

        if (fullIndicators != null)
        {
            foreach (UITexture fullIndicator in fullIndicators)
            {
                if (fullIndicator != null)
                {
                    fullIndicator.gameObject.SetActive(p == 1);
                }
            }
        }

        if (empty != null)
        {
            Vector3 s = empty.transform.localScale;
            s.x = (1 - p) * emptyScaleZ;
            empty.transform.localScale = s;

            if (empty.pivot == UIWidget.Pivot.Left)
            {
                empty.uvRect = new Rect(0, 0, p, 1);
            }
            if (empty.pivot == UIWidget.Pivot.Right)
            {
                empty.uvRect = new Rect(1 - p, 0, p, 1);
            }
        }
    }
}
