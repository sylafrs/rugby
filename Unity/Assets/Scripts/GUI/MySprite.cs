using UnityEngine;
using System.Collections.Generic;

/**
  * @class MySpire
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
[RequireComponent(typeof(UITexture))]
public class MySprite : MonoBehaviour {

    private UITexture uiTexture;
    private int nImages;

    public float timeToChange;
    public int cols, rows;

    private int n;
    private float timeRemaining;

    private Rect r;
    
    public void OnEnable()
    {
        if (cols <= 0 || rows <= 0)
        {
            this.enabled = false;
            return;
        }

        uiTexture = this.GetComponent<UITexture>();
        nImages = cols * rows;
        r = new Rect(0, 0, 1.0f / cols, 1.0f / rows);

        ChangeTo(0);
    }

    public void ChangeTo(int i)
    {
        if (i >= nImages)
        {
            i = 0;
        }

        n = i;
        
        timeRemaining = timeToChange;

        int row = i / cols;
        int col = i % cols;

        r.x = r.width * col;
        r.y = r.height * row;

        uiTexture.uvRect = r;
    }

    public void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0)
        {
            ChangeTo(n+1);
        }
    }
}
