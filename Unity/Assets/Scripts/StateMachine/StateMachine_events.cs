using UnityEngine;
using System.Collections;

/**
 * @class StateMachine
 * @brief Machine d'états finis (partie jeu)
 * @author Sylvain Lafon
 */
public partial class StateMachine {

    public void event_neworder()
    {
        foreach (State tmp in list)
        {
            if (tmp.OnNewOrder())
                return;
        }
    }

    public void event_ballChanged()
    {
        foreach (State tmp in list)
        {
            if (tmp.OnBallChanged())
                return;
        }
    }
}
