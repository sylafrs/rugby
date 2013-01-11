using UnityEngine;
using System.Collections;

/**
 * @class MainState
 * @brief Etat principal : Etat parent d'une unité
 * @author Sylvain Lafon
 */
public class MainState : UnitState {

    public MainState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override void OnEnter()
    {
        decide();
    }

    public override bool OnNewOrder()
    {
        decide();
        return true;
    }

    public void decide()
    {
        switch (this.unit.GetOrder().type)
        {
            case Order.TYPE.RIEN:
                sm.state_change_son(this, new IdleState(sm, unit));
                break;

            case Order.TYPE.DEPLACER:
                sm.state_change_son(this, new MoveState(sm, unit));
                break;

            case Order.TYPE.SUIVRE:
                sm.state_change_son(this, new FollowState(sm, unit));
                break;

            case Order.TYPE.PASSER:
                if (unit.Team.Game.Ball.Owner == unit)
                    unit.Team.Game.Ball.ShootTarget(unit.GetOrder().target);
                break;
                
            default:
                sm.state_change_son(this, new IdleState(sm, unit));
                break;

        }
    }	
}
