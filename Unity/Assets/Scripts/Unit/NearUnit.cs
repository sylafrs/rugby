using UnityEngine;
using System.Collections;

/**
 * @class NearUnit
 * @brief Trigger d'unite : proche 
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Unit/Near")]
public class NearUnit : Trigger {

    public Unit theUnit;

    public override void Start()
    {
        this.triggering = theUnit;
        base.Start();
    }
}
