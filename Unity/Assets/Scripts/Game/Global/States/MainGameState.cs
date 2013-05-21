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

    public override bool OnTackle()
    {
        sm.state_change_son(this, new TacklingState(sm, cam, game));
        return true;
    }

    public override bool OnNewOwner(Unit old, Unit current)
    {
        if (current)
        {
            sm.state_change_son(this, new RunningState(sm, cam, game));
            return true;
        }

        return false;
    }

    public override bool OnBallOnGround(bool onGround)
    {
        if (!onGround)
            return false;

        sm.state_change_son(this, new RunningState(sm, cam, game));
        return true;
    }

    public override void OnUpdate()
    {
        var p1 = this.game.right.Player;
        var p2 = this.game.left.Player;
       
        if (p1) p1.myUpdate();
        if (p2) p2.myUpdate();
    }
}