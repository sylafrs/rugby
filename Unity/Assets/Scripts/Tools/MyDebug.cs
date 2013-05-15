using UnityEngine;
using System.Collections.Generic;

/**
  * @class MyDebug
  * @brief Description.
  * @author Sylvain Lafon
  */
public static class MyDebug {

    public static bool enabled = true;

    public static void Log(object o)
    {
        if(enabled)
            Debug.Log(o);
    }
}
