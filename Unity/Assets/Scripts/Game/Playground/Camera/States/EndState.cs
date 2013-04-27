/**
  * @class EndState
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see CameraState
  */
public class EndState : CameraState
{
    public EndState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
}