using UnityEngine;
using System.Collections.Generic;

/**
  * @class Arbiter
  * @brief The arbiter watch the game and apply rules.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
[AddComponentMenu("Scripts/Game/Arbiter")]
public class Arbiter : myMonoBehaviour {
		
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
	
	public  Unit  UnitToGiveBallTo;
	
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

       
        Game.Ball.Owner = touchTeam[0];
        Game.cameraManager.setTarget(null);
    }
	
	public void OnTouch(Touche t) {
		if(t == null || t.a == null || t.b == null ){
			return;	
		}		
				
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
	
		//launch the event
        Game.OnTouch();

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
								
			// Charger le super à la touche
			
			// On donne la balle à la bonne personne
			if(result == TouchManager.Result.INTERCEPTION) {
				Game.Ball.Owner = interceptTeam[id];
				//super
				this.IncreaseSuper(Game.settings.super.touchInterceptSuperPoints, interceptTeam);
				this.IncreaseSuper(Game.settings.super.touchLooseSuperPoints, touchTeam); 
			}
			else {
				Game.Ball.Owner = touchTeam[id+1];
				//super
				this.IncreaseSuper(Game.settings.super.touchWinSuperPoints, touchTeam);
			}
				
			// Indicateur de bouton
			foreach(Unit u in interceptTeam)
				u.buttonIndicator.target.renderer.enabled = false;
				
			foreach(Unit u in touchTeam)
				u.buttonIndicator.target.renderer.enabled = false;
				
			// Retour en jeu
			//Game.state = Game.State.PLAYING;
			interceptTeam.fixUnits = touchTeam.fixUnits = false;	
			if(interceptTeam.Player) interceptTeam.Player.enableMove();
			if(touchTeam.Player) touchTeam.Player.enableMove();
		};			
			
		tm.enabled = true;
                  
	}
	
	public void OnScrum() {

       // this.Game.state = Game.State.SCRUM;

		scrumController sc =  this.Game.GetComponent<scrumController>();
		sc.callback = (Team t) => {
			Game.Ball.Owner = t[0];
            //this.Game.state = Game.State.PLAYING;
		};

        sc.enabled = true;
	}
		
	public void OnTackle(Unit tackler, Unit tackled) {
	
		/*
        if (Game.state != Game.State.PLAYING)
            return;

        this.Game.state = Game.State.TACKLE;
	 	*/
	 
	 
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
				
					//super				
					IncreaseSuper(Game.settings.super.tackleWinSuperPoints,tackler.Team);
				
                    tackled.sm.event_Tackle();
                    tackler.sm.event_Tackle();
                    break;
            }

            //this.Game.state = Game.State.PLAYING;
        };

        tm.Tackle();
	}
	
	public void PlacePlayersForTransfo(){
		Game.Ball.transform.position = Game.Ball.Owner.BallPlaceHolderTransformation.transform.position;
		float x = Game.Ball.transform.position.x;
		
		Team t = Game.Ball.Owner.Team;
		
		t.placeUnits(TransfoPlacement.FindChild("TeamShoot"), 1);
		t.placeUnit(TransfoPlacement.FindChild("ShootPlayer"), 0);
		Team.switchPlaces(t[0], Game.Ball.Owner);
		t.opponent.placeUnits(TransfoPlacement.FindChild("TeamLook"));
		 
        Team opponent = Game.Ball.Owner.Team.opponent;
		
		// Joueur face au look At
		Transform butPoint = t.opponent.But.transform.FindChild("Transformation LookAt");
		Game.Ball.Owner.transform.LookAt(butPoint);
	}
	
	public void EnableTransformation(){
		TransformationManager tm = this.Game.GetComponent<TransformationManager>();
		tm.enabled = true;
	}
	
	private void PlaceTransfoPlaceholders(){
		Team t = Game.Ball.Owner.Team;
		float x = Game.Ball.transform.position.x;
		
		Transform point = t.opponent.But.transformationPoint;
		point.transform.position = new Vector3(x, 0, point.transform.position.z);
		
		TransfoPlacement.transform.position = point.position;
		TransfoPlacement.transform.rotation = point.rotation;
	}
	
	public void OnEssai() {
		/*
		if(Game.state != Game.State.PLAYING) {
			return;	
		}
		*/
			
		Team t = Game.Ball.Owner.Team;
		
		t.fixUnits = t.opponent.fixUnits = true;			
		if(t.Player) t.Player.stopMove();
		if(t.opponent.Player) t.opponent.Player.stopMove();		
				
		MyDebug.Log("Essai de la part des " + t.Name + " !");
        t.nbPoints += Game.settings.score.points_essai;
		Team opponent = Game.Ball.Owner.Team.opponent;
		
		//super for try
		IncreaseSuper(Game.settings.super.tryWinSuperPoints,t);
		IncreaseSuper(Game.settings.super.tryLooseSuperPoints,opponent);
		
		TransformationManager tm = this.Game.GetComponent<TransformationManager>();
		tm.ball = Game.Ball;
		tm.gamer = t.Player;	
		
		tm.OnLaunch = () => {
			//this.Game.cameraManager.sm.event_TransfoShot();	
		};
		
        // After the transformation is done, according to the result :
		tm.CallBack = delegate(TransformationManager.Result transformed) {			
			
			if(transformed == TransformationManager.Result.TRANSFORMED) {
				MyDebug.Log ("Transformation");
				t.nbPoints += Game.settings.score.points_transfo;
				
				//transfo super
				IncreaseSuper(Game.settings.super.conversionWinSuperPoints,t);
			}else{

				//transfo super
				IncreaseSuper(Game.settings.super.conversionLooseSuperPoints,t);
			}
			IncreaseSuper(Game.settings.super.conversionOpponentSuperPoints,t.opponent);

            if (TransfoRemiseAuCentre || transformed != TransformationManager.Result.GROUND)
            {
                // Game.cameraManager.gameCamera.ResetRotation();
                //Game.Ball.setPosition(Vector3.zero);
				
				UnitToGiveBallTo = opponent[0];
                //this.StartPlacement();
			}			
            
            //Game.state = Game.State.PLAYING;           

            Timer.AddTimer(3, () => {
                if (t.Player) t.Player.enableMove();
                if (t.opponent.Player) t.opponent.Player.enableMove();
	            t.fixUnits = t.opponent.fixUnits = false;				    
            });
        };
		PlaceTransfoPlaceholders();
		//Game.state = Game.State.TRANSFORMATION;
	}

    public void OnDropTransformed(But but)
    {
		/*
        if (this.Game.state != Game.State.PLAYING)
        {
            return;
        }
        */
       
        // On donne les points
        but.Owner.opponent.nbPoints += this.Game.settings.score.points_drop;

                // A faire en caméra :
        this.StartPlacement();
        this.Game.Ball.Owner = but.Owner[0];

        //this.Game.TimedDisableIA(3);
    }
	
	private void GiveBall(Unit _u){
		Game.Ball.Owner = _u;
	}
	
    public void OnBallOut()
    {              
        
		Unit NewOwner = null;
        
        // Si on est du côté droit
        if (this.Game.Ball.RightSide())
        {
            NewOwner = Game.right[0];
        }
        else
        {
            NewOwner = Game.left[0];
        }
		
		// Remise au centre, donne la balle aux perdants.
		UnitToGiveBallTo = NewOwner;
        this.StartPlacement();
        //this.Game.TimedDisableIA(3);
    }

    public void StartPlacement()
    {	
        Game.right.placeUnits(Game.right.StartPlacement);
        Game.left.placeUnits(Game.left.StartPlacement);
		//Debug.Log("Unit to give : "+UnitToGiveBallTo);
		GiveBall(UnitToGiveBallTo);
    }

	public void PauseIngameTime(){
		TimePaused = true;
	}
	
	public void ResumeIngameTime(){
		TimePaused = false;
	}
	
 	public void IncreaseSuper(int amount, Team _t){
		_t.increaseSuperGauge(amount);
	}
	
	//plus d'update pour le monsieur
	
    public void Update()
    {
		
        //if (this.Game.state != Game.State.INTRODUCTION && this.Game.state != Game.State.END){
		
		//if(this.Game.sm.st
       		TimeEllapsedSinceIntro += Time.deltaTime;
			if(TimeEllapsedSinceIntro > IntroDelayTime){			
				if(TimePaused == false)IngameTime += Time.deltaTime;
				if(IngameTime > GameTimeDuration){
					IngameTime = GameTimeDuration;
					this.Game.OnGameEnd();
				}
			}
        //}
    }
}
