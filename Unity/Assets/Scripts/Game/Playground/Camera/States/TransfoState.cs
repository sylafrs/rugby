/**
  * @class TransfoState
  * @brief Etat de la caméra durant une transformation
  * @author Sylvain Lafon
  * @see CameraState
  */
using UnityEngine;

public class TransfoState : CameraState
{
    public TransfoState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
	
	public override void OnEnter ()
	{
		cam.setTarget(null);
		
		Transform cameraPlaceHolder = GameObject.Find("TransfoPlacement").transform.FindChild("ShootPlayer").
			transform.FindChild("CameraPlaceHolder");
		cameraPlaceHolder.LookAt(cam.game.Ball.Owner.transform);
		
		cam.transalateToWithFade(cameraPlaceHolder.position, cameraPlaceHolder.rotation, 0f, 1f, 1f, 0.5f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
            }, (/* OnFade */) => {
                //cam.setTarget(cam.game.Ball.Owner.transform);
            }
        );
	}	
	
}