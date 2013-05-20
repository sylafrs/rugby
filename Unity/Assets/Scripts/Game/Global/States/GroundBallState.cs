using UnityEngine;
using System.Collections.Generic;

/**
  * @class GroundBallState
  * @brief Etat de la caméra lorsque la balle est par terre
  * @author Sylvain Lafon
  * @see GameState
  */
public class GroundBallState : GameState {

    public GroundBallState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	public override void OnEnter()
    {
 
    }
}
