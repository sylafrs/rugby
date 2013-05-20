using UnityEngine;
using System.Collections.Generic;

/**
  * @class BallHandlingState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class BallHandlingState : GameState
{
    public BallHandlingState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
}
