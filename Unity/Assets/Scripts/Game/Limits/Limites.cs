using UnityEngine;
using System.Collections;

[AddComponentMenu("Triggers/Game/Limites")]
public class Limites : TriggeringTrigger
{

    public override void Entered(Triggered t)
    {
        Ball b = t.GetComponent<Ball>();
        if (b != null)
        {
            // La balle � d�pass� les bornes !
            Debug.Log("Hors limites : [Replace au centre]");
            b.setPosition(Vector3.zero);            
        }
    }
}
