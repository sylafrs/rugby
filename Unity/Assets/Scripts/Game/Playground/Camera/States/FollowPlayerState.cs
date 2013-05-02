/**
  * @class FollowPlayerState
  * @brief Etat de la caméra lorsqu'elle doit suivre un joueur.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class FollowPlayerState : CameraState {
    public FollowPlayerState(StateMachine sm, CameraManager cam, Unit target) : base(sm, cam) { this.target = target; }

    Unit target;

    public override void OnEnter()
    {
        cam.setTarget(this.target.transform);
    }

    public override bool OnPass(Unit from, Unit to)
    {
        sm.state_change_me(this, new PassCameraState(sm, cam));
        return true;
    }

    public override bool OnDrop()
    {
        sm.state_change_me(this, new DropState(sm, cam));
        return true;
    }

    public override bool OnDodge(Unit u)
    {
        if (u == this.target)
        {
            sm.state_change_son(this, new DodgeState(sm, cam, u));
            return true;
        }

        return false;
    }

    public override bool OnSprint(Unit u, bool sprinting)
    {
        if (u == this.target)
        {
            if (sprinting)
            {
                sm.state_change_son(this, new SprintState(sm, cam, u));
                return true;
            }
            // else : 
            //  if a sprintState exists, it must kill himself.
        }

        return false;
    }

    //  TODO à changer de place
    public override bool OnTackle()   
    {
        if(this.target.isTackled) 
        {
            sm.state_change_son(this, new TackleState(sm, cam, this.target));
            return true;
        }

        return false;
    }
	
	
}
