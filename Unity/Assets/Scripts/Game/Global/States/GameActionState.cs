using UnityEngine;
using System.Collections.Generic;

/**
  * @class GameActionState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class GameActionState : GameState {

    public GameActionState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

    public override bool OnTouch()
    {
        sm.state_change_son(this, new TouchState(sm, cam, game));
        return true;
    }

    public override bool OnScrum()
    {
        sm.state_change_son(this, new ScrumState(sm, cam, game));
        return true;
    }
}
