/**
  * @class ScrumState
  * @brief Etat de la cam�ra durant une m�l�e
  * @author Sylvain Lafon
  * @see GameState
  */
using UnityEngine;

public class ScrumState : GameState
{
	public ScrumState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

	void initCutScene()
	{
		cam.setTarget(this.cam.game.refs.gameObjects.ScrumBloc.transform);
	}

	public override void OnEnter()
	{
		initCutScene();
		// cam.game.Referee.NowScrum();
		game.refs.managers.ui.currentState = UIManager.UIState.ScrumUI;
		game.Referee.ScrumCinematicMovement();
		//cam.transalateWithFade(Vector3.one, Quaternion.identity, 2, 1, 1, 1, 
		//(/* OnFinish*/) => {
		//	game.Referee.NowScrum();
		//}, (/*OnFade*/) => {
		//	game.Referee.SwitchToBloc();
		//});

	}

	public override void OnLeave()
	{
		game.refs.managers.ui.currentState = UIManager.UIState.NULL;
	}
}