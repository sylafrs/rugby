/**
  * @class Waiting state
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class WaitingState : GameState
{
    public WaitingState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	public override void OnEnter()
    {
       game.arbiter.PauseIngameTime();
    }
	
	public override void OnLeave()
    {
       game.arbiter.ResumeIngameTime();
    }
	
	public override bool OnStartSignal()
	{
		sm.state_change_me(this,new RunningState(sm,cam,this.game.Ball.Owner,game));
		return true;
	}
}