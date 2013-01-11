using UnityEngine;
using System.Collections;

public class GiveBallState : UnitState
{

    public GiveBallState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override void OnEnter()
    {
        // Give ball

        // Next behavior
        sm.state_change_me(this, new IdleState(sm, unit));
    }

}
