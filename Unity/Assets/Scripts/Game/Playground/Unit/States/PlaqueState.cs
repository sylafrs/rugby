using UnityEngine;

/**
  * @class PlaqueState 
  * @brief Etat d'une unité plaquée
  * @author Sylvain Lafon
  */
class PlaqueState : UnitState
{
    // Tackle
    public PlaqueState(StateMachine sm, Unit u) : base(sm, u) 
    {
        this.tMax = unit.game.settings.GameStates.MainState.PlayingState.MainGameState.TacklingState.tackledTime;
        this.rotate = true;
    }

    // Stun
    public PlaqueState(StateMachine sm, Unit u, float duration) : base(sm, u) 
    {
        this.tMax = duration;
        this.rotate = false;
    }

    // Remise à zero :p
    public override bool OnStun(float d)
    {
        this.t = 0;
        this.tMax = d;

        return false;
    }
    
    float t;
    float tMax;
    bool rotate;

    public override bool OnUntackle()
    {
        sm.state_change_me(this, new MainUnitState(sm, unit));
        return true;
    }

    public override void OnEnter()
    {
        t = 0;
        unit.isTackled = true;
        unit.nma.Stop();

        if (unit == unit.game.Ball.Owner)
        {
            unit.game.Ball.Put();
        }

        if (unit.Team.useColors && unit.Model && unit.Model.renderer)
        {
			foreach (var mat in unit.Model.renderer.materials)
			{
				mat.color = unit.Team.PlaqueColor;
			}
		}

        if(rotate)
            unit.Model.transform.localRotation = Quaternion.Euler(90, 0, 0);

        UnitAnimator ua = unit.GetComponent<UnitAnimator>();
        if (ua != null)
        {
            ua.Tackled = true;
        }
    }

    public override void OnUpdate()
    {
        t += UnityEngine.Time.deltaTime;
        if (t > tMax)
        {
            this.OnUntackle();
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

        if(rotate)
            unit.Model.transform.localRotation = Quaternion.identity;

        UnitAnimator ua = unit.GetComponent<UnitAnimator>();
        if (ua != null)
        {
            ua.Tackled = false;
        }
	}
}

