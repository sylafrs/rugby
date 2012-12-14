using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Models/Debug/Unit Debugger")]
public class Unit_debugger : MonoBehaviour {
    public Unit unit;

    public Rect r;
    public float yy;

    void OnGUI()
    {
        if (unit == null) return;

        Rect rr = new Rect(r);
        string str;

        str = "Ordre : " + unit.GetOrder().type.ToString();
        GUI.Label(new Rect(rr.x, rr.y, rr.width, rr.height), str);
        rr.y += yy;

        switch (unit.GetOrder().type)
        {
            case Order.TYPE.DEPLACER:
                str = "Point : " + unit.GetOrder().point.ToString();
                GUI.Label(new Rect(rr.x, rr.y, rr.width, rr.height), str);
                rr.y += yy;
                break;

            case Order.TYPE.SUIVRE:
                str = "Cible : " + unit.GetOrder().target.name;
                GUI.Label(new Rect(rr.x, rr.y, rr.width, rr.height), str);
                rr.y += yy;
                break;
        }
    }
}
