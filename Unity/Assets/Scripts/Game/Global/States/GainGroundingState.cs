using UnityEngine;

/**
  * @class FollowPlayerState
  * @brief Etat de la cam�ra lorsqu'elle doit suivre un joueur.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
public class GainGroundingState : GameState {
    public GainGroundingState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

    public override void OnEnter()
    {  
		base.OnEnter();
		game.Ball.StopTrail();
		cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
		cam.LoadParameters(game.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallHandlingState.GainingGrounCamSettings);
		cam.flipForTeam(this.game.Ball.Owner.Team, () => {
			cam.ChangeCameraState(CameraManager.CameraState.FOLLOWING);
		});
    }
        
    public override bool OnDodge(Unit u)
    {
        if (u == this.game.Ball.Owner)
        {
            sm.state_change_me(this, new DodgingState(sm, cam, this.game));
            return true;
        }

        return false;
    }	
}
