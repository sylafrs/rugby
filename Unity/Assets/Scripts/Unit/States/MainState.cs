/**
 * @class OrderState
 * @brief Etat qui gère l'unité
 * @author Sylvain Lafon
 */
public class MainState : UnitState
{
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

    public override bool OnPlaque()
    {
        sm.state_change_me(this, new PlaqueState(sm, unit));
        return true;
    }

    public void decide()
    {
        switch (this.unit.Order.type)
        {
            case Order.TYPE.RIEN:
                sm.state_change_son(this, new IdleState(sm, unit));
                break;

            case Order.TYPE.DEPLACER:
                sm.state_change_son(this, new MoveState(sm, unit));
                break;

            case Order.TYPE.CHERCHER:
            case Order.TYPE.SUIVRE:
                sm.state_change_son(this, new FollowState(sm, unit));
                break;

            case Order.TYPE.PASSER:
                if (unit.Team.Game.Ball.Owner == unit)
                    unit.Team.Game.Ball.ShootTarget(unit.Order.target);
                break;

            case Order.TYPE.TRIANGLE:
                sm.state_change_son(this, new TriangleFormationState(sm, unit));
                break;

            case Order.TYPE.LIGNE:
                sm.state_change_son(this, new LineFormationState(sm, unit));
                break;
                
            case Order.TYPE.PLAQUER:
                unit.Order.target.sm.event_plaque();
                unit.sm.event_plaque();
                if (unit.Order.target == unit.Game.Ball.Owner)
                {
                    unit.Game.Ball.OwnerPlaque();
                }
                break;

            default:
                sm.state_change_son(this, new IdleState(sm, unit));
                break;

        }
    }	
}
