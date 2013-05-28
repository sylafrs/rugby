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
    }

    public override void OnEnter()
    {
		cam.LoadParameters(cam.game.settings.GameStates.MainState.PlayingState.MainGameState.PassingState.PassingCamSettings);
    }

    public override void OnUpdate()
    {
		//var p1 = this.game.southTeam.Player;
        //var p2 = this.game.northTeam.Player;
        //
        //if (p1 != null) p1.myUpdate();
        //if (p2 != null) p2.myUpdate();

        var p = this.game.Ball.Team.opponent.Player;
        if (p != null) p.myUpdate(); 
    }
	
	public override bool OnNewOwner(Unit old, Unit current)
    {
        if (current)
        {	
            sm.state_change_son(this, new RunningState(sm, cam, game));
            return true;
        }

        return false;
    }
}
