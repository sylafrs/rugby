/**
  * @class ScrumState
  * @brief Etat de la caméra durant une mêlée
  * @author Sylvain Lafon
  * @see CameraState
  */
using UnityEngine;

public class ScrumState : CameraState
{
    public ScrumState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
	
	void initCutScene(){
		cam.setTarget(null);
	}
	
	public override void OnEnter()
    {
		//initCutScene();
		//MyDebug.Log("previous owner "+cam.game.Ball.PreviousOwner);

       // cam.game.arbiter.ScrumCinematicMovement();
       // cam.game.arbiter.NowScrum();

    }
}