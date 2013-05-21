/**
  * @class EndState
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class EndState : GameState
{
    public EndState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	public override void OnEnter()
    {
		game.guiManager.currentState = UIManager.UIState.EndUI;
    }
	
	public override void OnLeave()
	{
		game.guiManager.currentState = UIManager.UIState.NULL;
	}
}