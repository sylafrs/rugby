using UnityEngine;
using System.Collections.Generic;

/**
  * @class Clignote
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class Clignote : MonoBehaviour {

    public UITexture texture;
    public float speed;
    private float t;

    private void Awake()
    {
        t = 0;
    }

    private void Update()
    {
        t += Time.deltaTime * speed;
        
        Color c = Color.white;
        c.a = Mathf.Abs(Mathf.Sin(t));

        texture.color = c;
    }
}
