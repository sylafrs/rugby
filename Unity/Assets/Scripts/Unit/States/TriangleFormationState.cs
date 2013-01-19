using UnityEngine;

class TriangleFormationState : UnitState
{
    public TriangleFormationState(StateMachine sm, Unit unit) : base(sm, unit) { }

    Order o;

    public override void OnEnter()
    {
        o = unit.Order;
    }

    public override void OnUpdate()
    {
        Vector3 tPos = o.target.transform.position;

        int dif = unit.Team.GetLineNumber(unit, o.target);
        float x = o.point.x * dif;
        float z = o.point.z * Mathf.Abs(dif);

        unit.GetNMA().stoppingDistance = 0;
        unit.GetNMA().SetDestination(new Vector3(tPos.x + x, 0, tPos.z + z));
    }
}

