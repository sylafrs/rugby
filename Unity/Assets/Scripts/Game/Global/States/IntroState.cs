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
	
	private bool Moving;
	
	public override void OnEnter()
    {
		
		this.Moving = true;
		
		/*
        cam.transalateWithFade(new Vector3(0,0,-120), 4f, 1f, 1f ,1f, () =>{
			
			//cam.transalateWithFade(new Vector3(-200,0,0), 3f, 1f, 1f ,1f, () =>{
				//please, kill after usage x)
			
				//Moving = false;
			
				CameraFade.wannaDie();
			//});
			
		});
		*/
		
		this.stepBack();
    }
	
	private void stepBack(){
		cam.transalateWithFade(new Vector3(0,0,-10), 4f, 1f, 1f ,1f, () =>{
			//please, kill after usage x)
				
			stepBack();
			//CameraFade.wannaDie();
		});
	}
	
	public override void OnUpdate()
    {
		if(this.Moving)
			Camera.mainCamera.transform.Translate(0,0,0.08f,Space.Self);
    }
	
	public override void OnLeave()
    {
		//CameraFade.wannaDie();
		Debug.Log("intro leave");
		cam.setTarget(cam.game.Ball.Owner.transform);
		CameraFade.StartAlphaFade(Color.black,true, 2f, 2f, () => {
			CameraFade.wannaDie();
		});
    }
}