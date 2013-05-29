using UnityEngine;

/**
  * @class Waiting state
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class SuperCutSceneState : GameState
{	
	//private Team 		teamOnSuper;
	private float		angle;
	
	public SuperCutSceneState(StateMachine sm, CameraManager cam, Game game, Team TeamOnSuper)
		: base(sm, cam, game)
	{
		//this.teamOnSuper = TeamOnSuper;
	}
	
	public override void OnEnter ()
	{
		this.cam.zoom	= 0.5f;
		this.angle  = 12500;
		cam.ChangeCameraState(CameraManager.CameraState.ONLYZOOM);
	}
	
	public override void OnUpdate()
	{
		
		//smooth damp angle again
		//Mathf.SmoothDampAngle(
		
		//rotate Around
		Camera.mainCamera.transform.RotateAround(game.Ball.Owner.transform.position,new Vector3(0,1,0), angle * Mathf.Deg2Rad * Time.deltaTime);
		
		//lookat
		Camera.mainCamera.transform.LookAt(game.Ball.Owner.transform.position);
	}
	
	public override void OnLeave ()
	{
		//se remettre derrière
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
	}
}