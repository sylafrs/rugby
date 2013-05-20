using UnityEngine;
using System.Collections.Generic;

/**
  * @class ConvertingState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class ConvertingState : GameState {

    public ConvertingState(StateMachine sm, CameraManager cam, Game game, Zone z) : base(sm, cam, game) {
        this.zone = z;
    }

    private Zone zone;

}
