using UnityEngine;
using System.Collections;

/**
 * @class NearUnit
 * @brief Trigger d'unite : proche 
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Unit/Near"), RequireComponent(typeof(SphereCollider))]
public class NearUnit : Trigger {

    public Unit theUnit;
	public new SphereCollider collider {get; set;}

    public override void Start()
    {
		this.theUnit.triggerTackle = this;
        this.triggering = theUnit;
		this.collider = this.GetComponent<SphereCollider>();
        base.Start();
    }
}
