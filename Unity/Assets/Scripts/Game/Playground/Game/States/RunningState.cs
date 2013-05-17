using UnityEngine;
using System.Threading;

/**
  * @class RunningState
  * @brief Etat de la caméra durant une touche
  * @author Maxens Dubois
  * @see GameState
  */
public class RunningState : GameState
{
	private Unit firstOwner;
	
    public RunningState(StateMachine sm, CameraManager cam, Unit current, Game game) : base(sm, cam, game) { 
		firstOwner = current;
	}
	
	public override void OnEnter ()
	{
		OnNewOwner(null, firstOwner);
	}
	
	public override void OnUpdate(){
		game.guiManager.hideGameUi = false;
	}
	
	public override bool OnNewOwner(Unit old, Unit current)
    {
        if (current != null)
        {
			//if ((old != null)&&(old.Team != current.Team))
            //{
				//sm.state_change_son(this, new FollowPlayerState(sm, cam, current, game));
				sm.state_change_son(this, new BallInHandState(sm, cam, current, game));
				//cam.setTarget();
				//cam.OnNextIdealPosition = () => {
				//Timer.AddTimer(1, () => {			
		             cam.flipForTeam(current.Team, () =>{
					 	MyDebug.Log("Hop, fin du flip"); 	
					 });
				//});
					
				//};
				//cam.setTarget(current.transform);
                
            //}			
           			
            return true;          
		}
        return OnBallOnGround(true);		        
    }

	public override bool OnBallOnGround(bool onGround)
    {
        if (onGround)
        {
            sm.state_change_son(this, new GroundBallState(sm, cam, game));
            return true;
        }

        return false;
    }
	
	public override bool OnPass(Unit from, Unit to)
    {
        sm.state_change_son(this, new PassGameState(sm, cam, game));
        return true;
    }
	
	public override bool OnDrop()
    {
		Debug.Log("Drop Cam");
        sm.state_change_me(this, new DropState(sm, cam, game));
        return true;
    }
}