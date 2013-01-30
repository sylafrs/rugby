/**
 * @class OrderState
 * @brief Etat qui gère l'unité
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
public class MainState : UnitState
{
    public MainState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override void OnEnter()
    {
        decide();
    }

    public override bool OnNearBall()
    {
        // FIX. (TODO)
        if (unit.Order.type == Order.TYPE.TRIANGLE)
        {
            return false;
        }

        if (unit.Game.Ball.Owner == null)
        {
            unit.Game.Ball.Taken(unit);
            return true;
        }

        return false;
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

	bool first = true;
    public override void OnUpdate()
    {
        if (first && unit.Game.disableIA)
        {
			first = false;
            sm.state_change_son(this, new IdleState(sm, unit));
            return;
        }   
		if(!unit.Game.disableIA)
			first = true;
    }

    public void decide()
    {
        if (unit.Game.disableIA)
        {
            sm.state_change_son(this, new IdleState(sm, unit));
            return;
        }         

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

            case Order.TYPE.DROP:
                if (unit.Team.Game.Ball.Owner == unit)
					unit.Team.Game.Ball.Drop();
                break;

			case Order.TYPE.PASS:
				if (unit.Team.Game.Ball.Owner == unit)
					unit.Team.Game.Ball.Pass(unit.Order.target);
				break;

            case Order.TYPE.TRIANGLE:
                sm.state_change_son(this, new TriangleFormationState(sm, unit));
                break;

            case Order.TYPE.LIGNE:
                sm.state_change_son(this, new LineFormationState(sm, unit));
                break;
                
            case Order.TYPE.PLAQUER:
                Unit target = unit.Order.target;

                target.sm.event_plaque();
                unit.sm.event_plaque();

                unit.Game.EventTackle(unit, target);
                break;

            default:
                sm.state_change_son(this, new IdleState(sm, unit));
                break;

        }
    }	
}
