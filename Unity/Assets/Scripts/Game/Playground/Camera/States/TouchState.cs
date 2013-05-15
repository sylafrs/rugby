/**
  * @class TouchState
  * @brief Etat de la caméra durant une touche
  * @author Sylvain Lafon
  * @see CameraState
  */
using UnityEngine;

public class TouchState : CameraState
{
    public TouchState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
	
 	public override void OnEnter ()
	{	
		cam.setTarget(null);
		
		Transform cameraPlaceHolder = GameObject.Find("TouchPlacement").transform.FindChild("CameraPlaceHolder");

        cam.transalateToWithFade(cameraPlaceHolder.position, cameraPlaceHolder.rotation, 0f, 1f, 1f, 0.3f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
            }, (/* OnFade */) => { 
				cam.CancelNextFlip = true;
                cam.game.arbiter.PlacePlayersForTouch();
            }
        );
		
	}
	
	public override void OnLeave ()
	{
		cam.setTarget(cam.game.Ball.transform);	
	}
}