/**
  * @class PlayingState
  * @brief Etat de la caméra durant le jeu
  * @author Sylvain Lafon
  * @see GameState
  */
using UnityEngine;


public class PlayingState : GameState
{
    public PlayingState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
	//private Game.State firstState;

    public override void OnEnter()
    {
       //decide(cam.game.state);
		Debug.Log("Playin");
		sm.state_change_son(this,new WaitingState(sm,cam,game));
    }

    public override void OnLeave()
    {
        // cam.setTarget(null);
    }
	
	/*
	public override bool OnGameStateChanged(Game.State old, Game.State current)
    {
        if (old == current)
        {
            throw new UnityException("OnGameStateChanged called without state changement..\nHow strange !");
        }

        return this.decide(current);
    }
    */
	
	//Touch
	//scrum
	//transfo
	//running
	//waiting 
	
	/*
    private bool decide(Game.State current)
    {
		
        Unit ballOwner = cam.game.Ball.Owner;
		
		//playin means followin
       	if (current == Game.State.PLAYING)
        {
            sm.state_change_son(this, new NewOwnerState(sm, cam, ballOwner));
			return true;
        }
		
		if (current == Game.State.SCRUM)
        {
            sm.state_change_son(this, new ScrumState(sm, cam, game));
            return true;
        }
		
        if (current == Game.State.TOUCH)
        {
            sm.state_change_son(this, new TouchState(sm, cam, game));
            return true;
        }
       
        if (current == Game.State.TRANSFORMATION)
        {
            sm.state_change_son(this, new TransfoState(sm, cam, game));
            return true;
        }

        return false;
       
    }*/
   
    public override bool OnSuper(Team t, SuperList super)
    {
        // Fin du super
        if (super == SuperList.superNull)
        {
            //cam.zoom = 1.50f;
        }
        else
        {
            // ....
        }

        return false;
    }
}
