using UnityEngine;
using System.Collections;

public class FollowState : UnitState {

    public FollowState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override void OnUpdate()
    {
        unit.GetNMA().SetDestination(unit.GetOrder().target.transform.position);
    }

}
