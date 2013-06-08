/**
  * @class EndState
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class EndState : GameState
{
    public EndState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
    // On entre dans l'état de fin
	public override void OnEnter()
    {
		base.OnEnter();
		game.refs.managers.ui.currentState = UIManager.UIState.EndUI;
    }
	
    // Ceci est un non-sens.
	public override void OnLeave()
	{
        game.refs.managers.ui.currentState = UIManager.UIState.NULL;
	}
}