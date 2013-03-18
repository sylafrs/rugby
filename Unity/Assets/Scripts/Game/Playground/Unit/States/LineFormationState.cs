using UnityEngine;

/**
 * @class LineFormationState
 * @brief Etat se met en ligne
 * @author Sylvain Lafon
 */
class LineFormationState : UnitState
{
    public LineFormationState(StateMachine sm, Unit unit) : base(sm, unit) { }

    Order o;

    public override void OnEnter()
    {
        o = unit.Order;
    }

    public override void OnUpdate()
    {        
        Vector3 tPos = o.target.transform.position;

        int dif = unit.Team.GetLineNumber(unit, o.target);
        float x = o.power * dif;
        
        unit.GetNMA().stoppingDistance = 0;
        unit.GetNMA().SetDestination(new Vector3(tPos.x + x, 0, tPos.z));
    }

    public override bool OnNearUnit(Unit u)
    {
        if (u == unit.Game.Ball.Owner)
        {
            unit.Order = Order.OrderPlaquer(u);
            return true;
        }
        return false;
    }
}

