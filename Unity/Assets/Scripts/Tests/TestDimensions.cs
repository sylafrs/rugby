using UnityEngine;
using System.Collections.Generic;

/**
  * @class TestDimensions
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class TestDimensions : MonoBehaviour {
    public Transform toucheE;
    public Transform essaiN;
    public Transform terrainSE, terrainNW;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
         if (terrainSE)
             Gizmos.DrawCube(terrainSE.position, Vector3.one);
        if (terrainNW)
            Gizmos.DrawCube(terrainNW.position, Vector3.one);

        if (terrainNW && terrainSE)
        {
            Vector3 sw = new Vector3(terrainNW.position.x, 0, terrainSE.position.z);
            Vector3 ne = new Vector3(terrainSE.position.x, 0, terrainNW.position.z);

            Gizmos.DrawLine(ne, terrainSE.position);
            Gizmos.DrawLine(terrainSE.position, sw);
            Gizmos.DrawLine(sw, terrainNW.position);
            Gizmos.DrawLine(terrainNW.position, ne);

            Gizmos.color = Color.blue;
            if (essaiN)
            {
                Vector3 ene = new Vector3(terrainSE.position.x, 0, essaiN.position.z);
                Vector3 ese = new Vector3(terrainSE.position.x, 0, -essaiN.position.z);
                Vector3 enw = new Vector3(terrainNW.position.x, 0, essaiN.position.z);
                Vector3 esw = new Vector3(terrainNW.position.x, 0, -essaiN.position.z);

                Gizmos.DrawLine(ene, enw);
                Gizmos.DrawLine(ese, esw);

                Gizmos.color = Color.red;
                if (toucheE)
                {
                    Vector3 tne = new Vector3(toucheE.position.x, 0, ene.z);
                    Vector3 tse = new Vector3(toucheE.position.x, 0, ese.z);
                    Vector3 tnw = new Vector3(-toucheE.position.x, 0, ene.z);
                    Vector3 tsw = new Vector3(-toucheE.position.x, 0, ese.z);

                    Gizmos.DrawLine(tne, tse);
                    Gizmos.DrawLine(tnw, tsw);
                }
            }
        }
        
        Gizmos.color = Color.red;
        if (toucheE)        
            Gizmos.DrawCube(toucheE.position, Vector3.one);
       
        Gizmos.color = Color.blue;
        if (essaiN)
            Gizmos.DrawCube(essaiN.position, Vector3.one);
    }
}
