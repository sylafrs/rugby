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

    public bool remiseAuCentre = false;

    public override void Entered(Triggered t)
    {
        Ball ball = t.GetComponent<Ball>();
        if (ball != null)
        {
            if (a == null || b == null)
            {
                Debug.Log("Touche : [Replace au centre]");
                ball.setPosition(Vector3.zero);
            }
            else
            {
                Debug.Log("Touche : [Replace au centre, sur la ligne]");

                Vector3 pos = Vector3.Project(ball.transform.position - a.position, b.position - a.position) + a.position;
                pos.x = remiseAuCentre ? 0 : pos.y; // Au centre
                pos.y = 0; // A terre
                ball.setPosition(pos);

            }           
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