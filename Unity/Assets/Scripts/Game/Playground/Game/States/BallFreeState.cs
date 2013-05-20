using UnityEngine;
using System.Collections.Generic;

/**
  * @class BallFreeState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class BallFreeState : GameState {
    public BallFreeState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
}
