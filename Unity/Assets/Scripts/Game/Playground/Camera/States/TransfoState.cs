/**
  * @class TransfoState
  * @brief Etat de la caméra durant une transformation
  * @author Sylvain Lafon
  * @see CameraState
  */
using System.Collections.Generic;
using UnityEngine;

public class TransfoState : CameraState
{
    public TransfoState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
	
	public override void OnEnter ()
	{
		cam.setTarget(null);
		
		Vector3 offset = Camera.mainCamera.transform.position+(cam.MinfollowOffset)*cam.zoom;
		
		cam.transalateToWithFade(cam.game.Ball.Owner.transform.position - offset, cam.game.Ball.Owner.transform.rotation, 0f, 1f, 2.5f, 0.5f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
            }, (/* OnFade */) => {
                cam.setTarget(cam.game.Ball.Owner.transform);
            }
        );
	}	
	
}