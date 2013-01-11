using UnityEngine;
using System.Collections;

/**
 * @class StateMachine_debugger
 * @brief Affiche les différents états d'une StateMachine dans le GUI
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/AI/Debug/StateMachine Debugger")]
public class StateMachine_debugger : MonoBehaviour {

    public StateMachine sm;
    public Rect r;
    public float yy;

    void OnGUI()
    {
        if (sm == null) return;

        int y = 0;
        string str = sm.GetStateName(y);

        Rect rr = new Rect(r);

        while (str != null)
        {
            GUI.Label(new Rect(rr.x, rr.y, rr.width, rr.height), str);
            rr.y += yy;            
            str = sm.GetStateName(++y);
        }
    }
}
