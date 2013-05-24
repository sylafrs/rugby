using UnityEngine;
using System.Threading;

/**
  * @class RunningState
  * @brief Etat de la caméra durant une touche
  * @author Maxens Dubois
  * @see GameState
  */
public class RunningState : GameState
{
	public RunningState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

	public override void OnEnter()
	{
		Unit owner = this.game.Ball.Owner;

		if (owner)
		{
			sm.state_change_son(this, new BallHandlingState(sm, cam, game));
		}
		else
		{
			sm.state_change_son(this, new BallFreeState(sm, cam, game));
		}
	}

	public override void OnUpdate()
	{
		var p1 = this.game.southTeam.Player;
		var p2 = this.game.northTeam.Player;

		if (p1 != null) p1.myUpdate();
		if (p2 != null) p2.myUpdate();

	}

	public override void OnLeave()
	{
		foreach (Unit u in game.northTeam)
		{
			u.Order = Order.OrderNothing();
		}

		foreach (Unit u in game.southTeam)
		{
			u.Order = Order.OrderNothing();
		}
	}

	public override bool OnConversion(But but)
	{
		game.Referee.OnDropTransformed(but);
		return true; // Could call signal
	}
}