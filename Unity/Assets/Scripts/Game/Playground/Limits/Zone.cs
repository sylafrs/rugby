using UnityEngine;
using System.Collections;

/**
 * @class Game
 * @brief Trigger des zones d'essai
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Zone")]
public class Zone : TriggeringTrigger {

    private Team owner;
    public Team Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }

    public override void Inside(Triggered o)
    {
        Ball b = o.GetComponent<Ball>();
        if (b != null)
        {
            if (b.Owner != null)
            {
                if (b.Owner.Team != this.Owner)
                {                   
                    if (b.Owner.Team.Player == null)
					{				
                        b.Game.OnEssai();
					}
                }
            }
        }
    }
	
	public override void Entered (Triggered o)
	{
		Ball b = o.GetComponent<Ball>();
        if (b != null)
        {
			b.inZone = this;
            if (b.Owner != null)
            {
                if (b.Owner.Team != this.Owner)
                {                   
                    if (b.Owner.Team.Player != null) {
						b.Owner.buttonIndicator.ApplyTexture("A");
						b.Owner.buttonIndicator.target.renderer.enabled = true;
					}
				}
			}
		}
	}
	
	public override void Left (Triggered o)
	{
		Ball b = o.GetComponent<Ball>();
        if (b != null)
        {
			b.inZone = null;
            if (b.Owner != null)
            {
                if (b.Owner.Team != this.Owner)
                {                   
                    if (b.Owner.Team.Player != null) {
						b.Owner.buttonIndicator.target.renderer.enabled = false;							
					}
				}
			}
			else if(b.PreviousOwner != null) {
				if (b.PreviousOwner.Team != this.Owner)
                {                   
                    if (b.PreviousOwner.Team.Player != null) {
						b.PreviousOwner.buttonIndicator.target.renderer.enabled = false;							
					}
				}				
			}
		}
	}

}
