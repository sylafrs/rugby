using UnityEngine;
using System.Collections;

public class IdleState : UnitState {

    public IdleState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override bool OnNewOrder()
    {
        if (unit.GetOrder().type != Order.TYPE.RIEN)
        {
            sm.state_kill_me(this);
        }

        return true;
    }


}
