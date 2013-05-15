/**
  * @class TransfoState
  * @brief Etat de la caméra durant une transformation
  * @author Sylvain Lafon
  * @see CameraState
  */
using UnityEngine;

public class TransfoShotState : CameraState
{
    public TransfoShotState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
	
	public override void OnEnter ()
	{
		cam.setTarget(cam.game.Ball.transform);
		cam.zoom = 0.1f;
	}
	
	public override void OnLeave ()
	{
		cam.zoom = 1f;
		
		
		cam.transalateToWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,2f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
            }, (/* OnFade */) => {
                cam.game.arbiter.StartPlacement();
            }
        );
	}
}
	