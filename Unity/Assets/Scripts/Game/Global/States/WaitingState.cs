using UnityEngine;
using System;

/**
  * @class Waiting state
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class WaitingState : GameState
{
	float remainingTime;
	Team  TeamOnSuper;
	bool  onWiningPoints;
	
	public WaitingState(StateMachine sm, CameraManager cam, Game game, float time)
		: base(sm, cam, game)
	{
		this.remainingTime = time;
		this.TeamOnSuper = null;
		this.onWiningPoints = false;
	}

	public WaitingState(StateMachine sm, CameraManager cam, Game game, float time, Team TeamOnSuper)
		: base(sm, cam, game)
	{
		this.remainingTime = time;
		this.TeamOnSuper = TeamOnSuper;
		this.onWiningPoints = false;
	}
	
	public WaitingState(StateMachine sm, CameraManager cam, Game game, float time, Team team, bool _onWiningPoints)
		: base(sm, cam, game)
	{
		this.remainingTime 	= time;
		this.TeamOnSuper 	= team;
		this.onWiningPoints = true;
	}

	public override void OnEnter()
	{
		base.OnEnter();

		if(this.onWiningPoints)
        {
			sm.state_change_son(this, new WiningPointCutSceneState(sm, cam, game, TeamOnSuper));
		}
        else
        {
			if (game.Ball.Owner)
	        {
	            cam.setTarget(game.Ball.Owner.transform);
	        }
	        else
	        {
	            UnityEngine.Debug.LogWarning("Error : ball owner is null !");
	            if (game.Ball.PreviousOwner)
	                cam.setTarget(game.Ball.PreviousOwner.transform);
	            else
	                cam.setTarget(game.Ball.transform);
	        }

			game.disableIA = true;
			game.Referee.PauseIngameTime();
			if(TeamOnSuper)
				sm.state_change_son(this, new SuperCutSceneState(sm, cam, game, TeamOnSuper,remainingTime));
		}
	}

	public override void OnUpdate()
	{
		remainingTime -= UnityEngine.Time.deltaTime;
		if (remainingTime <= 0)
		{
			sm.state_change_me(this, new MainGameState(sm, cam, game));
		}
	}

	public override void OnLeave()
	{
        game.Ball.OnConversion(false);

		game.Referee.ResumeIngameTime();
		game.disableIA = false;
        Team[] teams = new Team[2];
        teams[0] = game.southTeam;
        teams[1] = game.northTeam;

        foreach (Team t in teams){            
            foreach (Unit u in t){
                if (t.Player.Controlled && game.Ball.NextOwner != u){
					u.typeOfPlayer = Unit.TYPEOFPLAYER.DEFENSE;
                    u.UpdateTypeOfPlay();
                    u.UpdatePlacement();                        
                }
                u.buttonIndicator.target.renderer.enabled = false;
            }
            t.fixUnits = false;
        }
			
		Timer.AddTimer(3f, () => {
			cam.game.Referee.EnablePlayerMovement();
        });
	}
}