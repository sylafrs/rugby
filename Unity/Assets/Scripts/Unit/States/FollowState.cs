using UnityEngine;
using System.Collections;

public class FollowState : UnitState {

    public FollowState(StateMachine sm, Unit unit) : base(sm, unit) { }

    Unit target;

    public override void OnEnter()
    {
        target = unit.GetOrder().target;
    }

    public override void OnUpdate()
    {
        unit.GetNMA().stoppingDistance = 2;
        unit.GetNMA().SetDestination(target.transform.position);
    }
}
