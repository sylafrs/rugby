/**
  * @class FollowPlayerState
  * @brief Etat de la caméra lorsqu'elle doit suivre un joueur.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class FollowPlayerState : CameraState {
    public FollowPlayerState(StateMachine sm, CameraManager cam) : base(sm, cam) { }

    public override void OnEnter()
    {
        cam.setTarget(cam.game.Ball.Owner.transform);
    }

    public override bool OnNewOwner(Unit old, Unit current)
    {
        if (current != null)
        {
            cam.setTarget(current.transform);
        }

        return false;
    }

    public override bool OnPass(Unit from, Unit to)
    {
        sm.state_change_me(this, new PassCameraState(sm, cam));
        return true;
    }
}
