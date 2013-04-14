/**
  * @class IntroCameraState
  * @brief Etat de la caméra au départ
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class IntroCameraState : CameraState
{
    public IntroCameraState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
}