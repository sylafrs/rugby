/**
  * @class TransfoState
  * @brief Etat de la caméra durant une transformation
  * @author Sylvain Lafon
  * @see CameraState
  */
public class TransfoState : CameraState
{
    public TransfoState(StateMachine sm, CameraManager cam) : base(sm, cam) { }
}