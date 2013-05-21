using UnityEngine;
using System.Collections.Generic;

/**
  * @class AimingConversionState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class AimingConversionState : GameState
{
    public AimingConversionState(StateMachine sm, CameraManager cam, Game game, Zone z)
        : base(sm, cam, game)
    {
        this.zone = z;
    }

    private Zone zone;
}
