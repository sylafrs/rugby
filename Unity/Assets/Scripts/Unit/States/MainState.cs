using UnityEngine;
using System.Collections;

public class MainState : UnitState {

    public MainState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override void OnEnter()
    {
        decide();
    }

    public override void OnChildLeaved()
    {
        decide();    
    }

    public void decide() {
        switch (this.unit.GetOrder().type)
        {
            case Order.TYPE.RIEN:
                sm.state_change_son(this, new IdleState(sm, unit));
            break;

        }
    }
	
}
