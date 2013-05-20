using UnityEngine;
/**
  * @class FollowPlayerState
  * @brief Etat de la caméra lorsqu'elle doit suivre un joueur.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
public class FollowPlayerState : GameState {
    public FollowPlayerState(StateMachine sm, CameraManager cam, Unit target, Game game) : base(sm, cam, game) { this.target = target; }

    Unit target;

    public override void OnEnter()
    {
        cam.setTarget(this.target.transform);
    }

    public override bool OnDodge(Unit u)
    {
        if (u == this.target)
        {
            sm.state_change_son(this, new DodgeState(sm, cam, this.game));
            return true;
        }

        return false;
    }

    //  TODO à changer de place ou pas
    public override bool OnTackle()   
    {
		
		
		
        //if(this.target.isTackled) 
        //{
			//MyDebug.Log("On Tackle");
            sm.state_change_son(this, new TackleState(sm, cam, game, this.target));
            return true;
        //}

        //return false;
    }
	
	
}
