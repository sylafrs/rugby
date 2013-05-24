using UnityEngine;
using System.Collections.Generic;

/**
  * @class ShowScrumField
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class ShowScrumField : myMonoBehaviour {
    public GameReferences refs;
    public bool active = false;
    public Color color;
    public float Y = 0;

    public void OnDrawGizmos() {
        if(!refs || !active)
            return;

        Transform north = refs.positions.scrumFieldNE;
        Transform south = refs.positions.scrumFieldSW;
        Transform east = refs.positions.scrumFieldNE;
        Transform west = refs.positions.scrumFieldSW;

        if (!east || !west || !north || !south)
            return;

        float e = Mathf.Min(east.position.x, west.position.x);
        float w = Mathf.Max(east.position.x, west.position.x);
        float n = Mathf.Max(north.position.z, south.position.z);
        float s = Mathf.Min(north.position.z, south.position.z);

        Vector3 NW = new Vector3(w, Y, n);
        Vector3 NE = new Vector3(e, Y, n);
        Vector3 SW = new Vector3(w, Y, s);
        Vector3 SE = new Vector3(e, Y, s);

        Color prev = Gizmos.color;
        Gizmos.color = this.color;

        Gizmos.DrawLine(NW, NE);
        Gizmos.DrawLine(NE, SE);
        Gizmos.DrawLine(SE, SW);
        Gizmos.DrawLine(SW, NW);
        
        Gizmos.color = prev;
    }
}
