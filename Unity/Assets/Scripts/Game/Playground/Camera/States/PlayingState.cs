/**
  * @class PlayingState
  * @brief Etat de la caméra durant le jeu
  * @author Sylvain Lafon
  * @see CameraState
  */
using UnityEngine;


public class PlayingState : CameraState
{
    public PlayingState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
	
	private Game.State firstState;

    public override void OnEnter()
    {
       decide(cam.game.state);
    }

    public override void OnLeave()
    {
        // cam.setTarget(null);
    }
	
	public override bool OnGameStateChanged(Game.State old, Game.State current)
    {
        if (old == current)
        {
            throw new UnityException("OnGameStateChanged called without state changement..\nHow strange !");
        }

        return this.decide(current);
    }
	
	public override bool OnNewOwner(Unit old, Unit current)
    {
        if (current != null)
        {
            if ((old != null)&&(old.Team != current.Team))
            {
                cam.flipForTeam(current.Team);
            }
		}
        return false;
    }
	
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
            sm.state_change_son(this, new ScrumState(sm, cam));
            return true;
        }
		
        if (current == Game.State.TOUCH)
        {
            sm.state_change_son(this, new TouchState(sm, cam));
            return true;
        }
       
        if (current == Game.State.TRANSFORMATION)
        {
            sm.state_change_son(this, new TransfoState(sm, cam));
            return true;
        }

        return false;
    }

    

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
