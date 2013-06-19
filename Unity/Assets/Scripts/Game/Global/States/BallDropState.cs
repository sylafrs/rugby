using UnityEngine;
using System.Collections.Generic;

/**
  * @class BallDropState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class BallDropState : GameState {
    public BallDropState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	public override void OnEnter ()
	{
		base.OnEnter();
		game.Ball.ActivateFireTrail();
		cam.LoadParameters(game.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallDropCamSettings);
        game.Ball.audio.PlayOneShot(game.refs.sounds.ShootBall);
	}
	
	public override bool OnBallOut(){	
        //game.refs.transitionsTexts.ballOut.SetActive(true);
 //     Timer.AddTimer(1, () =>
 //     {
 //         game.Referee.StopPlayerMovement();
 //         cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,0.7f, 
 //             (/* OnFinish */) => {
 //                 //please, kill after usage x)
 //                 CameraFade.wannaDie();
//				    cam.game.Referee.EnablePlayerMovement();
 //             }, (/* OnFade */) => {
//				    //cam.ChangeCameraState(CameraManager.CameraState.FREE);
//				    cam.zoom = 1; //TODO cam settings
 //                 //game.refs.transitionsTexts.ballOut.SetActive(false);
 //                 cam.game.Referee.StartPlacement();
 //             }
 //         );
 //     });
		
        return true;
    }
	
	public override void OnLeave ()
	{
//		Debug.Log("leave de drop");
	}
}