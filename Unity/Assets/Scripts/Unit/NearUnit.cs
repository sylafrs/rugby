using UnityEngine;
using System.Collections;

[AddComponentMenu("Triggers/Unit/Near")]
public class NearUnit : Trigger {

    public Unit theUnit;

    public override void Start()
    {
        this.triggering = theUnit;
        base.Start();
    }
}
