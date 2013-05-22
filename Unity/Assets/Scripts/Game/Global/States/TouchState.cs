/**
  * @class TouchState
  * @brief Etat de la cam�ra durant une touche
  * @author Sylvain Lafon
  * @see GameState
  */
using UnityEngine;

public class TouchState : GameState
{
    public TouchState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
 	public override void OnEnter ()
	{	
		cam.setTarget(null);
		
		Transform cameraPlaceHolder = GameObject.Find("TouchPlacement").transform.FindChild("CameraPlaceHolder");

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
		cam.setTarget(cam.game.Ball.transform);	
	}
}