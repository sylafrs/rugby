/**
  * @class SprintState
  * @brief Etat de la caméra durant un sprint
  * @author Sylvain Lafon
  * @see GameState
  */
public class SprintState : GameState
{
    public SprintState(StateMachine sm, CameraManager cam, Game game, Unit u) : base(sm, cam, game) { this.unit = u; }

    Unit unit;

    // Check if the unit stops sprinting..
    public override bool OnSprint(Unit u, bool sprinting)
    {
        if (!sprinting && u == this.unit)
        {
            sm.state_kill_me(this);
            return true;
        }

        return false;
    }
}