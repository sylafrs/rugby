/**
  * @class PlayingState
  * @brief Etat de la caméra durant le jeu
  * @author Sylvain Lafon
  * @see GameState
  */
using UnityEngine;


public class PlayingState : GameState
{
	public PlayingState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

	// On passe en mode jeu après une petite pause
	public override void OnEnter()
	{
		game.refs.managers.ui.currentState = UIManager.UIState.GameUI;
		sm.state_change_son(this, new WaitingState(sm, cam, game, game.settings.GameStates.MainState.IntroState.timeToSleepAfterIntro));
	}

	// Action de jeu : touche
	public override bool OnTouch(Touche t)
	{
		if (sm.state_has_son(this, typeof(GameActionState)))
		{
			return false;
		}
		else
		{
			GameActionState newSon = new GameActionState(sm, cam, game);
			sm.state_change_son(this, newSon);
			newSon.OnTouch(t);

			return true;
		}
	}

	// Action de jeu : mêlée
	public override bool OnScrum()
	{
		GameActionState newSon = new GameActionState(sm, cam, game);
		sm.state_change_son(this, newSon);
		newSon.OnScrum();

		return true;
	}

	// Action de jeu : essai
	public override bool OnTry(Zone z)
	{
		GameActionState newSon = new GameActionState(sm, cam, game);
		sm.state_change_son(this, newSon);
		newSon.OnTry(z);

		return true;
	}

	// Lorsque l'on est plus 'en jeu'
	public override void OnLeave()
	{
		game.refs.managers.ui.currentState = UIManager.UIState.NULL;
	}
	
	public override bool OnSuper(Team t, SuperList super)
	{
		//freeze les gens
		sm.state_change_son(this, new WaitingState(sm,cam,game,4,t));
		
		//cam stuff
		
		
		
		return true;
	}
}
