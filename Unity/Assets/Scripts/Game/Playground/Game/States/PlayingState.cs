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
        Debug.Log("Playing State : onEnter");
		sm.state_change_son(this, new WaitingState(sm, cam, game, game.settings.timeToSleepAfterIntro));
    }
}
