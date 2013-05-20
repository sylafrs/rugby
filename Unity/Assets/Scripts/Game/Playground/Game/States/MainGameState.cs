using UnityEngine;

/**
  * @class MainGameState
  * @brief Etat principal de la caméra.
  * @author Sylvain Lafon
  * @see GameState
  */
public class MainGameState : GameState {

    public MainGameState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	/*
	public override bool OnIntroLaunch()
    {
		Debug.Log("check intro start");
		sm.state_change_son(this, new IntroState(sm, cam, game));
		return true;
	}
	*/
	
	/*
	public override bool OnEndLaunch()
    {
		sm.state_change_son(this, new EndState(sm, cam, game));
		return true;
	}
	*/
	
	/*
	public override bool OnIntroEnd()
    {
		//sm.state_change_son(this, new WaitingState(sm, cam, game));
		Debug.Log("check intro end");
		sm.state_change_son(this, new PlayingState(sm, cam, game));
		return true;
	}
	*/
}

