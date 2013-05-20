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
        MyDebug.Log("Main State : on enter");
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
}

