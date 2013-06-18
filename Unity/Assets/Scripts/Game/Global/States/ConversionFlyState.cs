using UnityEngine;
using System.Collections.Generic;

/**
  * @class ConversionFlyState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class ConversionFlyState : GameState
{
    public ConversionFlyState(StateMachine sm, CameraManager cam, Game game, Zone z)
        : base(sm, cam, game)
    {
        //this.zone = z;
    }

    //private Zone zone;

    public override bool OnTouch(Touche t)
    {
        game.refs.managers.conversion.OnLimit();
        return true; // Could call signal
    }
	
	/*
    public override bool OnBallOut()
    {
        game.refs.managers.conversion.OnLimit();
        return true; // Could call signal
    }
    */

    public override void OnEnter ()
    {
		base.OnEnter();
		cam.LoadParameters(game.settings.GameStates.MainState.PlayingState.GameActionState.ConvertingState.ConversionFly.ConversionFlyCam);
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
    }

    public override void OnLeave ()
    {   			
		
//		cam.ChangeCameraState(CameraManager.CameraState.FREE);
//	    cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,1.5f, 
//           (/* OnFinish */) => {
//               //please, kill after usage x)
//               CameraFade.wannaDie();
//           }, (/* OnFade */) => {
//				cam.ChangeCameraState(CameraManager.CameraState.FREE);
//				cam.zoom = 1; //TODO cam settings
//               cam.game.Referee.StartPlacement();
//           }
//       );
		
    }
	
	public override bool OnBallOut(){
		game.Referee.StopPlayerMovement();	
        cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,1.5f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
				cam.game.Referee.EnablePlayerMovement();
            }, (/* OnFade */) => {
				//cam.ChangeCameraState(CameraManager.CameraState.FREE);
				cam.zoom = 1; //TODO cam settings
                cam.game.Referee.StartPlacement();
            }
        );
        return true;
    }
}