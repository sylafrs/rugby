using UnityEngine;
using System.Collections.Generic;

/**
  * @class GameActionState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class GameActionState : GameState {

    public GameActionState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

    public override void OnEnter()
    {
		base.OnEnter();	
        game.northTeam.Super.endSuper();
        game.southTeam.Super.endSuper();
        game.Referee.PauseIngameTime();
    }

    public override bool OnTouch(Touche t)
    {
        if (!sm.state_is_last(this))
        {
            return false;
        }
        else
        {           
            sm.state_change_son(this, new TouchState(sm, cam, game, t));
            return true;
        }        
    }

    public override bool OnScrum()
    {
        if (!sm.state_is_last(this))
        {
            return false;
        }
        else
        {
            sm.state_change_son(this, new ScrumState(sm, cam, game));
            return true;
        }
    }

    public override bool OnTry(Zone z)
    {
        if (!sm.state_is_last(this))
        {
            return false;
        }
        else
        {
            sm.state_change_son(this, new ConvertingState(sm, cam, game, z));
            return true;
        }
    }
        
    public override bool OnResumeSignal(float time) 
    {
        sm.state_change_me(this, new WaitingState(sm, cam, game, time));
       
        return true;
    }
}
