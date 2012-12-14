using UnityEngine;
using System.Collections;

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
