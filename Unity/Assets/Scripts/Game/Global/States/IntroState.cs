/**
  * @class IntroState
  * @brief Etat de la caméra au départ
  * @author Sylvain Lafon
  * @see GameState
  */
using System;
using System.Collections;
using UnityEngine;


public class IntroState : GameState
{
    public IntroState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	private Transform cameraFirstPosition;
	private Transform fieldCenter;
	private Transform rotationCenter;
	
	private float	 rotationSpeed;
	private float	 rotationAngle;
	
    // Petit tp + fondu
	public override void OnEnter()
    {
        //this.stepBack()
		
		cameraFirstPosition = game.refs.positions.cameraFirstPosition;
		fieldCenter			= game.refs.positions.fieldCenter;
		rotationCenter		= game.refs.positions.rotationCenter;
		
		rotationSpeed = cam.game.settings.GameStates.MainState.IntroState.rotationSpeed;
		
		rotationAngle = 1 * rotationSpeed;
		
		//tp la camera au place holder
		Camera.mainCamera.transform.position = cameraFirstPosition.position;
		
    }
	
    // Petit tp + fondu, se rappelle quand se termine.
	private void stepBack()
	{
		cam.transalateWithFade(new Vector3(0, 0, -10), 4f, 1f, 1f, 1f, () =>
		{
			stepBack();
		});
	}

	// Recule sans arrêt
	public override void OnUpdate()
    {		
		//rotate Around
		Camera.mainCamera.transform.RotateAround(rotationCenter.position,new Vector3(0,1,0),rotationAngle * Mathf.Deg2Rad * Time.deltaTime);
		
		//lookat
		Camera.mainCamera.transform.LookAt(fieldCenter);
	}

	// On va vers la cible, on fait un fondu (en écrasant le précédent).
	public override void OnLeave()
	{
		cam.setTarget(cam.game.Ball.Owner.transform);
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
		CameraFade.StartAlphaFade(Color.black, true, 2f, 2f, () =>
		{
			CameraFade.wannaDie();
		});
	}
}