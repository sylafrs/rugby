using UnityEngine;
using System.Collections.Generic;

/**
  * @class ConversionFlyState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class ConversionFlyState : GameState
{
    public ConversionFlyState(StateMachine sm, CameraManager cam, Game game, Zone z)
        : base(sm, cam, game)
    {
        this.zone = z;
    }

    private Zone zone;
}
