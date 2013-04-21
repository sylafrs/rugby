/**
  * @class SprintState
  * @brief Etat de la caméra durant un sprint
  * @author Sylvain Lafon
  * @see CameraState
  */
public class SprintState : CameraState
{
    public SprintState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
}