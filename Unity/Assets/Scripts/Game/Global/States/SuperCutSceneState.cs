using UnityEngine;

/**
  * @class Waiting state
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class SuperCutSceneState : GameState
{	
	private Team 		teamOnSuper;
	private float		angle;
	private float		lastAngle;
	private float 		time;
	private float 		period;
	private float 		velocity;
	private bool		rotating;
	
	public SuperCutSceneState(StateMachine sm, CameraManager cam, Game game, Team TeamOnSuper)
		: base(sm, cam, game)
	{
		this.teamOnSuper = TeamOnSuper;
	}

	public override void OnEnter ()
	{        
        if (teamOnSuper.Player.Controlled)
        {
            if (teamOnSuper.Player.Controlled.unitAnimator)
            {
                teamOnSuper.Player.Controlled.unitAnimator.LaunchSuper();
            }
        }
        else
        {
            if (teamOnSuper.captain.unitAnimator)
            {
                teamOnSuper.captain.unitAnimator.LaunchSuper();
            }
        }

		
		if( (game.Ball.Owner != null) && (teamOnSuper == game.Ball.Owner.Team) ){
			this.cam.zoom	= 0.5f;
			this.angle  	= 360 * Mathf.Deg2Rad;
			this.lastAngle	= 0;
			this.period 	= 3.3f;
			this.rotating	= true;
			cam.ChangeCameraState(CameraManager.CameraState.FREE);
		}else{
			this.rotating	= false;
		}
	}
	//Rotation spéciale capitaine
	//Rotation normale
	
	public override void OnUpdate()
	{
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
			
			// This current state becomes the next previous one
		    this.lastAngle = angleFromZero;
			
			//lookat
		    Camera.mainCamera.transform.LookAt(game.Ball.Owner.transform.position);
		}
	}
	
	public override void OnLeave ()
	{
		//se remettre derrière
        teamOnSuper.Super.LaunchSuperEffects();
        teamOnSuper.PlaySuperGroundEffect();
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
	}
}