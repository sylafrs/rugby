/**
 * @class CameraState
 * @brief Etat de la caméra : patron de l'état pour la caméra
 * @author Sylvain Lafon
 */
public abstract class CameraState : State
{
    protected CameraManager cam;
    public CameraState(StateMachine sm, CameraManager cam)
        : base(sm)
    {
        this.cam = cam;
    }
}
