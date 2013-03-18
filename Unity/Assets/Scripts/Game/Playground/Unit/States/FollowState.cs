using UnityEngine;

/**
 * @class FollowState
 * @brief Etat suivre : fait suivre une unité par une autre
 * @author Sylvain Lafon
 */
public class FollowState : UnitState {

    public FollowState(StateMachine sm, Unit unit) : base(sm, unit) { }

    GameObject target;

    public override void OnEnter()
    {
        Order o = unit.Order;
        if (o.type == Order.TYPE.FOLLOW)
        {
            target = unit.Order.target.gameObject;
        }
        if (o.type == Order.TYPE.SEARCH)
        {
            target = unit.Game.Ball.gameObject;
        }
    }

    public override void OnUpdate()
    {
        Vector3 pos;
        if (target == null)
        {
            pos = unit.Game.Ball.transform.position;
        }
        else
        {
            pos = target.transform.position;
        }

        unit.GetNMA().stoppingDistance = 2;
        unit.GetNMA().SetDestination(pos);
    }
}
