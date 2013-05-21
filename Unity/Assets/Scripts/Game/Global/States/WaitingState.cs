/**
  * @class Waiting state
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class WaitingState : GameState
{
    public WaitingState(StateMachine sm, CameraManager cam, Game game, float time) : base(sm, cam, game) {
        remainingTime = time;
    }

    private float remainingTime;
	
	public override void OnEnter()
    {       
       game.disableIA = true;
       game.arbiter.PauseIngameTime();
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
       game.arbiter.ResumeIngameTime();
       game.disableIA = false;
    }
}