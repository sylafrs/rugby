using UnityEngine;

public abstract class State
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

    public virtual bool OnNewOrder()
    {
        return (false);
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

    public virtual string ToString()
    {
        return this.GetName();
    }
}