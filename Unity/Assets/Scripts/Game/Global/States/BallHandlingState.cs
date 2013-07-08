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

    public override void OnEnter()
    {
		base.OnEnter();
        sm.state_change_son(this, new GainGroundingState(sm, cam, game));
    }

    public override bool OnDodgeFinished(Unit u)
    {
        sm.state_change_son(this, new GainGroundingState(sm, cam, game));
        return true;
    }

    public override bool OnDodge(Unit u)
    {
        sm.state_change_son(this, new DodgingState(sm, cam, game));
        return true;
    }

    public override void OnUpdate()
    {
        var p1 = this.game.southTeam.Player;
        var p2 = this.game.northTeam.Player;

        if (p1 != null) p1.UpdateSUPER();
        if (p2 != null) p2.UpdateSUPER();

        if (game.Ball.Owner)
        {
            game.Ball.transform.position = game.Ball.Owner.BallPlaceHolderRight.transform.position;
        }
    }
}
