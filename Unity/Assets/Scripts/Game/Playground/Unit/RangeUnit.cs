using UnityEngine;
using System.Collections.Generic;

/**
  * @class SeeUnit
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
[AddComponentMenu("Triggers/Unit/Range"), RequireComponent(typeof(SphereCollider))]
public class RangeUnit : Trigger
{
    public Unit theUnit;
    public new SphereCollider collider { get; set; }

    public override void Start()
    {
        this.theUnit.triggerTackleFail = this;
        this.triggering = theUnit;
        this.collider = this.GetComponent<SphereCollider>();
        base.Start();
    }
}
