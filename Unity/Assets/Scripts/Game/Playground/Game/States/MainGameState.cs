using UnityEngine;
using System.Collections.Generic;

/**
  * @class MainGameState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class MainGameState : GameState {

    public MainGameState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

    public override void OnEnter()
    {
        sm.state_change_son(this, new RunningState(sm, cam, game));
    }

    public override bool OnPass(Unit from, Unit to)
    {
        sm.state_change_son(this, new PassingState(sm, cam, game, from, to));
        return true;
    }
}
