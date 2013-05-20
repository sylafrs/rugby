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
	public RunningState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

    public override void OnEnter()
    {
        decide(this.game.Ball.Owner);
    }   

    public override bool OnDrop()
    {
        decide(null);
        return true;
    }

    public override bool OnNewOwner(Unit old, Unit current)
    {
        decide(current);
        return true;
    }
	
    private void decide(Unit owner) 
    {
        if (owner)
        {
            sm.state_change_son(this, new BallHandlingState(sm, cam, game));
        }
        else
        {
            sm.state_change_son(this, new BallFreeState(sm, cam, game));
        }

    }

	//public override void OnEnter ()
	//{
	//	OnNewOwner(null, firstOwner);
	//}
	//
	//public override void OnUpdate(){
	//	game.guiManager.hideGameUi = false;
	//}
	//
	//public override bool OnNewOwner(Unit old, Unit current)
    //{
    //    if (current != null)
    //    {
	//		//if ((old != null)&&(old.Team != current.Team))
    //        //{
	//			//sm.state_change_son(this, new FollowPlayerState(sm, cam, current, game));
	//			sm.state_change_son(this, new BallInHandState(sm, cam, current, game));
	//			//cam.setTarget();
	//			//cam.OnNextIdealPosition = () => {
	//			//Timer.AddTimer(1, () => {			
	//	             cam.flipForTeam(current.Team, () =>{
	//				 	MyDebug.Log("Hop, fin du flip"); 	
	//				 });
	//			//});
	//				
	//			//};
	//			//cam.setTarget(current.transform);
    //            
    //        //}			
    //       			
    //        return true;          
	//	}
    //    return OnBallOnGround(true);		        
    //}
    //
	//public override bool OnBallOnGround(bool onGround)
    //{
    //    if (onGround)
    //    {
    //        sm.state_change_son(this, new GroundBallState(sm, cam, game));
    //        return true;
    //    }
    //
    //    return false;
    //}
	//
	//public override bool OnPass(Unit from, Unit to)
    //{
    //    sm.state_change_son(this, new PassGameState(sm, cam, game));
    //    return true;
    //}
	//
	//public override bool OnDrop()
    //{
	//	Debug.Log("Drop Cam");
    //    sm.state_change_me(this, new DropState(sm, cam, game));
    //    return true;
    //}
    //
    ///*
    //public override bool OnGameStateChanged(Game.State old, Game.State current)
    //{
    //    if (old == current)
    //    {
    //        throw new UnityException("OnGameStateChanged called without state changement..\nHow strange !");
    //    }
    //
    //    return this.decide(current);
    //}
    //*/
    //
    ////Touch
    ////scrum
    ////transfo
    ////running
    ////waiting 
    //
    ///*
    //private bool decide(Game.State current)
    //{
	//	
    //    Unit ballOwner = cam.game.Ball.Owner;
	//	
    //    //playin means followin
    //    if (current == Game.State.PLAYING)
    //    {
    //        sm.state_change_son(this, new NewOwnerState(sm, cam, ballOwner));
    //        return true;
    //    }
	//	
    //    if (current == Game.State.SCRUM)
    //    {
    //        sm.state_change_son(this, new ScrumState(sm, cam, game));
    //        return true;
    //    }
	//	
    //    if (current == Game.State.TOUCH)
    //    {
    //        sm.state_change_son(this, new TouchState(sm, cam, game));
    //        return true;
    //    }
    //   
    //    if (current == Game.State.TRANSFORMATION)
    //    {
    //        sm.state_change_son(this, new TransfoState(sm, cam, game));
    //        return true;
    //    }
    //
    //    return false;
    //   
    //}*/
    //
    //public override bool OnTouch()
    //{
    //    sm.state_change_son(this, new TouchState(sm, cam, game));
    //    return (true);
    //}
    //
    //public override bool OnSuper(Team t, SuperList super)
    //{
    //    // Fin du super
    //    if (super == SuperList.superNull)
    //    {
    //        //cam.zoom = 1.50f;
    //    }
    //    else
    //    {
    //        // ....
    //    }
    //
    //    return false;
    //}
}