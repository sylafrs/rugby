using UnityEngine;
using System.Collections;

public class MoveState : UnitState {

    const float epsilon = 0.1f;
       
    public MoveState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override void OnEnter()
    {
        unit.GetNMA().SetDestination(unit.GetOrder().point);
    }

    public override void OnUpdate()
    {
        if (unit.GetNMA().remainingDistance < epsilon)
        {
            unit.ChangeOrder(Order.OrderNothing());
        }
    }
}
