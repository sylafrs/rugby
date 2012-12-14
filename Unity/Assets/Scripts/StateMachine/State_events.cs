using UnityEngine;

public abstract partial class State {

    public virtual bool OnNewOrder()
    {
        return (false);
    }

    public virtual bool OnBallChanged()
    {
        return (false);
    }
}
