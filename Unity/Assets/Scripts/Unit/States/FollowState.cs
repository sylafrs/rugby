using UnityEngine;
using System.Collections;

/**
 * @class FollowState
 * @brief Etat suivre : fait suivre une unité par une autre
 * @author Sylvain Lafon
 */
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
