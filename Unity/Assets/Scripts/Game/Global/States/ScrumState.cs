/**
  * @class ScrumState
  * @brief Etat de la cam�ra durant une m�l�e
  * @author Sylvain Lafon
  * @see GameState
  */
using UnityEngine;

public class ScrumState : GameState
{
    public ScrumState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

    Vector3 offset;
	
	public override void OnEnter()
    {
        ScrumingStateSettings settings = game.settings.GameStates.MainState.PlayingState.GameActionState.ScrumingState;

        game.Referee.ScrumCinematicMovement();

        cam.transalateWithFade(Vector3.zero, Quaternion.identity, 2, 1, 1, 1, 
        (/* OnFinish*/) => {
            game.refs.managers.ui.currentState = UIManager.UIState.ScrumUI;
            game.Referee.NowScrum();
        }, (/*OnFade*/) => {

            offset = cam.MinfollowOffset;
		    cam.setTarget(this.game.refs.gameObjects.ScrumBloc.transform);
            cam.MinfollowOffset = settings.offsetCamera;

            game.Referee.ScrumSwitchToBloc();
        });
        
    }
	
	public override void OnLeave()
	{
        cam.MinfollowOffset = offset;
        
        cam.transalateWithFade(Vector3.zero, Quaternion.identity, 1, 1, 1, 1, 
            (/* OnFinish*/) => {
                CameraFade.wannaDie();
            },
            (/*OnFade*/) => {
                cam.setTarget(game.refs.managers.scrum.GetWinner().transform);
                game.refs.managers.ui.currentState = UIManager.UIState.NULL;
                game.Referee.ScrumAfter();
        });


        
	}   
}