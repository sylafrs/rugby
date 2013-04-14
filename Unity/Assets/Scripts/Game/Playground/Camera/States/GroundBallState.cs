using UnityEngine;
using System.Collections.Generic;

/**
  * @class GroundBallState
  * @brief Etat de la caméra lorsque la balle est par terre
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class GroundBallState : CameraState {

    public GroundBallState(StateMachine sm, CameraManager cam) : base(sm, cam) { }

    public override bool OnNewOwner(Unit old, Unit current)
    {
        if (current != null)
        {
            sm.state_change_me(this, new FollowPlayerState(sm, cam));
            return true;
        }

        return false;
    }

}
