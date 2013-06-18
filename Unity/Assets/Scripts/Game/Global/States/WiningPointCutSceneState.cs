using UnityEngine;
using System.Collections;

/**
  * @class WiningPointCutSceneState
  * @brief State du jeu au moment de la cutscene au moment ou on gagne les points
  * @author TEAM
  * @see GameState
  */
public class WiningPointCutSceneState : GameState {
	
	Team team;
	
	public WiningPointCutSceneState(StateMachine sm, CameraManager cam, Game game, Team _team): base(sm, cam, game){
		team = _team;
	}
	
	public override void OnEnter(){
		game.refs.managers.ui.currentState = UIManager.UIState.NULL;
		cam.ChangeCameraState(CameraManager.CameraState.FREE);
	    cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,1.5f, 
           (/* OnFinish */) => {
               //please, kill after usage x)
				game.refs.managers.ui.currentState = UIManager.UIState.GameUI;
				cam.WiningPointsCutSceneComponent.StartScene( () => { }, team);
               	CameraFade.wannaDie();
           }, (/* OnFade */) => {
				cam.zoom = 1; //TODO cam settings
				cam.game.Referee.StartPlacement();
				this.cam.CameraRotatingAroundComponent.StartEndlessRotation(
					game.refs.positions.rotationCenter,
					new Vector3(0,1,0),
					game.refs.positions.fieldCenter,
					game.refs.positions.cameraFirstPosition,
					cam.game.settings.GameStates.MainState.IntroState.rotationSpeed/100);	
           }
       	);
	}
	
	public override void OnLeave(){
		cam.transalateWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,1.5f, 
           (/* OnFinish */) => {
               //please, kill after usage
               	CameraFade.wannaDie();
           }, (/* OnFade */) => {
           }
       	);
	}
}
