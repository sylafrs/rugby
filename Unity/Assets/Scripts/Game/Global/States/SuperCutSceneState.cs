using UnityEngine;

/**
  * @class SuperCutSceneState
  * @brief State du jeu au moment de la cutscene pour les supers
  * @author TEAM
  * @see GameState
  */
public class SuperCutSceneState : GameState
{	
	Team 	team;
	
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
	}
	
	public override void OnLeave ()
	{
        team.Super.LaunchSuperEffects();
        team.PlaySuperGroundEffect();
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
	}
}