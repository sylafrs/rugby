using UnityEngine;
using System.Collections.Generic;

/**
  * @class GroundBallState
  * @brief Etat de la cam�ra lorsque la balle est par terre
  * @author Sylvain Lafon
  * @see GameState
  */
public class GroundBallState : GameState {

    public GroundBallState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	public override void OnEnter()
    {  
		base.OnEnter();
 		cam.LoadParameters(game.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.GroundBallCamSettings);
        this.game.Ball.audio.PlayOneShot(game.refs.sounds.BallGroundSound);
    }

    public override void OnUpdate()
    {
        Vector3 pos = game.Ball.transform.position;
        if (game.Ball.nbRebond == -1)
            pos.y = Ball.epsilonOnGround - 0.1f;
        game.Ball.transform.position = pos;
    }
}
