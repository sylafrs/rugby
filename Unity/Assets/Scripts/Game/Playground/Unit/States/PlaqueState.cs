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
        unit.isTackled = true;
        unit.nma.Stop();

        if (unit == unit.Game.Ball.Owner)
        {
            unit.Game.Ball.Put();
        }

        if (unit.Team.useColors && unit.Model && unit.Model.renderer)
        {
			foreach (var mat in unit.Model.renderer.materials)
			{
				mat.color = unit.Team.PlaqueColor;
			}
		}

        unit.Model.transform.localRotation = Quaternion.Euler(90, 0, 0);
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
        unit.isTackled = false;

		if (unit.Team.useColors && unit.Model && unit.Model.renderer) {
			foreach (var mat in unit.Model.renderer.materials)
			{
				mat.color = unit.Team.Color;
			}
		}

        unit.Model.transform.localRotation = Quaternion.identity;
	}
}

