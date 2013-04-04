using UnityEngine;

/**
  * @class PlaqueState 
  * @brief Etat d'une unité plaquée
  * @author Sylvain Lafon
  */
class PlaqueState : UnitState
{
    public PlaqueState(StateMachine sm, Unit u) : base(sm, u) { }

    float t;

    public override void OnEnter()
    {
        t = 0;
        unit.nma.Stop();

        if (unit == unit.Game.Ball.Owner)
        {
            unit.Game.Ball.Put();
        }

		if(unit.Team.useColors) {
			foreach (var mat in unit.Model.materials)
			{
				mat.color = unit.Team.PlaqueColor;
			}
		}
    }

    public override void OnUpdate()
    {
        t += UnityEngine.Time.deltaTime;
        if (t > unit.Game.settings.timePlaque)
        {
            sm.state_change_me(this, new MainState(sm, unit));
        }
    }

	public override void OnLeave()
	{
		if (unit.Team.useColors) {
			foreach (var mat in unit.Model.materials)
			{
				mat.color = unit.Team.Color;
			}
		}
	}
}

