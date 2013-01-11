using UnityEngine;
using System.Collections;

/**
 * @class UnitState
 * @brief Etat d'unité : patron de l'état pour une unité
 * @author Sylvain Lafon
 */
public abstract class UnitState : State {

    protected Unit unit;
    public UnitState(StateMachine sm, Unit unit) : base(sm)
    {
        this.unit = unit;
    }

}
