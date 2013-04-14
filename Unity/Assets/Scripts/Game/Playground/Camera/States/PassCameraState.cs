using UnityEngine;
using System.Collections.Generic;

/**
  * @class PassCameraState
  * @brief Etat de la caméra lorsque l'on fait une passe
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class PassCameraState : CameraState {
    public PassCameraState(StateMachine sm, CameraManager cam) : base(sm, cam) { }

    public override bool OnNewOwner(Unit old, Unit current)
    {
        if (current != null)
        {
            sm.state_change_me(this, new FollowPlayerState(sm, cam));
            return true;
        }

        return false;
    }

    public override bool OnBallOnGround(bool onGround)
    {
        if (onGround)
        {
            sm.state_change_me(this, new GroundBallState(sm, cam));
            return true;
        }

        return false;
    }
}
