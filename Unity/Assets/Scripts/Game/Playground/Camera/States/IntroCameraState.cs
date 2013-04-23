/**
  * @class IntroCameraState
  * @brief Etat de la caméra au départ
  * @author Sylvain Lafon
  * @see CameraState
  */
using System.Collections;
using UnityEngine;


public class IntroCameraState : CameraState
{
    public IntroCameraState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
	
	
	public override void OnEnter()
    {
        cam.transalateWithFade(new Vector3(-200,0,0), 3f, 1f, 1f ,1f, () =>{
			cam.transalateWithFade(new Vector3(-200,0,0), 3f, 1f, 1f ,1f, () =>{
				//please, kill after usage x)
				CameraFade.wannaDie();
			});
			
		});
    }
	
	
	public override void OnLeave()
    {
       	Debug.Log("Leaving intro ...");
    }
	
}