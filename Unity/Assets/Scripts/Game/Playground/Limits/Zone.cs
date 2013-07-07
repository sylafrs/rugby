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
        if (b != null && b.Owner != null && b.Owner.Team != this.Owner){
            if (b.Owner.Team.Player == null){				
                b.Game.OnTry(this);
			}
			else{
				foreach(Unit u in b.Owner.Team) {
					u.HideButton();
				}
				
				if(!b.Owner.ButtonVisible) {
					b.Owner.ShowButton("A");
				}								
			}
			if(b.Owner.isInTryZone == false){
				b.Owner.HideButton();
			}
        }
    }
	
	public override void Entered (Triggered o)
	{
		//Debug.Log("Entrée dans la zone de "+o.name);
		Ball b = o.GetComponent<Ball>();
		
		if(o.GetType() == typeof(Unit)){
			Unit u = (Unit)o;
			u.isInTryZone = true;		
			//u.ShowButton("A");	
		}
		
        if (b != null)
        {
			if(b.Owner != null){
				b.Owner.ShowButton("A");
				b.inZone = this; 
			}
		}
	}
	
	void Update() {
		
		if(owner.opponent.Player.Controlled && owner.opponent.Player.Controlled == owner.game.Ball.Owner) {
			if(owner == owner.game.southTeam)
			{
				if(owner.But.transform.position.z <= owner.game.Ball.transform.position.z){
					owner.game.Ball.Owner.HideButton();
				}
			}
			else 
			{
				if(owner.But.transform.position.z >= owner.game.Ball.transform.position.z){
					owner.game.Ball.Owner.HideButton();
				}
			}
		}
		
		/*
		if (
		(this.game.Ball.Owner.Team == this.game.southTeam && this.game.Ball.transform.position.z >= this.game.northTeam.But.transform.position.z )
		||
		(this.game.Ball.Owner.Team == this.game.northTeam && this.game.Ball.transform.position.z <= this.game.southTeam.But.transform.position.z )
		)
		{
			 ;
		}
		*/
	}
	
	public override void Left (Triggered o)
	{
		//Debug.Log("Sortie de la zone "+this.name+" par "+o.name);
		Ball b = o.GetComponent<Ball>();
		
		if(o.GetType() == typeof(Unit)){
			Unit u = (Unit)o;
			u.isInTryZone = false;
			u.HideButton();
		}
		
        if (b != null)
        {
			b.inZone = null;
			//Debug.Log("ball zone : "+b.inZone);
            foreach(Unit u in b.Team) {
				u.HideButton();
			}
			foreach(Unit u in b.Team.opponent) {
				u.HideButton();
			}
		}
	}
}
