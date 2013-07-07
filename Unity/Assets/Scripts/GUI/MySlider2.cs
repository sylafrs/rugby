using UnityEngine;
using System.Collections.Generic;

/**
  * @class MySlider2
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class MySlider2 : MonoBehaviour {

    public PivotObject bar;
    public PivotObject empty;
    public PivotObject full;
    public PivotObject separator;
    public PivotObject[] fullIndicators;

    //public float fullScaleZ;
    //public float emptyScaleZ;

    public float sepMinX;
    public float sepMaxX;

    public float percent;

    public void Update()
    {
        float p = Mathf.Clamp01(percent);

        if (bar != null)
        {
            FitForPercent(bar, p);
        }

        if (separator != null)
        {
            Vector3 pos = separator.transform.localPosition;
            pos.x = sepMinX + (p * (sepMaxX - sepMinX));
            separator.transform.localPosition = pos;
        }

        if (full != null)
        {
            full.gameObject.SetActive(p == 1);
        }

        if (fullIndicators != null)
        {
            foreach (PivotObject fullIndicator in fullIndicators)
            {
                if (fullIndicator != null)
                {
                    fullIndicator.gameObject.SetActive(p == 1);
                }
            }
        }

        if (empty != null)
        {
            FitForPercent(empty, p);
        }
    }

    public void FitForPercent(PivotObject obj, float p)
    {
        Vector2 size = obj.size;

        if (obj.pivot == PivotObject.PIVOT_POSITION.LEFT)
        {
            obj.uvRect = new Rect(0, 0, p, 1);
            size.x = p;
        }
        if (obj.pivot == PivotObject.PIVOT_POSITION.RIGHT)
        {
            obj.uvRect = new Rect(1 - p, 0, p, 1);
            size.x = 1 - p;
        }
        if (obj.pivot == PivotObject.PIVOT_POSITION.BOTTOM)
        {
            obj.uvRect = new Rect(0, 0, 1, p);
            size.y = p;
        }
        if (obj.pivot == PivotObject.PIVOT_POSITION.TOP)
        {
            obj.uvRect = new Rect(0, 1 - p, 1, p);
            size.y = 1 - p;
        }
        obj.size = size;
    }
}
