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
        : base(sm, cam, game) { }

    public override bool OnTouch(Touche t){
		game.Referee.StopPlayerMovement();	
        cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,2.5f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
				cam.game.Referee.EnablePlayerMovement();
				this.game.OnResumeSignal(game.Referee.FreezeAfterConversion);
            }, (/* OnFade */) => {
				//cam.ChangeCameraState(CameraManager.CameraState.FREE);
				cam.zoom = 1; //TODO cam settings
				game.refs.managers.conversion.OnLimit();
                cam.game.Referee.StartPlacement();
            }
        );
        return true;
    }

    public override void OnEnter ()
    {
        cam.gameCamera.animation.Play("SpeedFxOnCam", PlayMode.StopAll);
		base.OnEnter();
		game.Ball.ActivateFireTrail();
		cam.LoadParameters(game.settings.GameStates.MainState.PlayingState.GameActionState.ConvertingState.ConversionFly.ConversionFlyCam);
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
    }

    public override void OnLeave (){
		
		Debug.Log("Leave conversion fly");
        
//		cam.ChangeCameraState(CameraManager.CameraState.FREE);
//	    cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,2.5f, 
//		(/* OnFinish */) => {
//               //please, kill after usage x)
//               CameraFade.wannaDie();
//           }, (/* OnFade */) => {
//				cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
//				cam.zoom = 1; //TODO cam settings
//                cam.game.Referee.StartPlacement();
//           }
//       );
    }
	
	public override bool OnBallOnGround (bool onGround)
	{
		game.Referee.StopPlayerMovement();	
        cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,2.5f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
				cam.game.Referee.EnablePlayerMovement();
				
            }, (/* OnFade */) => {
				//cam.ChangeCameraState(CameraManager.CameraState.FREE);
				this.game.OnResumeSignal(game.Referee.FreezeAfterConversion);
				cam.zoom = 1; //TODO cam settings
				game.refs.managers.conversion.OnLimit();
                cam.game.Referee.StartPlacement();
            }
        );
		return true;
	}
	
	public override bool OnBallOut(){
		game.Referee.StopPlayerMovement();
		Debug.Log("out");
        cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,1.5f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
				cam.game.Referee.EnablePlayerMovement();
				this.game.OnResumeSignal(game.Referee.FreezeAfterConversion);
            }, (/* OnFade */) => {
				//cam.ChangeCameraState(CameraManager.CameraState.FREE);
				cam.zoom = 1; //TODO cam settings
				game.refs.managers.conversion.OnLimit();
                cam.game.Referee.StartPlacement();
            }
        );
        return true;
    }
}