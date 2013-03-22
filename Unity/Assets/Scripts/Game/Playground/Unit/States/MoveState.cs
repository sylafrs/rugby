using UnityEngine;

/**
 * @class MoveState
 * @brief Etat déplacement : déplace une unité à une position donnée
 * @author Sylvain Lafon
 */
public class MoveState : UnitState {

    const float epsilon = 0.1f;
       
    public MoveState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override void OnEnter()
    {
        unit.nma.stoppingDistance = 0f;
        unit.nma.SetDestination(unit.Order.point);
    }

    public override void OnUpdate()
    {       
        if (unit.nma.remainingDistance < epsilon)
        {
            unit.nma.Stop();
            unit.Order = Order.OrderNothing();
        }
    }
}
