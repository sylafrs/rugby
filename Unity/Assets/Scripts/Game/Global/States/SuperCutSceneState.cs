/**
  * @class Waiting state
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class SuperCutSceneState : GameState
{	
	private Team teamOnSuper;
	
	public SuperCutSceneState(StateMachine sm, CameraManager cam, Game game, Team TeamOnSuper)
		: base(sm, cam, game)
	{
		this.teamOnSuper = TeamOnSuper;
	}
	
	public override void OnEnter ()
	{
		//lancer un flip 360 ?
		cam.turnAround();
	}
	
	public override void OnLeave ()
	{
		//se remettre derrière
	}
}