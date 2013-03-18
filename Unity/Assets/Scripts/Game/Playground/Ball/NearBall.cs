using UnityEngine;
using System.Collections;

/**
 * @class NearBall
 * @brief Trigger de la balle : proche
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Ball/Near")]
public class NearBall : Trigger {

    public Ball theBall;

    public override void Start()
    {
        this.triggering = theBall;
        base.Start();
    }
}
