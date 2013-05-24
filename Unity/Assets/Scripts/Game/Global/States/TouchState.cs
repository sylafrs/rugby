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
		cam.setTarget(null);
		
		Transform cameraPlaceHolder = game.refs.placeHolders.touchPlacement.FindChild("CameraPlaceHolder");

        cam.transalateToWithFade(cameraPlaceHolder.position, cameraPlaceHolder.rotation, 0f, 1f, 1f, 0.3f, 
            (/* OnFinish */) => {               
                CameraFade.wannaDie();
            }, (/* OnFade */) => { 
				cam.CancelNextFlip = true;
                cam.game.Referee.PlacePlayersForTouch();
            }
        );
		
	}
	
	public override void OnLeave ()
	{
        foreach (Unit u in game.northTeam)
            u.buttonIndicator.target.renderer.enabled = false;

        foreach (Unit u in game.southTeam)
            u.buttonIndicator.target.renderer.enabled = false;

        cam.setTarget(cam.game.Ball.transform);	
	}
}