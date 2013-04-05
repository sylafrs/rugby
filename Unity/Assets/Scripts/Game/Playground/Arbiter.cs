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
	public bool TransfoRemiseAuCentre = false;
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
			
			// Fixe les unités			
			if(interceptTeam.Player) interceptTeam.Player.stopMove();
			if(touchTeam.Player) touchTeam.Player.stopMove();
			interceptTeam.fixUnits = touchTeam.fixUnits = true;					
									
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
			
			// Place les unités
			Transform interceptConfiguration = TouchPlacement.FindChild("InterceptionTeam");
			interceptTeam.placeUnits(interceptConfiguration);
			
			Transform passConfiguration = TouchPlacement.FindChild("TouchTeam");
			touchTeam.placeUnits(passConfiguration, 1);
			
			Transform passUnitPosition = TouchPlacement.FindChild("TouchPlayer");
			touchTeam.placeUnit(passUnitPosition, 0);
			
			Game.Ball.Owner = touchTeam[0];
			
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
			// TODO : patch j2
			tm.randomTouch = (tm.gamerTouch == null || (tm.gamerTouch == Game.p2 && !Game.p2.XboxController.IsConnected));
			tm.randomIntercept = (tm.gamerIntercept == null || (tm.gamerTouch == Game.p2 && !Game.p2.XboxController.IsConnected));
						
			// Fonction à appeller à la fin de la touche
			tm.CallBack = delegate(TouchManager.Result result, int id) {
				
				// On remet la caméra à sa rotation d'origine
				Game.cameraManager.gameCamera.ResetRotation();
								
				// On donne la balle à la bonne personne
				if(result == TouchManager.Result.INTERCEPTION) {
					Game.Ball.Owner = interceptTeam[id];
				}
				else {
					Game.Ball.Owner = touchTeam[id+1];
				}
				
				// Caméra bien orientée
				if(Game.Ball.Owner.Team == Game.left) {
					Game.cameraManager.gameCamera.transform.RotateAround(new Vector3(0, 1, 0), Mathf.Deg2Rad * 180);	
				}
								
				// Indicateur de bouton
				interceptTeam[0].buttonIndicator.target.renderer.enabled = false;
				interceptTeam[1].buttonIndicator.target.renderer.enabled = false;
				interceptTeam[2].buttonIndicator.target.renderer.enabled = false;				
				touchTeam[1].buttonIndicator.target.renderer.enabled = false;
				touchTeam[2].buttonIndicator.target.renderer.enabled = false;
				touchTeam[3].buttonIndicator.target.renderer.enabled = false;
				
				// Caméra
				Game.cameraManager.gameCamera.gameObject.SetActive(true);
				Game.cameraManager.touchCamera.gameObject.SetActive(false);
				
				// Retour en jeu
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
		
	public void OnTackle() {
		
	}
	
	public void OnEssai() {
		if(Game.state != Game.State.PLAYING) {
			return;	
		}	
		
		float x = Game.Ball.transform.position.x;
		
		Team t = Game.Ball.Owner.Team;
		
		t.fixUnits = t.opponent.fixUnits = true;			
		if(t.Player) t.Player.stopMove();
		if(t.opponent.Player) t.opponent.Player.stopMove();		
				
		Debug.Log("Essai de la part des " + t.Name + " !");
        t.nbPoints += Game.settings.score.points_essai;
		        			
		Game.state = Game.State.TRANSFORMATION;
				
		Transform point = t.opponent.But.transformationPoint;
		point.transform.position = new Vector3(x, 0, point.transform.position.z);
		
		TransfoPlacement.transform.position = point.position;
		TransfoPlacement.transform.rotation = point.rotation;
				
		t.placeUnits(TransfoPlacement.FindChild("TeamShoot"), 1);
		t.placeUnit(TransfoPlacement.FindChild("ShootPlayer"), 0);
		Team.switchPlaces(t[0], Game.Ball.Owner);
		t.opponent.placeUnits(TransfoPlacement.FindChild("TeamLook"));
		
		// Switch/Position de caméra
		Transform butPoint = t.opponent.But.transform.FindChild("Transformation LookAt");
		Transform cameraPlaceHolder = TransfoPlacement.FindChild("CameraPlaceHolder");
			
		///////
		
		Vector3 BallCamera = cameraPlaceHolder.transform.position - Game.Ball.Owner.transform.position;
		BallCamera.y = 0;
				
		Vector3 ButBall = Game.Ball.Owner.transform.position - butPoint.transform.position;
		ButBall.y = 0;
		
		Vector3 CameraPos = ButBall.normalized * BallCamera.magnitude + Game.Ball.Owner.transform.position;
		CameraPos.y = cameraPlaceHolder.transform.position.y;
		
		Game.cameraManager.transfoCamera.transform.position = CameraPos;
		
		///////
		
		Game.cameraManager.gameCamera.gameObject.SetActive(false);
		Game.cameraManager.transfoCamera.gameObject.SetActive(true);		
		
		Game.cameraManager.transfoCamera.transform.LookAt(butPoint);
		
		TransformationManager tm = this.Game.GetComponent<TransformationManager>();
		tm.ball = Game.Ball;
		tm.gamer = t.Player;		
		
		tm.CallBack = delegate(TransformationManager.Result transformed) {			
			
			if(transformed == TransformationManager.Result.TRANSFORMED) {
				Debug.Log ("Transformation");
				t.nbPoints += Game.settings.score.points_transfo;
			}

            if (TransfoRemiseAuCentre || transformed != TransformationManager.Result.GROUND)
            {
                Game.cameraManager.gameCamera.ResetRotation();
                Game.Ball.setPosition(Vector3.zero);
                Game.right.initPos();
                Game.left.initPos();
            }
          
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
