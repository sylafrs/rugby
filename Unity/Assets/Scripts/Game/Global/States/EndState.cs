using System;
using System.Collections;
using UnityEngine;

/**
  * @class EndState
  * @brief Etat de la caméra à la fin du jeu
  * @author Sylvain Lafon
  * @see GameState
  */
public class EndState : GameState
{
    public EndState(StateMachine sm, CameraManager cam, Game game) : base(sm, cam, game) { }
	
    // On entre dans l'état de fin
	public override void OnEnter()
    {
		base.OnEnter();
		cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0, 1f, 1f, 1.5f, 
		     (/* OnFinish */) => {
	    	    CameraFade.wannaDie();				
				game.refs.managers.ui.currentState = UIManager.UIState.EndUI;
	     	}, (/* OnFade */) => {
				cam.zoom = 1;
				this.cam.CameraRotatingAroundComponent.StartEndlessRotation(
				game.refs.positions.rotationCenter,
				new Vector3(0,1,0),
				game.refs.positions.fieldCenter,
				game.refs.positions.cameraFirstPosition,
				cam.game.settings.GameStates.MainState.IntroState.rotationSpeed/100);	
		     	}
		 );
    }
	
    // Ceci est un non-sens// ou pas ? Si on redémarre une partie de façon "propre" un jour :p
	public override void OnLeave()
	{
        //game.refs.managers.ui.currentState = UIManager.UIState.NULL;
	}
}