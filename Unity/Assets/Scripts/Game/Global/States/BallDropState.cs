using UnityEngine;
using System.Collections.Generic;

/**
  * @class BallDropState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class BallDropState : GameState {
    public BallDropState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	public override void OnEnter ()
	{
		base.OnEnter();
		cam.LoadParameters(game.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallDropCamSettings);
        game.Ball.audio.PlayOneShot(game.refs.sounds.ShootBall);
	}
}