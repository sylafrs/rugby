using UnityEngine;
using System.Collections.Generic;

/**
  * @class PassCameraState
  * @brief Etat de la caméra lorsque l'on fait une passe
  * @author Sylvain Lafon
  * @see CameraState
  */
public class PassCameraState : CameraState {
    public PassCameraState(StateMachine sm, CameraManager cam) : base(sm, cam) { }

    public override void OnEnter()
    {
        cam.setTarget(cam.game.Ball.transform);
    }	
}
