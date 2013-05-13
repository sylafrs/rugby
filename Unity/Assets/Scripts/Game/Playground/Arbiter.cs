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
	
	public bool ToucheRemiseAuCentre 	= false;
	public bool TransfoRemiseAuCentre 	= false;
	public Transform TouchPlacement 	= null;
	public Transform TransfoPlacement 	= null;
	
	
	public  float IngameTime;
    private float GameTimeDuration;
	private float IntroDelayTime;
    private float TimeEllapsedSinceIntro;
	private bool  TimePaused;
	
	
	void Start(){
		TimeEllapsedSinceIntro 	= 0;
		IngameTime	 			= 0;
		GameTimeDuration 		= Game.settings.score.period_time;
		IntroDelayTime			= Game.settings.timeToSleepAfterIntro;
		PauseIngameTime();
	}
	
	//when the game start after intro
    public void OnStart()
    {
        ResumeIngameTime();
    }
	
	//public Action OnFadingTouchCamera = null;

    public void PlacePlayersForTouch()
    {
        Team interceptTeam = Game.Ball.Team;
        Team touchTeam = interceptTeam.opponent;

        // Fixe les unités			
        if (interceptTeam.Player) interceptTeam.Player.stopMove();
        if (touchTeam.Player) touchTeam.Player.stopMove();
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

        // Touche à droite ?
        bool right = (TouchPlacement.position.x > 0);

        // Place les unités


        Transform blueTeam, redTeam, rightTeam, leftTeam;
        rightTeam = TouchPlacement.FindChild("RightTeam");
        leftTeam = TouchPlacement.FindChild("LeftTeam");

        if (right)
        {
            redTeam = rightTeam;
            blueTeam = leftTeam;
        }
        else
        {
            redTeam = leftTeam;
            blueTeam = rightTeam;
        }

        Transform interceptConfiguration = TouchPlacement.FindChild("InterceptionTeam");
        if (interceptTeam == this.Game.left/*(red)*/)
        {
            interceptConfiguration.transform.position = redTeam.transform.position;
            interceptConfiguration.transform.rotation = redTeam.transform.rotation;
        }
        else
        {
            interceptConfiguration.transform.position = blueTeam.transform.position;
            interceptConfiguration.transform.rotation = blueTeam.transform.rotation;
        }

        interceptTeam.placeUnits(interceptConfiguration);

        Transform passConfiguration = TouchPlacement.FindChild("TouchTeam");
        if (touchTeam == this.Game.left/*(red)*/)
        {
            passConfiguration.transform.position = redTeam.transform.position;
            passConfiguration.transform.rotation = redTeam.transform.rotation;
        }
        else
        {
            passConfiguration.transform.position = blueTeam.transform.position;
            passConfiguration.transform.rotation = blueTeam.transform.rotation;
        }

        touchTeam.placeUnits(passConfiguration, 1);

        Transform passUnitPosition = TouchPlacement.FindChild("TouchPlayer");
        touchTeam.placeUnit(passUnitPosition, 0);

        Game.cameraManager.CancelNextFlip = true;
        Game.Ball.Owner = touchTeam[0];
        Game.cameraManager.setTarget(null);
    }
	
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
            
			// Placement dans la scène de la touche.
			Vector3 pos = Vector3.Project(Game.Ball.transform.position - t.a.position, t.b.position - t.a.position) + t.a.position;
            pos.y = 0; // A terre           
			
			if(TouchPlacement == null) {
				throw new UnityException("I need to know how place the players when a touch occurs");
			}

            bool right = (pos.x > 0);
            			
			if(right) {
				TouchPlacement.localRotation = Quaternion.Euler(0, -90, 0);
			}
			else {
				TouchPlacement.localRotation = Quaternion.Euler(0, 90, 0);
			}
			
			TouchPlacement.position = pos;
						
			Team interceptTeam = Game.Ball.Team;
			Team touchTeam = interceptTeam.opponent;

            //PlacePlayersForTouch();			
            Game.state = Game.State.TOUCH;		
			
			//Game.Ball.Owner = touchTeam[0];			        

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
								
				// On donne la balle à la bonne personne
				if(result == TouchManager.Result.INTERCEPTION) {
					Game.Ball.Owner = interceptTeam[id];
				}
				else {
					Game.Ball.Owner = touchTeam[id+1];
				}
				
				// Indicateur de bouton
				foreach(Unit u in interceptTeam)
					u.buttonIndicator.target.renderer.enabled = false;
				
				foreach(Unit u in touchTeam)
					u.buttonIndicator.target.renderer.enabled = false;
				
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

        this.Game.state = Game.State.SCRUM;

		scrumController sc =  this.Game.GetComponent<scrumController>();
		sc.callback = (Team t) => {
			Game.Ball.Owner = t[0];
            this.Game.state = Game.State.PLAYING;
		};

        sc.enabled = true;
	}
		
	public void OnTackle(Unit tackler, Unit tackled) {

        if (Game.state != Game.State.PLAYING)        
            return;

        this.Game.state = Game.State.TACKLE;

        TackleManager tm = this.Game.GetComponent<TackleManager>();
        if (tm == null)
            throw new UnityException("Game needs a TackleManager !");
        
        if (tackler == null || tackled == null || tackler.Team == tackled.Team)
            throw new UnityException("Error : " + tackler + " cannot tackle " + tackled + " !");

        tm.tackler = tackler;
        tm.tackled = tackled;

        // End of a tackle, according to the result
        tm.callback = (TackleManager.RESULT res) =>
        {
            switch (res)
            {
                // Plaquage critique, le plaqueur recupère la balle, le plaqué est knockout
                case TackleManager.RESULT.CRITIC:
                    this.Game.Ball.Owner = tackler;
                    break;

                // Passe : les deux sont knock-out mais la balle a pu être donnée à un allié
                case TackleManager.RESULT.PASS:
                    Unit target = tackled.GetNearestAlly();
                    Game.Ball.Pass(target);
                    
                    tackled.sm.event_Tackle();
                    tackler.sm.event_Tackle();
                    break;

                // Normal : les deux sont knock-out et la balle est par terre 
                // /!\ Mêlée possible /!\
                case TackleManager.RESULT.NORMAL:
                    tackled.sm.event_Tackle();
                    tackler.sm.event_Tackle();
                    break;
            }

            this.Game.state = Game.State.PLAYING;
        };

        tm.Tackle();
	}
	
	public void OnEssai() {
		if(Game.state != Game.State.PLAYING) {
			return;	
		}	
		
		Game.Ball.transform.position = Game.Ball.Owner.BallPlaceHolderTransformation.transform.position;
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

        Team opponent = Game.Ball.Owner.Team.opponent;
				
		TransformationManager tm = this.Game.GetComponent<TransformationManager>();
		tm.ball = Game.Ball;
		tm.gamer = t.Player;		
		
        // After the transformation is done, according to the result :
		tm.CallBack = delegate(TransformationManager.Result transformed) {			
			
			if(transformed == TransformationManager.Result.TRANSFORMED) {
				Debug.Log ("Transformation");
				t.nbPoints += Game.settings.score.points_transfo;
			}

            if (TransfoRemiseAuCentre || transformed != TransformationManager.Result.GROUND)
            {
                // Game.cameraManager.gameCamera.ResetRotation();
                //Game.Ball.setPosition(Vector3.zero);

                Game.right.initPos();
                Game.left.initPos();
                Game.Ball.Owner = opponent[0];
            
			}			
            
            Game.state = Game.State.PLAYING;
			t.fixUnits = t.opponent.fixUnits = false;	
			if(t.Player) t.Player.enableMove();
			if(t.opponent.Player) t.opponent.Player.enableMove();
		};
				
		tm.enabled = true;
	}

    public void OnBut()
    {
        /*
	        Debug.Log(Owner.name + " viens de se prendre un but dans sa face");
	        Owner.nbPoints += b.Game.settings.score.points_drop;
	        b.setPosition(Vector3.zero);
        */
    }

	public void PauseIngameTime(){
		TimePaused = true;
	}
	
	public void ResumeIngameTime(){
		TimePaused = false;
	}
	
    public void Update()
    {
        if (this.Game.state != Game.State.INTRODUCTION && this.Game.state != Game.State.END){
       		TimeEllapsedSinceIntro += Time.deltaTime;
			if(TimeEllapsedSinceIntro > IntroDelayTime){
				if(TimePaused == false)IngameTime += Time.deltaTime;
				if(IngameTime > GameTimeDuration){
					IngameTime = GameTimeDuration;
                	this.Game.state = Game.State.END;
				}
			}
        }
    }
}
