using UnityEngine;
using System.Collections.Generic;

/**
  * @class ConvertingState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class ConvertingState : GameState {

    public ConvertingState(StateMachine sm, CameraManager cam, Game game, Zone z) : base(sm, cam, game) {
        this.zone = z;
    }

    private Zone zone;

    public override void OnEnter()
    {
        camera_edited = false;
        game.Referee.OnTry();
        sm.state_change_son(this, new AimingConversionState(sm, cam, game, zone));        
    }

    public override bool OnConversionShot()
    {
        sm.state_change_son(this, new ConversionFlyState(sm, cam, game, zone));
        return true;
    }

    bool camera_edited;
    public override void OnUpdate()
    {
        if (!camera_edited)
        {
            UnitAnimator ua = this.game.Ball.Owner.unitAnimator;
            if (ua == null || ua.isInState(UnitAnimator.BallIdleState))
            {
                EditCamera();
            }            
        }
    }


	private void EditCamera ()
	{
        camera_edited = true;

		cam.setTarget(null);
		cam.ChangeCameraState(CameraManager.CameraState.FREE);
		
		Transform cameraPlaceHolder = GameObject.Find("TransfoPlacement").transform.FindChild("ShootPlayer").
			FindChild("CameraPlaceHolder");
		
		Debug.Log("hop "+cam.flipedForTeam);
		GameObject Goal = null;
		if(cam.flipedForTeam == cam.game.southTeam)
		{
			Goal = GameObject.Find("but_maori");
			cameraPlaceHolder.LookAt(Goal.transform);
			
		}
		if(cam.flipedForTeam == cam.game.northTeam)
		{
			Goal = GameObject.Find("but_jap");
			cameraPlaceHolder.LookAt(Goal.transform);
		}
		
		cam.transalateToWithFade(Vector3.zero, cameraPlaceHolder.rotation, 0f, 1f, 1f, 1f, 
            (/* OnFinish */) => {
                CameraFade.wannaDie();
            }, (/* OnFade */) => {
				cam.game.Referee.PlacePlayersForTransfo();

				Vector3 GoalToPlayer = cam.game.Ball.Owner.transform.position - Goal.transform.position;
				Vector3	GoalToCam	 = cameraPlaceHolder.transform.position - Goal.transform.position;
				Vector3 Proj		 = Vector3.Project(GoalToCam,GoalToPlayer);
				float saveY 		 = cameraPlaceHolder.transform.position.y;
				Vector3 dest		 = new Vector3(Proj.x + Goal.transform.position.x,saveY,Proj.z + Goal.transform.position.z);

				Camera.mainCamera.transform.position = dest;
				Camera.mainCamera.transform.LookAt(Goal.transform);
				cam.game.Referee.EnableTransformation();
            }
        );
    }
}