using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/**
  * @class PauseInput
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class GetAnimators : myMonoBehaviour, Debugable
{
    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        Object [] objs = GameObject.FindObjectsOfType(typeof(UnitAnimator));
        foreach (var o in objs)
        {
            EditorGUILayout.ObjectField(o.name, ((UnitAnimator)o).animator, typeof(Animator), true);
        }
#endif
    }
}
