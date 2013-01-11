using UnityEngine;
using System.Collections;

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
