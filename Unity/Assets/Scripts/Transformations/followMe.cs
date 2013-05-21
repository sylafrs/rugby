using UnityEngine;
using System.Collections.Generic;

/**
  * @class followMe
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
[AddComponentMenu("Scripts/Transformations/followMe")]
public class followMe : myMonoBehaviour {

    public Transform target;

    public void Update()
    {
        if (target)
        {
            this.transform.position = target.position;
        }
    }
}
