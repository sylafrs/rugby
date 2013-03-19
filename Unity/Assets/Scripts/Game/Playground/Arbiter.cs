using UnityEngine;
using System.Collections.Generic;

/**
  * @class Arbiter
  * @brief The arbiter watch the game and apply rules.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
[AddComponentMenu("Scripts/Game/Arbiter")]
public class Arbiter : MonoBehaviour {
		
	public Game Game {get;set;}
	
	public bool ToucheRemiseAuCentre = false;
	public Transform TouchPlacement = null;
	public Transform TransfoPlacement = null;	
	
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
			if(interceptTeam.Player) interceptTeam.Player.stopMove();
			if(touchTeam.Player) touchTeam.Player.stopMove();
						
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
			
			// Switch de caméra
			Game.cameraManager.gameCamera.gameObject.SetActive(false);
			Game.cameraManager.touchCamera.gameObject.SetActive(true);
			
			// Placement de la caméra
			Transform cameraPlaceHolder = TouchPlacement.FindChild("CameraPlaceHolder");
			Game.cameraManager.touchCamera.transform.position = cameraPlaceHolder.position;
			Game.cameraManager.touchCamera.transform.rotation = cameraPlaceHolder.rotation;
			
			// Règlage du mini-jeu
			TouchManager tm = this.Game.GetComponent<TouchManager>();
			
			// On indique les équipes
			tm.gamerIntercept = interceptTeam.Player;
			tm.gamerTouch = touchTeam.Player;
			
			// On indique si l'un ou l'autre sera fait au pif
			tm.randomTouch = (tm.gamerTouch == null);
			tm.randomIntercept = (tm.gamerIntercept == null);
						
			// Fonction à appeller à la fin de la touche
			tm.CallBack = delegate(TouchManager.Result result, int id) {
				
				// On remet la caméra à sa rotation d'origine
				Game.cameraManager.gameCamera.ResetRotation();
								
				if(result == TouchManager.Result.INTERCEPTION)
					Game.Ball.Owner = interceptTeam[id];
				else
					Game.Ball.Owner = touchTeam[id+1];
				
				if(Game.Ball.Owner.Team == Game.left) {
					Game.cameraManager.gameCamera.transform.RotateAround(new Vector3(0, 1, 0), Mathf.Deg2Rad * 180);	
				}
								
				interceptTeam[0].buttonIndicator.target.renderer.enabled = false;
				interceptTeam[1].buttonIndicator.target.renderer.enabled = false;
				interceptTeam[2].buttonIndicator.target.renderer.enabled = false;
				
				touchTeam[1].buttonIndicator.target.renderer.enabled = false;
				touchTeam[2].buttonIndicator.target.renderer.enabled = false;
				touchTeam[3].buttonIndicator.target.renderer.enabled = false;
				
				Game.cameraManager.gameCamera.gameObject.SetActive(true);
				Game.cameraManager.touchCamera.gameObject.SetActive(false);
				
				Game.state = Game.State.PLAYING;
				interceptTeam.fixUnits = touchTeam.fixUnits = false;	
				if(interceptTeam.Player) interceptTeam.Player.enableMove();
				if(touchTeam.Player) touchTeam.Player.enableMove();
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
	
	public void OnEssai() {
		if(Game.state != Game.State.PLAYING) {
			return;	
		}	
		
		Team t = Game.Ball.Owner.Team;
		
		t.fixUnits = t.opponent.fixUnits = true;			
		if(t.Player) t.Player.stopMove();
		if(t.opponent.Player) t.opponent.Player.stopMove();		
				
		Debug.Log("Essai de la part des " + t.Name + " !");
        t.nbPoints += Game.settings.score.points_essai;
        			
		Game.state = Game.State.TRANSFORMATION;
		
		Transform point = t.opponent.But.transformationPoint;
		TransfoPlacement.transform.position = point.position;
		TransfoPlacement.transform.rotation = point.rotation;
				
		t.placeUnits(TransfoPlacement.FindChild("TeamShoot"), 1);
		t.placeUnit(TransfoPlacement.FindChild("ShootPlayer"), 0);
		Team.switchPlaces(t[0], Game.Ball.Owner);
		t.opponent.placeUnits(TransfoPlacement.FindChild("TeamLook"));
		
		// Switch de caméra
		Game.cameraManager.gameCamera.gameObject.SetActive(false);
		Game.cameraManager.transfoCamera.gameObject.SetActive(true);
		
		Transform cameraPlaceHolder = TransfoPlacement.FindChild("CameraPlaceHolder");
		Game.cameraManager.transfoCamera.transform.position = cameraPlaceHolder.position;
		Game.cameraManager.transfoCamera.transform.rotation = cameraPlaceHolder.rotation;
		
		TransformationManager tm = this.Game.GetComponent<TransformationManager>();
		tm.gamer = t.Player;
		tm.CallBack = delegate(bool transformed) {						
			Game.cameraManager.gameCamera.ResetRotation();
			Game.Ball.setPosition(Vector3.zero);

       		Game.right.initPos();
        	Game.left.initPos();
			
			Game.cameraManager.gameCamera.gameObject.SetActive(true);
			Game.cameraManager.transfoCamera.gameObject.SetActive(false);
			
			Game.state = Game.State.PLAYING;
			t.fixUnits = t.opponent.fixUnits = false;	
			if(t.Player) t.Player.enableMove();
			if(t.opponent.Player) t.opponent.Player.enableMove();
		};
		
		
		tm.enabled = true;
		
		
	}
}
