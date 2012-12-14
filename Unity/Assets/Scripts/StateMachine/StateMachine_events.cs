using UnityEngine;
using System.Collections;

/* Ici : events de la StateMachine */

public partial class StateMachine {

    public void event_neworder()
    {
        foreach (State tmp in list)
        {
            if (tmp.OnNewOrder())
                return;
        }
    }

}
