/**
  * @class EndState
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class EndState : GameState
{
    public EndState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
}