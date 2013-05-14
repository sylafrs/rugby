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
		
		//Transform cameraPlaceHolder = cam.game.Ball.Owner.transform.FindChild("TransfoCamPlaceholder");
		Transform cameraPlaceHolder = GameObject.Find("TransfoPlacement").transform.FindChild("ShootPlayer").
			FindChild("CameraPlaceHolder");
		
		//cameraPlaceHolder.LookAt(cam.game.Ball.Owner.transform);
		
		GameObject Goal = null;
		if(cam.flipedForTeam == cam.game.right)
		{
			Goal = GameObject.Find("but_maori");
			cameraPlaceHolder.LookAt(Goal.transform);
			
		}
		if(cam.flipedForTeam == cam.game.left)
		{
			Goal = GameObject.Find("but_jap");
			cameraPlaceHolder.LookAt(Goal.transform);
		}
		
		//Vector3 PlayerToGoalDir = cam.game.Ball.Owner.transform.position - Goal.transform.position;
		Vector3 GoalToPlayer = cam.game.Ball.Owner.transform.position - Goal.transform.position;
		Vector3	GoalToCam	 = Camera.mainCamera.transform.position - Goal.transform.position;
		Vector3 Proj		 = Vector3.Project(cameraPlaceHolder.transform.position,GoalToPlayer);
		
		cam.transalateToWithFade(new Vector3(Proj.x,Proj.y,Proj.z), cameraPlaceHolder.rotation,
			
			0f, 1f, 1f, 1f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
				//cam.setTarget(cam.game.Ball.Owner.transform);
				//cam.setTarget(null);
			
                CameraFade.wannaDie();
            }, (/* OnFade */) => {
				//cam.setTarget(cam.game.Ball.Owner.transform);
				Camera.mainCamera.transform.LookAt(Goal.transform);
            }
        );
	}	
	
}