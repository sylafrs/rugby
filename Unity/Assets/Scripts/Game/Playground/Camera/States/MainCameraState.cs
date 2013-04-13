using UnityEngine;
using System.Collections.Generic;

/**
  * @class MainCameraState
  * @brief Etat principal de la caméra.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class MainCameraState : CameraState {

    public MainCameraState(StateMachine sm, CameraManager cam) : base(sm, cam) { }



}
