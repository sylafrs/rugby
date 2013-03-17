using UnityEngine;
using System.Collections;

/**
 * @class Game
 * @brief Trigger des lignes de touche
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Game/Touche")]
public class Touche : TriggeringTrigger
{
    public Transform a, b;

    public override void Entered(Triggered t)
    {
        Ball ball = t.GetComponent<Ball>();
        if (ball != null)
        {
			this.gameObject.SendMessageUpwards("OnTouch", this, SendMessageOptions.DontRequireReceiver);
        }
    }
    void OnDrawGizmos()
    {
        if (a == null || b == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(a.position, b.position);
    }
}