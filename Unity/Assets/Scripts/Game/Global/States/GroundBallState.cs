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
        pos.y = Ball.epsilonOnGround - 0.1f;
        game.Ball.transform.position = pos;

        var p1 = this.game.southTeam.Player;
        var p2 = this.game.northTeam.Player;

        if (p1 != null) p1.UpdateSUPER();
        if (p2 != null) p2.UpdateSUPER();
    }
}
