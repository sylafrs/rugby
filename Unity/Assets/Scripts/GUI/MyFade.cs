using UnityEngine;
using System.Collections.Generic;

/**
  * @class MyFade
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class MyFade : MonoBehaviour {
    public UITexture texture;

    public float speed;
    private bool unfade = false;

    public void OnEnable()
    {
        Color c = Color.black;
        c.a = 1;
        texture.material.color = c;

        unfade = false;

        Inverse();
    }

    public void Inverse()
    {
        unfade = !unfade;
    }

    public void Update()
    {
        float s = speed;
        if (unfade)
        {
            s = -speed;
        }

        Color c = texture.material.color;
        c.a = Mathf.Clamp01(c.a + s * Time.deltaTime);
        texture.material.color = c;
    }
}
