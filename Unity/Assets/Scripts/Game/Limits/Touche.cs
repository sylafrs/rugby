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
    public Transform LigneTouche;

    public override void Entered(Triggered t)
    {
        Ball b = t.GetComponent<Ball>();
        if (b != null)
        {
            b.setPosition(Vector3.zero);
        }
    }
}
