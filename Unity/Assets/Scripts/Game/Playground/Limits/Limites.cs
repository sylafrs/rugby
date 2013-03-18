using UnityEngine;
using System.Collections;

/**
 * @class Game
 * @brief Trigger définissant les limites à ne pas franchir
 * @author Sylvain Lafon
 */
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
