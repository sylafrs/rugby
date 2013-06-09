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
	
	Transform 	cameraFirstPosition,
	 			fieldCenter,
	 			rotationCenter;
	float		rotationSpeed,
			 	rotationAngle;
	Vector3		rotationAxis;
	
	public override void OnEnter()
    {  
		base.OnEnter();
		cameraFirstPosition = game.refs.positions.cameraFirstPosition;
		fieldCenter			= game.refs.positions.fieldCenter;
		rotationCenter		= game.refs.positions.rotationCenter;
		rotationSpeed 		= cam.game.settings.GameStates.MainState.IntroState.rotationSpeed;
		rotationAxis 		= cam.game.settings.GameStates.MainState.IntroState.rotationAxis;
		rotationAngle 		= 1 * rotationSpeed;

		this.cam.CameraRotatingAroundComponent.StartEndlessRotation(rotationCenter,rotationAxis,fieldCenter, cameraFirstPosition, rotationAngle);
    }

	public override void OnLeave()
	{
		cam.CameraRotatingAroundComponent.StopRotation();
		cam.setTarget(cam.game.Ball.Owner.transform);
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
		CameraFade.StartAlphaFade(Color.black, true, 2f, 2f, () =>
		{
			CameraFade.wannaDie();
		});
	}
}