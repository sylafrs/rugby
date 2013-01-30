/**
 * @class IdleState
 * @brief Etat inerte : ne fait rien de particulier
 * @author Sylvain Lafon
 */
public class IdleState : UnitState {

    public IdleState(StateMachine sm, Unit unit) : base(sm, unit) { }

    public override void OnEnter()
    {
		unit.GetNMA().Stop();
    }
    
}
