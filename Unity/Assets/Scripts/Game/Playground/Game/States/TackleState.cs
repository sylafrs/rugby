/**
  * @class TackleState
  * @brief Etat de la caméra durant un plaquage
  * @author Sylvain Lafon
  * @see GameState
  */
public class TackleState : GameState
{
    public TackleState(StateMachine sm, CameraManager cam, Game game, Unit unit) : base(sm, cam, game) { /*this.u = unit;*/ }

    /*
    Unit u;
	
	public override void OnEnter ()
	{
	}
     */
}