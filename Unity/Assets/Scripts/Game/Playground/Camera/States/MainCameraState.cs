using UnityEngine;

/**
  * @class MainCameraState
  * @brief Etat principal de la caméra.
  * @author Sylvain Lafon
  * @see CameraState
  */
public class MainCameraState : CameraState {

    public MainCameraState(StateMachine sm, CameraManager cam) : base(sm, cam) { }

    public override void OnEnter()
    {
        this.decide(Game.State.NULL, cam.game.state);
    }

    public override bool OnGameStateChanged(Game.State old, Game.State current)
    {
        if (old == current)
        {
            throw new UnityException("OnGameStateChanged called without state changement..\nHow strange !");
        }

        return this.decide(old, current);
    }

    public bool decide(Game.State old, Game.State current)
    {
        if (current == Game.State.INTRODUCTION)
        {
            sm.state_change_son(this, new IntroCameraState(sm, cam));
            return true;
        }

        if (current == Game.State.PLAYING)
        {
            sm.state_change_son(this, new PlayingState(sm, cam));
            return true;
        }

        if (current == Game.State.SCRUM)
        {
            sm.state_change_son(this, new ScrumState(sm, cam));
            return true;
        }

        if (current == Game.State.TOUCH)
        {
            sm.state_change_son(this, new TouchState(sm, cam));
            return true;
        }

        if (current == Game.State.TRANSFORMATION)
        {
            sm.state_change_son(this, new TransfoState(sm, cam));
            return true;
        }

        if (current == Game.State.END)
        {
            sm.state_change_son(this, new EndState(sm, cam));
            return true;
        }

        return false;
    }
}
