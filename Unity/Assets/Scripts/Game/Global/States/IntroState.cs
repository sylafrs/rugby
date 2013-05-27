/**
  * @class IntroState
  * @brief Etat de la caméra au départ
  * @author Sylvain Lafon
  * @see GameState
  */
using System;
using System.Collections;
using UnityEngine;


public class IntroState : GameState
{
	public IntroState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }

	// Petit tp + fondu
	public override void OnEnter()
	{
		this.stepBack();
	}

	// Petit tp + fondu, se rappelle quand se termine.
	private void stepBack()
	{
		cam.transalateWithFade(new Vector3(0, 0, -10), 4f, 1f, 1f, 1f, () =>
		{
			stepBack();
		});
	}

	// Recule sans arrêt
	public override void OnUpdate()
	{
		Camera.mainCamera.transform.Translate(0, 0, 0.08f, Space.Self);

		//foreach (Unit u in game.northTeam)
		//{
		//	u.UpdateTypeOfPlay();
		//}

		//foreach (Unit u in game.southTeam)
		//{
		//	u.UpdateTypeOfPlay();
		//}
	}

	// On va vers la cible, on fait un fondu (en écrasant le précédent).
	public override void OnLeave()
	{
		cam.setTarget(cam.game.Ball.Owner.transform);
		CameraFade.StartAlphaFade(Color.black, true, 2f, 2f, () =>
		{
			CameraFade.wannaDie();
		});
	}
}