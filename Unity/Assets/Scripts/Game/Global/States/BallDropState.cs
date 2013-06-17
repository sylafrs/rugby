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
		cam.LoadParameters(game.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallFreeState.BallDropCamSettings);
	}
	
	public override bool OnBallOut()
    {
		Debug.Log("Drop fail");
        cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,1.5f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
            }, (/* OnFade */) => {
				//cam.ChangeCameraState(CameraManager.CameraState.FREE);
				cam.zoom = 1; //TODO cam settings
                cam.game.Referee.StartPlacement();
            }
        );
        return true;
    }
}