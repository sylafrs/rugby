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
		
		Camera.mainCamera.transform.position = cameraPlaceHolder.position;
		Camera.mainCamera.transform.rotation = cameraPlaceHolder.rotation;
		
	}
	
	public override void OnLeave ()
	{
		cam.setTarget(cam.game.Ball.transform);
	}
}