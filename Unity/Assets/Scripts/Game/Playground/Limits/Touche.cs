using UnityEngine;
using System.Collections;

/**
 * @class Game
 * @brief Trigger des lignes de touche
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Game/Touche")]
public class Touche : TriggeringTrigger
{
    public Transform a, b;
    
    public override void Entered(Triggered t)
    {
        Ball ball = t.GetComponent<Ball>();
        if (ball != null)
        {
            Game.instance.Referee.OnTouch(this);
        
			//ball.inTouch = this;
			
            //if (ball.Game.state == Game.State.PLAYING)
            //{
                //this.gameObject.SendMessageUpwards("OnTouch", this, SendMessageOptions.DontRequireReceiver);
            //}
			/*
            else if(ball.Game.state == Game.State.TRANSFORMATION)
            {
                TransformationManager tm = ball.Game.GetComponent<TransformationManager>();
                if (tm)
                    tm.OnLimit();   
                else
                    Debug.LogWarning("Error : I dont find Transformation Manager in the Game");
            }
            */
        }
    }
	
	/*public override void Left (Triggered o)
	{
		Ball b = o.GetComponent<Ball>();
        if (b != null)
        {
			b.inTouch = null;          
		}
	}*/
	
    void OnDrawGizmos()
    {
        if (a == null || b == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(a.position, b.position);
    }
}