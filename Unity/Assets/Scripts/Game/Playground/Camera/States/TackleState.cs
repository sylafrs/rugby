/**
  * @class TackleState
  * @brief Etat de la caméra durant un plaquage
  * @author Sylvain Lafon
  * @see CameraState
  */
public class TackleState : CameraState
{
    public TackleState(StateMachine sm, CameraManager cam, Unit unit) : base(sm, cam) { this.u = unit; }

    Unit u;
	
	public override void OnEnter ()
	{
	}
}