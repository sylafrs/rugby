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
    }
}
