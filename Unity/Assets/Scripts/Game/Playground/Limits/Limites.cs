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
				game.OnBallOut();
			}
			
			if(game.state == Game.State.TRANSFORMATION) {
				TransformationManager tm = game.GetComponent<TransformationManager>();
				tm.OnLimit();
			}
        }
    }
}
