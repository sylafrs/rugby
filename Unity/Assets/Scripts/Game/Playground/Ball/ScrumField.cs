using UnityEngine;
using System.Collections;

/**
 * @class ScrumField
 * @brief Champs de mêlée
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Ball/ScrumField")]
public class ScrumField : Trigger
{
    public Ball theBall;

    public override void Start()
    {
        this.triggering = theBall;
        base.Start();
    }
}
