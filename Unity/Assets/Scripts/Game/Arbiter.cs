using UnityEngine;
using System.Collections.Generic;

/**
  * @class Arbiter
  * @brief The arbiter watch the game and apply rules.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class Arbiter : MonoBehaviour {
		
	public Game Game {get;set;}
	
	public bool ToucheRemiseAuCentre = false;
	public Transform TouchPlacement = null;
		
	public void OnTouch(Touche t) {
		if(t == null || Game.state != Game.State.PLAYING) {
			return;	
		}		
		
		if (t.a == null || t.b == null)
        {
            Debug.Log("Touche : [Replace au centre]");
            Game.Ball.setPosition(Vector3.zero);
        }
        else
        {
			// Indique que le jeu passe en mode "Touche"
			Game.state = Game.State.TOUCH;		
            Debug.Log("Touche : [Replace au centre, sur la ligne]");

            
			// Placement dans la scène de la touche.
			Vector3 pos = Vector3.Project(Game.Ball.transform.position - t.a.position, t.b.position - t.a.position) + t.a.position;
            pos.y = 0; // A terre
           
			
			if(TouchPlacement == null) {
				throw new UnityException("I need to know how place the players when a touch occurs");
			}			
			
			if(pos.x > 0) {
				TouchPlacement.localRotation = Quaternion.Euler(0, -90, 0);
			}
			else {
				TouchPlacement.localRotation = Quaternion.Euler(0, 90, 0);
			}
			
			TouchPlacement.position = pos;
			
			Team interceptTeam = Game.Ball.Team;
			Team touchTeam = interceptTeam.opponent;
			
			// Fixe et place les unités (relatif à la touche déjà placée)			
			interceptTeam.fixUnits = touchTeam.fixUnits = true;
			
			Transform interceptConfiguration = TouchPlacement.FindChild("InterceptionTeam");
			interceptTeam.placeUnits(interceptConfiguration);
			
			Transform passConfiguration = TouchPlacement.FindChild("TouchTeam");
			touchTeam.placeUnits(passConfiguration, 1);
			
			Transform passUnitPosition = TouchPlacement.FindChild("TouchPlayer");
			touchTeam.placeUnit(passUnitPosition, 0);
			
			Game.Ball.Owner = touchTeam[0];
			
			// Bouttons pour la touche.			
			interceptTeam[0].buttonIndicator.ApplyTexture("A");
			interceptTeam[1].buttonIndicator.ApplyTexture("B");
			interceptTeam[2].buttonIndicator.ApplyTexture("X");
			
			touchTeam[1].buttonIndicator.ApplyTexture("A");
			touchTeam[2].buttonIndicator.ApplyTexture("B");
			touchTeam[3].buttonIndicator.ApplyTexture("X");
			
			interceptTeam[0].buttonIndicator.target.renderer.enabled = true;
			interceptTeam[1].buttonIndicator.target.renderer.enabled = true;
			interceptTeam[2].buttonIndicator.target.renderer.enabled = true;
			
			touchTeam[1].buttonIndicator.target.renderer.enabled = true;
			touchTeam[2].buttonIndicator.target.renderer.enabled = true;
			touchTeam[3].buttonIndicator.target.renderer.enabled = true;
			
			TouchManager tm = this.Game.GetComponent<TouchManager>();
			
			tm.CallBack = delegate(TouchManager.Result result, int id) {
				if(result == TouchManager.Result.INTERCEPTION)
					Game.Ball.Owner = interceptTeam[id];
				else
					Game.Ball.Owner = touchTeam[id];
				
				interceptTeam[0].buttonIndicator.target.renderer.enabled = false;
				interceptTeam[1].buttonIndicator.target.renderer.enabled = false;
				interceptTeam[2].buttonIndicator.target.renderer.enabled = false;
				
				touchTeam[1].buttonIndicator.target.renderer.enabled = false;
				touchTeam[2].buttonIndicator.target.renderer.enabled = false;
				touchTeam[3].buttonIndicator.target.renderer.enabled = false;
				
				Game.state = Game.State.PLAYING;
				interceptTeam.fixUnits = touchTeam.fixUnits = false;				
			};			
			
			tm.enabled = true;
        }           
	}
	
	public void OnScrum() {
		
	}
	
	public void OnTransformation() {
		
	}
	
	public void OnTackle() {
		
	}
	
	public void OnBut() {
		
	}
}
