using UnityEngine;
using System.Collections.Generic;

/**
  * @class ShowMeleeDistance
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class ShowMeleeDistance : MonoBehaviour {
    public void OnDrawGizmos()
    {
        float d = Game.instance.settings.GameStates.MainState.PlayingState.GameActionState.ScrumingState.MaximumDistance;

        Vector3 center = this.transform.position;
        Vector3 south = center - (Vector3.forward * d);
        Vector3 north = center + (Vector3.forward * d);

        Gizmos.DrawCube(south, Vector3.one);
        Gizmos.DrawCube(north, Vector3.one);
        Gizmos.DrawCube(center, Vector3.one);
        Gizmos.DrawLine(south, north);
    }
}
