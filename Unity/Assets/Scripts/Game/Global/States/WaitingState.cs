/**
  * @class Waiting state
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class WaitingState : GameState
{
	public WaitingState(StateMachine sm, CameraManager cam, Game game, float time)
		: base(sm, cam, game)
	{
		remainingTime = time;
	}

	private float remainingTime;

	public override void OnEnter()
	{
		game.disableIA = true;
		game.Referee.PauseIngameTime();
	}

	public override void OnUpdate()
	{
		remainingTime -= UnityEngine.Time.deltaTime;
		if (remainingTime <= 0)
		{
			sm.state_change_me(this, new MainGameState(sm, cam, game));
		}
	}

	public override void OnLeave()
	{
		game.Referee.ResumeIngameTime();
		game.disableIA = false;

		foreach (Unit u in game.northTeam)
			u.buttonIndicator.target.renderer.enabled = false;

		foreach (Unit u in game.southTeam)
			u.buttonIndicator.target.renderer.enabled = false;

		game.northTeam.fixUnits = game.southTeam.fixUnits = false;
		if (game.northTeam.Player != null) game.northTeam.Player.enableMove();
		if (game.southTeam.Player != null) game.southTeam.Player.enableMove();
	}
}