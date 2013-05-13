using UnityEngine;
using System.Threading;

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
				sm.state_change_son(this, new FollowPlayerState(sm, cam, current));
				//cam.setTarget();
				//cam.OnNextIdealPosition = () => {
			
			
			
				//Timer.AddTimer(1, () => {			
		              	cam.flipForTeam(current.Team, () =>{
							Debug.Log("Hop, fin du flip"); 	
					 });
				//});
					
				//};
				//cam.setTarget(current.transform);
                
            //}
			
           
			
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