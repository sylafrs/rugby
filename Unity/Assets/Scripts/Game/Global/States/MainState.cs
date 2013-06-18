using System;
using System.Collections;
using UnityEngine;

/**
  * @class MainGameState
  * @brief Etat principal du jeu.
  * @author Sylvain Lafon
  * @see GameState
  */
public class MainState : GameState {

    public MainState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

    public override void OnEnter()
    {
		base.OnEnter();
        sm.state_change_son(this, new IntroState(sm, cam, game));
    }

    public override bool OnEndSignal()
    {			
        sm.state_change_son(this, new EndState(sm, cam, game));
        return true;
    }
	
	public override bool OnStartSignal()
    {
		sm.state_change_son(this, new PlayingState(sm, cam, game));
		return true;
	}

    public override void OnUpdate()
    {
        if (this.game.northTeam.Player.UpdateRESET())
            return;

        this.game.southTeam.Player.UpdateRESET();
    }
}

