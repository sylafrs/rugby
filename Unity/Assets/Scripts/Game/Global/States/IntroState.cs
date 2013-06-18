/**
  * @class IntroState
  * @brief Etat de la cam�ra au d�part
  * @author Sylvain Lafon
  * @see GameState
  */
using System;
using System.Collections;
using UnityEngine;


public class IntroState : GameState
{
    public IntroState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	public override void OnEnter(){  
		base.OnEnter();	
		this.cam.CameraRotatingAroundComponent.StartEndlessRotation(
			game.refs.positions.rotationCenter,
			new Vector3(0,1,0),
			game.refs.positions.fieldCenter,
			game.refs.positions.cameraFirstPosition,
			cam.game.settings.GameStates.MainState.IntroState.rotationSpeed/100);	
    }

	public override void OnLeave(){
		cam.CameraRotatingAroundComponent.StopRotation();
		cam.setTarget(cam.game.Ball.Owner.transform);
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
		CameraFade.StartAlphaFade(Color.black, true, 2f, 2f, () =>
		{
			CameraFade.wannaDie();
		});
	}
}