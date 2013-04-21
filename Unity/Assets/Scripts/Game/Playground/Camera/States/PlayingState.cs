/**
  * @class PlayingState
  * @brief Etat de la caméra durant le jeu
  * @author Sylvain Lafon
  * @see CameraState
  */
public class PlayingState : CameraState
{
    public PlayingState(StateMachine sm, CameraManager cam) : base(sm, cam) { }

    public override void OnEnter()
    {
        decide();
    }

    public override void OnLeave()
    {
        // cam.setTarget(null);
    }

    private bool decide()
    {
        Unit ballOwner = cam.game.Ball.Owner;

        if (ballOwner == null)
        {
            sm.state_change_son(this, new GroundBallState(sm, cam));
        }
        else
        {
            sm.state_change_son(this, new FollowPlayerState(sm, cam, ballOwner));
        }

        return true;
    }

    public override bool OnNewOwner(Unit old, Unit current)
    {
        if (current != null)
        {
            sm.state_change_me(this, new FollowPlayerState(sm, cam, current));
            return true;
        }

        return false;
    }

    public override bool OnBallOnGround(bool onGround)
    {
        if (onGround)
        {
            sm.state_change_me(this, new GroundBallState(sm, cam));
            return true;
        }

        return false;
    }

    public override bool OnSuper(Team t, SuperList super)
    {
        // Fin du super
        if (super == SuperList.superNull)
        {
            // ....
        }
        else
        {
            // ....
        }

        return false;
    }
}
