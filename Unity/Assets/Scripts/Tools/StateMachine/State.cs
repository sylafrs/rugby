using UnityEngine;

/**
 * @class State
 * @brief Etat (partie globale)
 * @author Sylvain Lafon
 */
public abstract partial class State
{
    protected StateMachine sm;

    public State(StateMachine cursm)
    {
        sm = cursm;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnLeave()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnChildLeaved()
    {
    }

    public virtual void OnChildChanged()
    {
    }

    public virtual string GetName()
    {
        return this.GetType().Name;
    }

    public override string ToString()
    {
        return this.GetName();
    }
}