/**
  * @class Waiting state
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see CameraState
  */
public class WaitingState : CameraState
{
    public WaitingState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
}