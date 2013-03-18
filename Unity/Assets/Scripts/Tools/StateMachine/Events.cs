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

    public virtual bool OnPlaque()
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

    public void event_plaque()
    {
        foreach (State tmp in list)
        {
            if (tmp.OnPlaque())
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
}