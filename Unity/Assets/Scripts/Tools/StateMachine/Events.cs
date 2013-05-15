/**
 * @class State
 * @brief Etat (partie jeu)
 * @author Sylvain Lafon
 */
public abstract partial class State
{
    public virtual bool OnNearUnit(Unit u)
    {
        return (false);
    }

    public virtual bool OnNearBall()
    {
        return (false);
    }

    public virtual bool OnNewOrder()
    {
        return (false);
    }

    public virtual bool OnTackle()
    {
        return (false);
    }

    public virtual bool OnNewOwner(Unit old, Unit current)
    {
        return (false);
    }

    public virtual bool OnPass(Unit from, Unit to)
    {
        return (false);
    }

    public virtual bool OnDodge(Unit u)
    {
        return (false);
    }

    public virtual bool OnSprint(Unit u, bool sprinting)
    {
        return (false);
    }

    public virtual bool OnGameStateChanged(Game.State old, Game.State current)
    {
        return (false);
    }

    public virtual bool OnBallOnGround(bool onGround)
    {
        return (false);
    }

    public virtual bool OnDrop()
    {
        return (false);
    }

    public virtual bool OnSuper(Team t, SuperList super)
    {
        return (false);
    }
	
	public virtual bool OnTranfoShot()
    {
        return (false);
    }

    public virtual bool OnDropTransformed(But b)
    {
        return (false);
    }

    public virtual bool OnBallOut()
    {
        return (false);
    }
}

/**
 * @class StateMachine
 * @brief Machine d'états finis (partie jeu)
 * @author Sylvain Lafon
 */
public partial class StateMachine
{
    public void event_neworder()
    {
        foreach (State tmp in list)
        {
            if (tmp.OnNewOrder())
                return;
        }
    }

    public void event_Tackle()
    {
        foreach (State tmp in list)
        {
            if (tmp.OnTackle())
                return;
        }
    }

    public void event_NearUnit(Unit u)
    {
        foreach (State tmp in list)
        {
            if (tmp.OnNearUnit(u))
                return;
        }
    }

    public void event_NearBall()
    {
        foreach (State tmp in list)
        {
            if (tmp.OnNearBall())
                return;
        }
    }

    public void event_GameStateChanged(Game.State old, Game.State current)
    {
        foreach (State tmp in list)
        {
            if (tmp.OnGameStateChanged(old, current))            
                return;            
        }
    }

    public void event_NewOwner(Unit old, Unit current)
    {
        foreach (State tmp in list)
        {
            if (tmp.OnNewOwner(old, current))
                return;
        }
    }

    public void event_Pass(Unit from, Unit to)
    {
        foreach (State tmp in list)
        {
            if (tmp.OnPass(from, to))
                return;
        }
    }

    public void event_BallOnGround(bool onGround) {
        foreach (State tmp in list)
        {
            if (tmp.OnBallOnGround(onGround))
                return;
        }
    }

    public void event_Sprint(Unit unit, bool sprinting)
    {
        foreach (State tmp in list)
        {
            if (tmp.OnSprint(unit, sprinting))
                return;
        }
    }

    public void event_Dodge(Unit unit)
    {
        foreach (State tmp in list)
        {
            if (tmp.OnDodge(unit))
                return;
        }
    }
	
	public void event_TransfoShot(){
		foreach (State tmp in list)
        {
            if (tmp.OnTranfoShot())
                return;
        }
	}
	
    public void event_Drop()
    {
        foreach (State tmp in list)
        {
            if (tmp.OnDrop())
                return;
        }
    }

    public void event_Super(Team t, SuperList super)
    {
        foreach (State tmp in list)
        {
            if (tmp.OnSuper(t, super))
                return;
        }
    }

    public void event_DropTransformed(But but)
    {
        foreach (State tmp in list)
        {
            if (tmp.OnDropTransformed(but))
                return;
        }
    }

    public void event_BallOut()
    {
        foreach (State tmp in list)
        {
            if (tmp.OnBallOut())
                return;
        }
    }
}