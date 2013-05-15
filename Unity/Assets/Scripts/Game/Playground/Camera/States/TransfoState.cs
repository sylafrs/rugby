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
			FindChild("CameraPlaceHolder");
		
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
		
		cam.transalateToWithFade(Vector3.zero, cameraPlaceHolder.rotation,0f, 1f, 1f, 1f, 
            (/* OnFinish */) => {
                CameraFade.wannaDie();
            }, (/* OnFade */) => {
				cam.game.arbiter.PlacePlayersForTransfo();
				Vector3 GoalToPlayer = cam.game.Ball.Owner.transform.position - Goal.transform.position;
				Vector3	GoalToCam	 = cameraPlaceHolder.transform.position - Goal.transform.position;
				Vector3 Proj		 = Vector3.Project(GoalToCam,GoalToPlayer);
				float saveY 		 = cameraPlaceHolder.transform.position.y;
				Vector3 dest		 = new Vector3(Proj.x + Goal.transform.position.x,saveY,Proj.z + Goal.transform.position.z);
				Camera.mainCamera.transform.position = dest;
				Camera.mainCamera.transform.LookAt(Goal.transform);
				cam.game.arbiter.EnableTransformation();
            }
        );
	}	
	
	/*
	public override void OnUpdate()
	{
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
		Vector3	GoalToCam	 = cameraPlaceHolder.transform.position - Goal.transform.position;
		Vector3 Proj		 = Vector3.Project(GoalToCam,GoalToPlayer);
		float saveY 		 = cameraPlaceHolder.transform.position.y;
		Vector3 dest		 = new Vector3(Proj.x + Goal.transform.position.x,saveY,Proj.z + Goal.transform.position.z);
		
		Camera.mainCamera.transform.position=des;
	}
	*/
	
	public override bool OnTranfoShot()
    {
		sm.state_change_son(this, new TransfoShotState(sm, cam));
		return true;
	}
}