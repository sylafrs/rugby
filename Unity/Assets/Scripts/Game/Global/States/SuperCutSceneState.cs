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
        var SoundSettings = game.settings.Global.Super.sounds;

       	base.OnEnter();	
        foreach (Unit u in team){
            u.unitAnimator.LaunchSuper();
        }

		TeamNationality ballOwnerNat = game.Ball.Owner.Team.nationality;
		if(game.Ball.Owner.Team == team){
			if(ballOwnerNat == TeamNationality.JAPANESE){
				cam.SuperJapaneseCutSceneComponent.StartCutScene(this.cutsceneDuration);
			}
			if(ballOwnerNat == TeamNationality.MAORI){
				cam.SuperMaoriCutSceneComponent.StartCutScene(this.cutsceneDuration);
			}
		}

        if (ballOwnerNat == TeamNationality.MAORI)
        {
            Timer.AddTimer(SoundSettings.RockFxDelay, () => 
            {
                AudioSource src = this.game.Ball.Owner.audio;
                src.clip = this.game.refs.sounds.SuperScreamNorth;
                src.Play();
                src = game.refs.CameraAudio["Super"];
                src.clip = this.game.refs.sounds.SuperNorth;
                src.Play();
            });
        }
        if (ballOwnerNat == TeamNationality.JAPANESE)
        {
            Timer.AddTimer(SoundSettings.RockFxDelay, () =>
            {
                AudioSource src = this.game.Ball.Owner.audio;
                src.clip = this.game.refs.sounds.SuperScreamSouth;
                src.Play();
                src = game.refs.CameraAudio["Super"];
                src.clip = this.game.refs.sounds.SuperSouth;
                src.Play();
            });           
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