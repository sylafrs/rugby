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
	float 	cutsceneDuration;
	
	public SuperCutSceneState(StateMachine sm, CameraManager cam, Game game, Team TeamOnSuper, float _cutsceneDuration): base(sm, cam, game){
		this.team = TeamOnSuper;
		this.cutsceneDuration = _cutsceneDuration;
	}

	public override void OnEnter ()
	{
       	base.OnEnter();	
        foreach (Unit u in team){
            u.unitAnimator.LaunchSuper();
        }
		cam.SuperCutSceneComponent.StartCutScene(this.cutsceneDuration);
	}
	
	public override void OnLeave ()
	{
		cam.CameraZoomComponent.ZoomToOrigin(0.3f,0.3f);
        team.Super.LaunchSuperEffects();
        team.PlaySuperGroundEffect();
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
	}
}