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
	
    public override void OnEnter()
    {
        
		sm.state_change_son(this, new WaitingState(sm, cam, game, game.settings.timeToSleepAfterIntro));
    }

    public override bool OnTouch()
    {
        GameActionState newSon = new GameActionState(sm, cam, game);
        sm.state_change_son(this, newSon);
        newSon.OnTouch();

        return true;
    }
}
