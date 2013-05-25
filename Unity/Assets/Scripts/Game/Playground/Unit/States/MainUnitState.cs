/**
 * @class OrderState
 * @brief Etat qui gère l'unité
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
public class MainUnitState : UnitState
{
    public MainUnitState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override void OnEnter()
    {
        decide();
    }

    public override bool OnNearBall()
    {
       /* // FIX. (TODO)
        if (unit.Order.type == Order.TYPE.TRIANGLE)
        {
            return false;
        }*/

        if ( unit.game.Ball.Owner == null && unit.canCatchTheBall /*&& this.unit.game.state == Game.State.PLAYING*/)
        {
			unit.game.Ball.Owner = unit;
            return true;
        }

        return false;
    }

    public override bool OnNewOrder()
    {
        decide();
        return true;
    }

    public override bool OnTackle()
    {
        sm.state_change_me(this, new PlaqueState(sm, unit));
        return true;
    }

    public override bool OnStun(float d)
    {
        sm.state_change_me(this, new PlaqueState(sm, unit, d));
        return true;
    }

	bool first = true;
    public override void OnUpdate()
    {
	    if (first && unit.game.disableIA)
        {
			first = false;
            sm.state_change_son(this, new IdleState(sm, unit));
            return;
        }   
		if(!unit.game.disableIA)
			first = true;
    }

    public void decide()
    {
        if (unit.game.disableIA)
        {
            sm.state_change_son(this, new IdleState(sm, unit));
            return;
        }

     
        switch (this.unit.Order.type)
        {
            case Order.TYPE.DODGE:
                sm.state_change_son(this, new DodgeState(sm, unit));
                break;

            case Order.TYPE.NOTHING:
                sm.state_change_son(this, new IdleState(sm, unit));
                break;

            case Order.TYPE.MOVE:
                sm.state_change_son(this, new MoveState(sm, unit));
                break;

            case Order.TYPE.SEARCH:
            case Order.TYPE.FOLLOW:
                sm.state_change_son(this, new FollowState(sm, unit));
                break;

            case Order.TYPE.DROPKICK:
                if (unit.Team.game.Ball.Owner == unit)
					unit.Team.game.Ball.Drop(DropManager.TYPEOFDROP.KICK);
                break;

			case Order.TYPE.DROPUPANDUNDER:
				if (unit.Team.game.Ball.Owner == unit)
					unit.Team.game.Ball.Drop(DropManager.TYPEOFDROP.UPANDUNDER);
				break;

			case Order.TYPE.PASS:
				if (unit.Team.game.Ball.Owner == unit)
					unit.Team.game.Ball.Pass(unit.Order.target);
				break;

            case Order.TYPE.TRIANGLE:
                sm.state_change_son(this, new TriangleFormationState(sm, unit));
                break;

			case Order.TYPE.DEFENSIVE_SIDE:
				sm.state_change_son(this, new DefensiveSide(sm, unit));
				break;

            case Order.TYPE.LANE:
                sm.state_change_son(this, new LineFormationState(sm, unit));
                break;
                
            case Order.TYPE.TACKLE:
                Unit target = unit.Order.target;
                unit.game.OnTackle(unit, target);
                break;

            default:
                sm.state_change_son(this, new IdleState(sm, unit));
                break;

        }
    }	
}
