/**
  * @class TouchState
  * @brief Etat de la cam�ra durant une touche
  * @author Sylvain Lafon
  * @see GameState
  */
using UnityEngine;

public class TouchState : GameState
{
    public TouchState(StateMachine sm, CameraManager cam, Game game, Touche t) : base(sm, cam, game) {
        game.Referee.OnTouch(t);
    }
	
 	public override void OnEnter ()
	{
		base.OnEnter();
		cam.setTarget(null);
		game.Ball.ActivateFairyTrail();
		cam.LoadParameters(game.settings.GameStates.MainState.PlayingState.GameActionState.TouchingSgtate.TouchCamSettings);
		cam.ChangeCameraState(CameraManager.CameraState.FREE);
		
		Transform cameraPlaceHolder = game.refs.placeHolders.touchPlacement.FindChild("CameraPlaceHolder");

        game.refs.transitionsTexts.ballOut.SetActive(true);

        game.Ball.collider.enabled = false;
        Timer.AddTimer(2, () => {
            cam.transalateToWithFade(cameraPlaceHolder.position, cameraPlaceHolder.rotation, 0f, 1f, 1f, 0.3f, 
                (/* OnFinish */) => {               
                    CameraFade.wannaDie();
                }, (/* OnFade */) => {

                    game.refs.managers.ui.currentState = UIManager.UIState.TouchUI;

                    game.refs.transitionsTexts.ballOut.SetActive(false);
				    cam.CancelNextFlip = true;
                    game.Referee.PlacePlayersForTouch();
                    game.Ball.collider.enabled = true;
                }
            );

            this.game.northTeam.OnTouch();
            this.game.southTeam.OnTouch();
        });
	}
	
	public override void OnLeave ()
	{
        foreach (Unit u in game.northTeam)
            u.buttonIndicator.target.renderer.enabled = false;

        foreach (Unit u in game.southTeam)
            u.buttonIndicator.target.renderer.enabled = false;

        cam.setTarget(cam.game.Ball.transform);
		cam.ChangeCameraState(CameraManager.CameraState.FREE);

        game.Referee.EnablePlayerMovement();
	}
}