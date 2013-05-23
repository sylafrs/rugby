using UnityEngine;
using System.Collections.Generic;

/**
  * @class ConversionFlyState
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class ConversionFlyState : GameState
{
    public ConversionFlyState(StateMachine sm, CameraManager cam, Game game, Zone z)
        : base(sm, cam, game)
    {
        //this.zone = z;
    }

    //private Zone zone;

    public override bool OnTouch(Touche t)
    {
        game.refs.managers.conversion.OnLimit();
        return true; // Could call signal
    }

    public override bool OnBallOut()
    {
        game.refs.managers.conversion.OnLimit();
        return true; // Could call signal
    }

    public override bool OnConversion(But but)
    {
        game.refs.managers.conversion.But();
        return true; // Could call signal
    }

    public override void OnEnter ()
    {
	    cam.setTarget(cam.game.Ball.transform);
	    cam.zoom = 0.1f;
    }

    public override void OnLeave ()
    {
	    cam.zoom = 1f;
        cam.setTarget(game.Referee.unitToGiveBallAfterConversion().transform);	
	    cam.transalateToWithFade(Vector3.zero, Quaternion.identity, 0f, 1f, 1f,2f, 
            (/* OnFinish */) => {
                //please, kill after usage x)
                CameraFade.wannaDie();
            }, (/* OnFade */) => {
                cam.game.Referee.StartPlacement();
            }
        );
    }    
}