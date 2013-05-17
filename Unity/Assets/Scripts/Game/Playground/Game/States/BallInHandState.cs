/**
  * @class TackleState
  * @brief Etat de la caméra durant un plaquage
  * @author Sylvain Lafon
  * @see GameState
  */
public class BallInHandState : GameState
{
	Unit current;
	
    public BallInHandState(StateMachine sm, CameraManager cam, Unit _current, Game game) : base(sm, cam, game) 
	{
		this.current = _current;
	}

	public override void OnEnter ()
	{
		sm.state_change_son(this, new FollowPlayerState(sm, cam, current, game));	
	}
}