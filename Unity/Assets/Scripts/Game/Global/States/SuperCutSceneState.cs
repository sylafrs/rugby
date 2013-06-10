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
	float	angle,
			lastAngle,
	 		time,
	 		period,
	 		velocity;
	bool	rotating;
	Vector3 axis;
	
	public SuperCutSceneState(StateMachine sm, CameraManager cam, Game game, Team TeamOnSuper)
		: base(sm, cam, game)
	{
		this.team = TeamOnSuper;
	}

	public override void OnEnter ()
	{
       	base.OnEnter();	
        foreach (Unit u in team){
            u.unitAnimator.LaunchSuper();
        }
		
		//if( (game.Ball.Owner != null) && (team == game.Ball.Owner.Team) ){
			SuperCutsceneStateSettings settings = cam.game.settings.GameStates.MainState.PlayingState.WaintingState.superCutsceneState;
			this.period     = settings.duration;
			this.angle  	= settings.finalAngle;
			this.axis		= settings.rotationAxis;
			cam.ChangeCameraState(CameraManager.CameraState.FREE);
		
					
			cam.CameraRotatingAroundComponent.StartTimedRotation(game.Ball.Owner.transform, this.axis, game.Ball.Owner.transform, 
				Camera.mainCamera.transform,
				this.angle,
				this.period);
		//}
	}
	
	public override void OnUpdate()
	{
		/*
		if(this.rotating)
		{
			time += Time.deltaTime;
		
			//smooth damp angle again
			//float angleFromZero = Mathf.SmoothDampAngle(this.lastAngle, this.angle, ref this.velocity,0.3f);
			
			float angleFromZero = Mathf.LerpAngle(0, this.angle, this.time/this.period);
						
			//rotate Around
			Camera.mainCamera.transform.RotateAround(game.Ball.Owner.transform.position,
				new Vector3(0,1,0), 
				Mathf.Rad2Deg * (angleFromZero - this.lastAngle));
			
			Debug.Log("Has rotated");
			
			// This current state becomes the next previous one
		    this.lastAngle = angleFromZero;
			
			//lookat
		    Camera.mainCamera.transform.LookAt(game.Ball.Owner.transform.position);
		}
		*/
	}
	
	public override void OnLeave ()
	{
        team.Super.LaunchSuperEffects();
        team.PlaySuperGroundEffect();
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
	}
}