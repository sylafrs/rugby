using UnityEngine;
/**
  * @class DropState
  * @brief Etat de la caméra durant un drop
  * @author Sylvain Lafon
  * @see GameState
  */
public class DropState : GameState
{
    public DropState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	public override void OnEnter ()
	{
		
	}
}