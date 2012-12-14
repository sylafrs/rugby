using UnityEngine;
using System.Collections;

public abstract class UnitState : State {

    protected Unit unit;
    public UnitState(StateMachine sm, Unit unit) : base(sm)
    {
        this.unit = unit;
    }

}
