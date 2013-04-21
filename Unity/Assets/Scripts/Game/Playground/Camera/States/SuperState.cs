/**
  * @class SuperState
  * @brief Etat de la caméra pendant un super.
  * @author Sylvain Lafon
  * @see CameraState
  */
public class SuperState : CameraState
{
    public SuperState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
}