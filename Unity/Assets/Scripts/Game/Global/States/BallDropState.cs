using UnityEngine;
using System.Collections.Generic;

/**
  * @class BallDropState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class BallDropState : GameState {
    public BallDropState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
}
