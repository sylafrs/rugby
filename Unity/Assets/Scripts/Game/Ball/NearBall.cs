using UnityEngine;
using System.Collections;

[AddComponentMenu("Triggers/Ball/Near")]
public class NearBall : Trigger {

    public Ball theBall;

    public override void Start()
    {
        this.triggering = theBall;
        base.Start();
    }
}
