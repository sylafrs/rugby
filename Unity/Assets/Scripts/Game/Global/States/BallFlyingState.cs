using UnityEngine;
using System.Collections.Generic;

/**
  * @class BallFlyingState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class BallFlyingState : GameState {
    public BallFlyingState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	public override void OnEnter ()
	{
		base.OnEnter();
		game.Ball.ActivateTrail();
		sm.state_change_son(this, new BallDropState(sm, cam, game));
	}
}
