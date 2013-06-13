using UnityEngine;
using System.Collections;

/**
  * @class SuperCutSceneState
  * @brief State du jeu au moment de la cutscene pour les supers
  * @author TEAM
  * @see GameState
  */
public class SuperCutSceneState : GameState
{	
	Team 	team;
	float   timeElapsed;
	
	public SuperCutSceneState(StateMachine sm, CameraManager cam, Game game, Team TeamOnSuper): base(sm, cam, game){
		this.team = TeamOnSuper;
	}

	public override void OnEnter ()
	{
       	base.OnEnter();	
        foreach (Unit u in team){
            u.unitAnimator.LaunchSuper();
        }
		SuperCutsceneStateSettings settings = cam.game.settings.GameStates.MainState.PlayingState.WaintingState.superCutsceneState;
		cam.CameraRotatingAroundComponent.StartTimedRotation(
			game.Ball.Owner.transform, 
			settings.rotationAxis, 
			game.Ball.Owner.transform, 
			Camera.mainCamera.transform,
			settings.finalAngle,
			settings.duration,
			settings.smooth);
		cam.CameraZoomComponent.StartZoomIn(20,0.3f,0.3f);
		this.timeElapsed = 0;
	}
	
	public override void OnUpdate(){
		this.timeElapsed += Time.deltaTime;
		
	}
	
	public override void OnLeave ()
	{
		cam.CameraZoomComponent.ZoomToOrigin(0.3f,0.3f);
        team.Super.LaunchSuperEffects();
        team.PlaySuperGroundEffect();
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
	}
}