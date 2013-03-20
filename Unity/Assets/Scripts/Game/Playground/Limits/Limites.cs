using UnityEngine;
using System.Collections;

/**
 * @class Game
 * @brief Trigger définissant les limites à ne pas franchir
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Game/Limites")]
public class Limites : TriggeringTrigger
{
	
	public Game game;

    public override void Entered(Triggered t)
    {
        Ball b = t.GetComponent<Ball>();
        if (b != null)
        {		
			if(game.state == Game.State.PLAYING) {
				// La balle � d�pass� les bornes !
            	Debug.Log("Hors limites : [Replace au centre]");
            	b.setPosition(Vector3.zero);  
			}
			
			if(game.state == Game.State.TRANSFORMATION) {
				TransformationManager tm = game.GetComponent<TransformationManager>();
				tm.OnLimit();
			}
        }
    }
}
