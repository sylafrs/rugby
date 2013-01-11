using UnityEngine;

/**
 * @class State
 * @brief Etat (partie jeu)
 * @author Sylvain Lafon
 */
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
