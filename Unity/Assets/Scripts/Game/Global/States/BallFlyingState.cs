using UnityEngine;
using System.Collections.Generic;

/**
  * @class BallFlyingState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class BallFlyingState : GameState {
    public BallFlyingState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
}
