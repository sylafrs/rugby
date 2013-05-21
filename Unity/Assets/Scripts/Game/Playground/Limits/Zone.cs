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
                        b.Game.OnEssai(this);
					}
					else {
						foreach(Unit u in b.Owner.Team) {
							u.HideButton();
						}
						
						if(!b.Owner.ButtonVisible) {
							b.Owner.ShowButton("A");
						}								
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
		}
	}
	
	public override void Left (Triggered o)
	{
		Ball b = o.GetComponent<Ball>();
        if (b != null)
        {
			b.inZone = null;
            foreach(Unit u in b.Team) {
				u.HideButton();
			}
			foreach(Unit u in b.Team.opponent) {
				u.HideButton();
			}
		}
	}

}
