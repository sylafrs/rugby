using UnityEngine;

/**
 * @class FollowState
 * @brief Etat suivre : fait suivre une unité par une autre
 * @author Sylvain Lafon
 */
public class FollowState : UnitState {

    public FollowState(StateMachine sm, Unit unit) : base(sm, unit) { }

    GameObject target;

    public override bool OnNearBall()
    {
        if (unit.Game.Ball.Owner == null)
        {
            unit.Game.Ball.Taken(unit);
            return true;
        }

        return false;
    }

    public override void OnEnter()
    {
        Order o = unit.Order;
        if (o.type == Order.TYPE.SUIVRE)
        {
            target = unit.Order.target.gameObject;
        }
        if (o.type == Order.TYPE.CHERCHER)
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
