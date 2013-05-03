/**
  * @class NewOwnerState
  * @brief Etat de la caméra durant une touche
  * @author Maxens Dubois
  * @see CameraState
  */
public class NewOwnerState : CameraState
{
	private Unit firstOwner;
	
    public NewOwnerState(StateMachine sm, CameraManager cam, Unit current) : base(sm, cam) { 
		firstOwner = current;
	}
	
	
	public override void OnEnter ()
	{
		OnNewOwner(null, firstOwner);
	}
		
	public override bool OnNewOwner(Unit old, Unit current)
    {
        if (current != null)
        {
			
			//if ((old != null)&&(old.Team != current.Team))
            //{
				cam.setTarget(current.transform);
                cam.flipForTeam(current.Team);
            //}
			
            sm.state_change_son(this, new FollowPlayerState(sm, cam, current));
			
            return true;          
		}else{
			OnBallOnGround(true);
			return true;
		}
        return false;
    }

	public override bool OnBallOnGround(bool onGround)
    {
        if (onGround)
        {
            sm.state_change_son(this, new GroundBallState(sm, cam));
            return true;
        }

        return false;
    }
	
	public override bool OnPass(Unit from, Unit to)
    {
        sm.state_change_son(this, new PassCameraState(sm, cam));
        return true;
    }
}