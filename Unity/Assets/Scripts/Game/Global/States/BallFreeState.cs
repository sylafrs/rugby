using UnityEngine;
using System.Collections.Generic;

/**
  * @class BallFreeState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class BallFreeState : GameState {
    public BallFreeState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

    public override void OnEnter()
    {  
		base.OnEnter();
        this.decide();
    }

    public override bool OnBallOnGround(bool onGround)
    {
        //this.decide();
		sm.state_change_son(this, new GroundBallState(sm, cam, game));
        return true;
    }

    public void decide()
    {		
		sm.state_change_son(this, new BallFlyingState(sm, cam, game));
		//ball flying ou ground ball
		/*
        DropManager.TYPEOFDROP? drop = this.game.Ball.typeOfDrop;
        if (drop == DropManager.TYPEOFDROP.KICK)
        {
            sm.state_change_son(this, new BallFlyingState(sm, cam, game));
        }
        else if (drop == DropManager.TYPEOFDROP.UPANDUNDER)
        {
            sm.state_change_son(this, new BallUpAndUnderState(sm, cam, game));
        }
        */
    }
}
