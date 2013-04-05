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
			ball.inTouch = this;
			this.gameObject.SendMessageUpwards("OnTouch", this, SendMessageOptions.DontRequireReceiver);
        }
    }
	
	public override void Left (Triggered o)
	{
		Ball b = o.GetComponent<Ball>();
        if (b != null)
        {
			b.inTouch = null;          
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