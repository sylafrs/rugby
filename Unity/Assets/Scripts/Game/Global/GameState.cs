using System.IO;
using UnityEngine;
/**
 * @class GameState
 * @brief Etat de la caméra : patron de l'état pour la caméra
 * @author Sylvain Lafon
 */
public abstract class GameState : State
{
    protected CameraManager 	cam;
	protected Game			 	game;
	protected LogFile			StateMachineLog = new LogFile();
	
    public GameState(StateMachine _sm, CameraManager _cam, Game _game)
        : base(_sm)
    {
        this.cam  = _cam;
		this.game = _game;
    }
	
	public override void OnEnter ()
	{
		StateMachineLog.SetName("Assets/Scripts/Game/Autres/LOG/StateMachine");
		StateMachineLog.WriteLine(System.DateTime.Now + "------ENTER IN " + GetName() + "------");
	}
}
