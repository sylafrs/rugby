using UnityEngine;
using System.Collections.Generic;

/**
  * @class PassGameState
  * @brief Etat de la caméra lorsque l'on fait une passe
  * @author Sylvain Lafon
  * @see GameState
  */
public class PassingState : GameState {
    public PassingState(StateMachine sm, CameraManager cam, Game game, Unit from, Unit to) : base(sm, cam, game) {
      //  this.from = from;
      //  this.to = to;
    }

    //private Unit from, to;

    public override void OnEnter()
    {
        cam.setTarget(cam.game.Ball.transform);
    }

    public override void OnUpdate()
    {
        var p1 = this.game.southTeam.Player;
        var p2 = this.game.northTeam.Player;

        if (p1 != null) p1.myUpdate();
        if (p2 != null) p2.myUpdate();
    }
}
