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
            UnitAnimator ua = (UnitAnimator)o;
            bool isOwner = false, isControlled = false, isTackled = false;
            Color c = GUI.color;
            if(ua.unit.isOwner()) {
                isOwner = true;
            }
            if(ua.unit.isTackled) {
                isTackled = true;
            }
            if(ua.unit.isControlled()) {
                isControlled = true;
            }

            if (isOwner)
            {
                GUI.color = Color.cyan;
            }
            else if (isTackled)
            {
                if (isControlled)
                {
                    GUI.color = new Color(1f, 0.54f, 0f);
                }
                else
                {
                    GUI.color = Color.red;
                }
            }
            else if (isControlled)
            {
                GUI.color = Color.yellow;
            }

            EditorGUILayout.ObjectField(o.name, ua.animator, typeof(Animator), true);
        
            if(isOwner || isControlled || isTackled)
                GUI.color = c;
        }
#endif
    }
}
