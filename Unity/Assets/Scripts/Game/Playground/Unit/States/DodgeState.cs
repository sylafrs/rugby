using UnityEngine;
using System.Collections.Generic;

/**
  * @class DodgeState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class DodgeState : UnitState {
    public DodgeState(StateMachine sm, Unit unit) : base(sm, unit) {
        direction = unit.Order.point;
    }

    Vector3 direction;

    public override void OnEnter()
    {
        unit.Dodge = true;
        unit.Team.setSpeed();
        unit.nma.SetDestination(unit.transform.position + direction);
    }

    public override void OnUpdate()
    {
        unit.nma.SetDestination(unit.transform.position + direction);

        if (!unit.Dodge)
        {
            unit.nma.Stop();
            unit.Order = Order.OrderNothing();
        }
    }

    public override void OnLeave()
    {
        unit.Team.setSpeed();
    }
}
