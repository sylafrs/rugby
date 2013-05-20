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

    public override void OnEnter()
    {
        sm.state_change_son(this, new AimingConversionState(sm, cam, game, zone));
    }

    public override bool OnConversionShot()
    {
        sm.state_change_son(this, new ConversionFlyState(sm, cam, game, zone));
        return true;
    }
}
