using UnityEngine;
using System.Collections;

/**
  * @class WiningPointCutSceneState
  * @brief State du jeu au moment de la cutscene au moment ou on gagne les points
  * @author TEAM
  * @see GameState
  */
public class WiningPointCutSceneState : GameState {
	
	public WiningPointCutSceneState(StateMachine sm, CameraManager cam, Game game): base(sm, cam, game){
	}
	
	public override void OnEnter(){
		cam.WiningPointsCutSceneComponent.StartScene();
	}
}
