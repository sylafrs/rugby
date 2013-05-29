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
		this.remainingTime = time;
		this.TeamOnSuper = null;
	}

	public WaitingState(StateMachine sm, CameraManager cam, Game game, float time, Team TeamOnSuper)
		: base(sm, cam, game)
	{
		this.remainingTime = time;
		this.TeamOnSuper = TeamOnSuper;
	}

	private float remainingTime;
	private Team TeamOnSuper;

	public override void OnEnter()
	{
		game.disableIA = true;
		game.Referee.PauseIngameTime();
		if(TeamOnSuper)
			sm.state_change_son(this, new SuperCutSceneState(sm, cam, game, TeamOnSuper));
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
         
        Team[] teams = new Team[2];
        teams[0] = game.southTeam;
        teams[1] = game.northTeam;

        foreach (Team t in teams)
        {
            foreach (Unit u in t)
            {
                u.typeOfPlayer = Unit.TYPEOFPLAYER.DEFENSE;
            }
        }

        foreach (Team t in teams)
        {            
            foreach (Unit u in t)
            {
                if (t.Player.Controlled && game.Ball.NextOwner != u)
                {
                    u.UpdateTypeOfPlay();
                    u.UpdatePlacement();                        
                }

                u.buttonIndicator.target.renderer.enabled = false;
            }

            t.fixUnits = false;
            t.Player.enableMove();
        }        
	}
}