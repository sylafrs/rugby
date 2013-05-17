using UnityEngine;
using System.Collections.Generic;

/**
  * @class PassGameState
  * @brief Etat de la caméra lorsque l'on fait une passe
  * @author Sylvain Lafon
  * @see GameState
  */
public class PassGameState : GameState {
    public PassGameState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

    public override void OnEnter()
    {
        cam.setTarget(cam.game.Ball.transform);
    }	
}
