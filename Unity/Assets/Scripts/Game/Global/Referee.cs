using UnityEngine;
using System.Collections.Generic;

/**
  * @class Referee
  * @brief The Referee watch the game and apply rules.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
[AddComponentMenu("Scripts/Game/Referee")]
public class Referee : myMonoBehaviour {
		
	public Game Game {get;set;}
	
	public bool ToucheRemiseAuCentre 	= false;
	public bool TransfoRemiseAuCentre 	= false;

    public float IngameTime { get; set; }
    private float GameTimeDuration;
	private float IntroDelayTime;
    private float TimeEllapsedSinceIntro;
	private bool  TimePaused;

    private float LastTackle = -1;
    public Unit UnitToGiveBallTo { get; set; }
	
	void Start(){
		TimeEllapsedSinceIntro 	= 0;
		IngameTime	 			= 0;
		GameTimeDuration 		= Game.settings.Global.Game.period_time;
		IntroDelayTime			= Game.settings.Global.Game.timeToSleepAfterIntro;
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
        if (interceptTeam.Player != null) interceptTeam.Player.stopMove();
        if (touchTeam.Player != null) touchTeam.Player.stopMove();
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
        bool right = (this.Game.refs.placeHolders.touchPlacement.position.x > 0);

        // Place les unités


        Transform blueTeam, redTeam, rightTeam, leftTeam;
        rightTeam = this.Game.refs.placeHolders.touchPlacement.FindChild("RightTeam");
        leftTeam = this.Game.refs.placeHolders.touchPlacement.FindChild("LeftTeam");

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

        Transform interceptConfiguration = this.Game.refs.placeHolders.touchPlacement.FindChild("InterceptionTeam");
        if (interceptTeam == this.Game.northTeam/*(red)*/)
        {
            interceptConfiguration.transform.position = redTeam.transform.position;
            interceptConfiguration.transform.rotation = redTeam.transform.rotation;
        }
        else
        {
            interceptConfiguration.transform.position = blueTeam.transform.position;
            interceptConfiguration.transform.rotation = blueTeam.transform.rotation;
        }

        interceptTeam.placeUnits(interceptConfiguration, true);

        Transform passConfiguration = this.Game.refs.placeHolders.touchPlacement.FindChild("TouchTeam");
        if (touchTeam == this.Game.northTeam/*(red)*/)
        {
            passConfiguration.transform.position = redTeam.transform.position;
            passConfiguration.transform.rotation = redTeam.transform.rotation;
        }
        else
        {
            passConfiguration.transform.position = blueTeam.transform.position;
            passConfiguration.transform.rotation = blueTeam.transform.rotation;
        }

        touchTeam.placeUnits(passConfiguration, 1, true);

        Transform passUnitPosition = this.Game.refs.placeHolders.touchPlacement.FindChild("TouchPlayer");
        touchTeam.placeUnit(passUnitPosition, 0, true);

       
        Game.Ball.Owner = touchTeam[0];
        Game.refs.managers.camera.setTarget(null);
    }
	
	public void OnTouch(Touche t) {
		if(t == null || t.a == null || t.b == null ){
			return;	
		}		
				
		// Indique que le jeu passe en mode "Touche"			
            
		// Placement dans la scène de la touche.
		Vector3 pos = Vector3.Project(Game.Ball.transform.position - t.a.position, t.b.position - t.a.position) + t.a.position;
        pos.y = 0; // A terre           
			
		if(this.Game.refs.placeHolders.touchPlacement == null) {
			throw new UnityException("I need to know how place the players when a touch occurs");
		}

        bool right = (pos.x > 0);
            			
		if(right) {
			this.Game.refs.placeHolders.touchPlacement.localRotation = Quaternion.Euler(0, -90, 0);
		}
		else {
			this.Game.refs.placeHolders.touchPlacement.localRotation = Quaternion.Euler(0, 90, 0);
		}
			
		this.Game.refs.placeHolders.touchPlacement.position = pos;
						
		Team interceptTeam = Game.Ball.Team;
		Team touchTeam = interceptTeam.opponent;
	
		//launch the event
        Game.OnTouch();

		// Règlage du mini-jeu
        TouchManager tm = this.Game.refs.managers.touch;
			
		// On indique les équipes
		tm.gamerIntercept = interceptTeam.Player;
		tm.gamerTouch = touchTeam.Player;
			
		// On indique si l'un ou l'autre sera fait au pif
		// TODO : patch j2
		tm.randomTouch = (tm.gamerTouch == null || (tm.gamerTouch == Game.northTeam.Player && !Game.northTeam.Player.XboxController.IsConnected));
		tm.randomIntercept = (tm.gamerIntercept == null || (tm.gamerTouch == Game.northTeam.Player && !Game.northTeam.Player.XboxController.IsConnected));
						
		// Fonction à appeller à la fin de la touche
		tm.CallBack = delegate(TouchManager.Result result, int id) {
								
			// Charger le super à la touche
			
			// On donne la balle à la bonne personne
			if(result == TouchManager.Result.INTERCEPTION) {
				Game.Ball.Owner = interceptTeam[id];
				//super
				this.IncreaseSuper(Game.settings.Global.Super.touchInterceptSuperPoints, interceptTeam);
				this.IncreaseSuper(Game.settings.Global.Super.touchLooseSuperPoints, touchTeam); 
			}
			else {
				Game.Ball.Owner = touchTeam[id+1];
				//super
				this.IncreaseSuper(Game.settings.Global.Super.touchWinSuperPoints, touchTeam);
			}
				
			// Indicateur de bouton
			foreach(Unit u in interceptTeam)
				u.buttonIndicator.target.renderer.enabled = false;
				
			foreach(Unit u in touchTeam)
				u.buttonIndicator.target.renderer.enabled = false;
				
			// Retour en jeu
			//Game.state = Game.State.PLAYING;
			interceptTeam.fixUnits = touchTeam.fixUnits = false;	
			if(interceptTeam.Player != null) interceptTeam.Player.enableMove();
			if(touchTeam.Player != null) touchTeam.Player.enableMove();
		};			
			
		tm.enabled = true;
                  
	}

	public void OnScrum() {

       // this.Game.state = Game.State.SCRUM;
        this.Game.Ball.Owner = null;

        ScrumCinematicMovement();
        NowScrum();
	}

    public void NowScrum()
    {
        Renderer bloc = this.Game.refs.gameObjects.ScrumBloc;
        bloc.transform.position = this.Game.Ball.transform.position;

        scrumController sc = this.Game.refs.managers.scrum;
        sc.InitialPosition = this.Game.Ball.transform.position;
        sc.ScrumBloc = bloc.transform;        
        
        this.Game.southTeam.ShowPlayers(false);
        this.Game.northTeam.ShowPlayers(false);
        this.Game.Ball.Model.enabled = false;
        bloc.enabled = true;

        sc.callback = (Team t, Vector3 endPos) =>
        {
            Game.Ball.Owner = t[0];

            this.Game.Ball.Model.enabled = true;
            this.Game.southTeam.ShowPlayers(true);
            this.Game.northTeam.ShowPlayers(true);
            bloc.enabled = false;

            // this.Game.state = Game.State.PLAYING;
        };

        sc.enabled = true;
    }

    public void ScrumCinematicMovement()
    {
        Vector3 pos = this.Game.Ball.transform.position;
        Transform cinematic = this.Game.refs.placeHolders.scrumPlacement.FindChild("CinematicPlacement");
        cinematic.position = new Vector3(pos.x, 0, pos.z);

        Transform red = cinematic.FindChild("RedTeam");
        Transform blue = cinematic.FindChild("BlueTeam");

        this.Game.southTeam.placeUnits(red, false);
        this.Game.northTeam.placeUnits(blue, false);
    }
		
	public void OnTackle(Unit tackler, Unit tackled) {
	
		/*
        if (Game.state != Game.State.PLAYING)
            return;

        if (tackled == null)
        {
            tackler.sm.event_Tackle();
            return;
        }

        this.Game.state = Game.State.TACKLE;
	 	*/


        TackleManager tm = this.Game.refs.managers.tackle;
        if (tm == null)
            throw new UnityException("Game needs a TackleManager !");
        
        if (tackler == null || tackled == null || tackler.Team == tackled.Team)
            throw new UnityException("Error : " + tackler + " cannot tackle " + tackled + " !");

        tm.game = this.Game;
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
					IncreaseSuper(Game.settings.Global.Super.tackleWinSuperPoints, tackler.Team);
                    tackled.sm.event_Tackle();
                    tackler.sm.event_Tackle();
                    break;
            }

            LastTackle = Time.time;

            //this.Game.state = Game.State.PLAYING;
        };

        tm.Tackle();
	}
	
	public void PlacePlayersForTransfo(){
		Game.Ball.transform.position = Game.Ball.Owner.BallPlaceHolderTransformation.transform.position;
		float x = Game.Ball.transform.position.x;
		
		Team t = Game.Ball.Owner.Team;
		
		t.placeUnits(this.Game.refs.placeHolders.conversionPlacement.FindChild("TeamShoot"), 1, true);
		t.placeUnit(this.Game.refs.placeHolders.conversionPlacement.FindChild("ShootPlayer"), 0, true);
		Team.switchPlaces(t[0], Game.Ball.Owner);
		t.opponent.placeUnits(this.Game.refs.placeHolders.conversionPlacement.FindChild("TeamLook"), true);
		 
        Team opponent = Game.Ball.Owner.Team.opponent;
		
		// Joueur face au look At
		Transform butPoint = t.opponent.But.transform.FindChild("Transformation LookAt");
		Game.Ball.Owner.transform.LookAt(butPoint);
	}
	
	public void EnableTransformation(){
        TransformationManager tm = this.Game.refs.managers.conversion;
		tm.enabled = true;
	}
	
	private void PlaceTransfoPlaceholders(){
		Team t = Game.Ball.Owner.Team;
		float x = Game.Ball.transform.position.x;
		
		Transform point = t.opponent.But.transformationPoint;
		point.transform.position = new Vector3(x, 0, point.transform.position.z);
		
		this.Game.refs.placeHolders.conversionPlacement.transform.position = point.position;
		this.Game.refs.placeHolders.conversionPlacement.transform.rotation = point.rotation;
	}
	
	public void OnEssai() {
		/*
		if(Game.state != Game.State.PLAYING) {
			return;	
		}
		*/
			
		Team t = Game.Ball.Owner.Team;
		
		t.fixUnits = t.opponent.fixUnits = true;			
		if(t.Player != null) t.Player.stopMove();
		if(t.opponent.Player != null) t.opponent.Player.stopMove();		
				
		
        t.nbPoints += Game.settings.Global.Game.points_essai;
		Team opponent = Game.Ball.Owner.Team.opponent;
		
		//super for try
		IncreaseSuper(Game.settings.Global.Super.tryWinSuperPoints,t);
		IncreaseSuper(Game.settings.Global.Super.tryLooseSuperPoints,opponent);
		
		TransformationManager tm = this.Game.refs.managers.conversion;

		tm.ball = Game.Ball;
		tm.gamer = t.Player;	
		
		tm.OnLaunch = () => {
			//this.Game.cameraManager.sm.event_TransfoShot();	
		};
		
        // After the transformation is done, according to the result :
		tm.CallBack = delegate(TransformationManager.Result transformed) {			
			
			if(transformed == TransformationManager.Result.TRANSFORMED) {
				MyDebug.Log ("Transformation");
				t.nbPoints += Game.settings.Global.Game.points_transfo;
				
				//transfo super
				IncreaseSuper(Game.settings.Global.Super.conversionWinSuperPoints,t);
			}else{

				//transfo super
				IncreaseSuper(Game.settings.Global.Super.conversionLooseSuperPoints,t);
			}
			IncreaseSuper(Game.settings.Global.Super.conversionOpponentSuperPoints,t.opponent);

            if (TransfoRemiseAuCentre || transformed != TransformationManager.Result.GROUND)
            {
                // Game.cameraManager.gameCamera.ResetRotation();
                //Game.Ball.setPosition(Vector3.zero);
				
				UnitToGiveBallTo = opponent[0];
                //this.StartPlacement();
			}			
            
            //Game.state = Game.State.PLAYING;           

            Timer.AddTimer(3, () => {
                if (t.Player != null) t.Player.enableMove();
                if (t.opponent.Player != null) t.opponent.Player.enableMove();
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
        but.Owner.opponent.nbPoints += this.Game.settings.Global.Game.points_drop;

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
            NewOwner = Game.southTeam[0];
        }
        else
        {
            NewOwner = Game.northTeam[0];
        }
		
		// Remise au centre, donne la balle aux perdants.
		UnitToGiveBallTo = NewOwner;
        this.StartPlacement();
        //this.Game.TimedDisableIA(3);
    }

    public void StartPlacement()
    {	
        Transform t = Game.refs.placeHolders.startPlacement;

        Game.southTeam.placeUnits(t.Find("South"), true);
        Game.northTeam.placeUnits(t.Find("North"), true);	

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

        UpdateTackle();
    }

    public void UpdateTackle()
    {
        if (LastTackle != -1)
        {
            // TODO cte : 2 -> temps pour checker
            if (Time.time - LastTackle > Game.settings.Global.Game.timeToGetOutTackleAreaBeforeScrum)
            {
                LastTackle = -1;
                int right = 0, left = 0;
                for (int i = 0; i < this.Game.Ball.scrumFieldUnits.Count; i++)
                {
                    if (this.Game.Ball.scrumFieldUnits[i].Team == Game.southTeam)
                        right++;
                    else
                        left++;
                }

                // TODO cte : 3 --> nb de joueurs de chaque equipe qui doivent etre dans la zone
                if (right >= Game.settings.Global.Game.minPlayersEachTeamToTriggerScrum && 
                    left >= Game.settings.Global.Game.minPlayersEachTeamToTriggerScrum)
                {
                    Game.OnScrum();
                    //goScrum = true;
                    //
                }
            }
        }
    }
}
