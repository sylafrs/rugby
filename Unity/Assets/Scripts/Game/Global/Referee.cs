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
		
	public Game game {get;set;}
	
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
		GameTimeDuration 		= game.settings.Global.Game.period_time;
		IntroDelayTime			= game.settings.Global.Game.timeToSleepAfterIntro;
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
        Team interceptTeam = game.Ball.Team;
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
        bool right = (this.game.refs.placeHolders.touchPlacement.position.x > 0);

        // Place les unités


        Transform blueTeam, redTeam, rightTeam, leftTeam;
        rightTeam = this.game.refs.placeHolders.touchPlacement.FindChild("RightTeam");
        leftTeam = this.game.refs.placeHolders.touchPlacement.FindChild("LeftTeam");

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

        Transform interceptConfiguration = this.game.refs.placeHolders.touchPlacement.FindChild("InterceptionTeam");
        if (interceptTeam == this.game.northTeam/*(red)*/)
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

        Transform passConfiguration = this.game.refs.placeHolders.touchPlacement.FindChild("TouchTeam");
        if (touchTeam == this.game.northTeam/*(red)*/)
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

        Transform passUnitPosition = this.game.refs.placeHolders.touchPlacement.FindChild("TouchPlayer");
        touchTeam.placeUnit(passUnitPosition, 0, true);

       
        game.Ball.Owner = touchTeam[0];
        game.refs.managers.camera.setTarget(null);
    }
	
	public void OnTouch(Touche t) {
		if(t == null || t.a == null || t.b == null ){
			return;	
		}		
				
		// Indique que le jeu passe en mode "Touche"			
            
		// Placement dans la scène de la touche.
		Vector3 pos = Vector3.Project(game.Ball.transform.position - t.a.position, t.b.position - t.a.position) + t.a.position;
        pos.y = 0; // A terre           
			
		if(this.game.refs.placeHolders.touchPlacement == null) {
			throw new UnityException("I need to know how place the players when a touch occurs");
		}

        bool right = (pos.x > 0);
            			
		if(right) {
			this.game.refs.placeHolders.touchPlacement.localRotation = Quaternion.Euler(0, -90, 0);
		}
		else {
			this.game.refs.placeHolders.touchPlacement.localRotation = Quaternion.Euler(0, 90, 0);
		}
			
		this.game.refs.placeHolders.touchPlacement.position = pos;
						
		Team interceptTeam = game.Ball.Team;
		Team touchTeam = interceptTeam.opponent;
	
		//launch the event
        game.OnTouch();

		// Règlage du mini-jeu
        TouchManager tm = this.game.refs.managers.touch;
			
		// On indique les équipes
		tm.gamerIntercept = interceptTeam.Player;
		tm.gamerTouch = touchTeam.Player;
			
		// On indique si l'un ou l'autre sera fait au pif
		// TODO : patch j2
		tm.randomTouch = (tm.gamerTouch == null || (tm.gamerTouch == game.northTeam.Player && !game.northTeam.Player.XboxController.IsConnected));
		tm.randomIntercept = (tm.gamerIntercept == null || (tm.gamerTouch == game.northTeam.Player && !game.northTeam.Player.XboxController.IsConnected));
						
		// Fonction à appeller à la fin de la touche
		tm.CallBack = delegate(TouchManager.Result result, int id) {
								
			// Charger le super à la touche
			
			// On donne la balle à la bonne personne
			if(result == TouchManager.Result.INTERCEPTION) {
				game.Ball.Owner = interceptTeam[id];
				//super
				this.IncreaseSuper(game.settings.Global.Super.touchInterceptSuperPoints, interceptTeam);
				this.IncreaseSuper(game.settings.Global.Super.touchLooseSuperPoints, touchTeam); 
			}
			else {
				game.Ball.Owner = touchTeam[id+1];
				//super
				this.IncreaseSuper(game.settings.Global.Super.touchWinSuperPoints, touchTeam);
			}
				
			// Indicateur de bouton
			foreach(Unit u in interceptTeam)
				u.buttonIndicator.target.renderer.enabled = false;
				
			foreach(Unit u in touchTeam)
				u.buttonIndicator.target.renderer.enabled = false;
				
			// Retour en jeu
			interceptTeam.fixUnits = touchTeam.fixUnits = false;	
			if(interceptTeam.Player != null) interceptTeam.Player.enableMove();
			if(touchTeam.Player != null) touchTeam.Player.enableMove();
		};			
			
		tm.enabled = true;
                  
	}

	public void OnScrum() {
		
        this.game.Ball.Owner = null;

        ScrumCinematicMovement();
        NowScrum();
	}

    public void NowScrum()
    {
        Renderer bloc = this.game.refs.gameObjects.ScrumBloc;
        bloc.transform.position = this.game.Ball.transform.position;

        scrumController sc = this.game.refs.managers.scrum;
        sc.InitialPosition = this.game.Ball.transform.position;
        sc.ScrumBloc = bloc.transform;        
        
        this.game.southTeam.ShowPlayers(false);
        this.game.northTeam.ShowPlayers(false);
        this.game.Ball.Model.enabled = false;
        bloc.enabled = true;

        sc.callback = (Team t, Vector3 endPos) =>
        {
            game.Ball.Owner = t[0];

            this.game.Ball.Model.enabled = true;
            this.game.southTeam.ShowPlayers(true);
            this.game.northTeam.ShowPlayers(true);
            bloc.enabled = false;
        };

        sc.enabled = true;
    }

    public void ScrumCinematicMovement()
    {
        Vector3 pos = this.game.Ball.transform.position;
        Transform cinematic = this.game.refs.placeHolders.scrumPlacement.FindChild("CinematicPlacement");
        cinematic.position = new Vector3(pos.x, 0, pos.z);

        Transform red = cinematic.FindChild("RedTeam");
        Transform blue = cinematic.FindChild("BlueTeam");

        this.game.southTeam.placeUnits(red, false);
        this.game.northTeam.placeUnits(blue, false);
    }
		
	public void OnTackle(Unit tackler, Unit tackled) {
	
        TackleManager tm = this.game.refs.managers.tackle;
        if (tm == null)
            throw new UnityException("Game needs a TackleManager !");
        
        if (tackler == null || tackled == null || tackler.Team == tackled.Team)
            throw new UnityException("Error : " + tackler + " cannot tackle " + tackled + " !");

        tm.game = this.game;
        tm.tackler = tackler;
        tm.tackled = tackled;

        // End of a tackle, according to the result
        tm.callback = (TackleManager.RESULT res) =>
        {
            switch (res)
            {
                // Plaquage critique, le plaqueur recupère la balle, le plaqué est knockout
                case TackleManager.RESULT.CRITIC:
                    this.game.Ball.Owner = tackler;
                    break;

                // Passe : les deux sont knock-out mais la balle a pu être donnée à un allié
                case TackleManager.RESULT.PASS:
                    Unit target = tackled.GetNearestAlly();
                    game.Ball.Pass(target);
                    
                    tackled.sm.event_Tackle();
                    tackler.sm.event_Tackle();
                    break;

                // Normal : les deux sont knock-out et la balle est par terre 
                // /!\ Mêlée possible /!\
                case TackleManager.RESULT.NORMAL:
				
					//super				
					IncreaseSuper(game.settings.Global.Super.tackleWinSuperPoints, tackler.Team);
                    tackled.sm.event_Tackle();
                    tackler.sm.event_Tackle();
                    break;
            }

            LastTackle = Time.time;
        };

        tm.Tackle();
	}
	
	public void PlacePlayersForTransfo(){
		game.Ball.transform.position = game.Ball.Owner.BallPlaceHolderTransformation.transform.position;
		float x = game.Ball.transform.position.x;
		
		Team t = game.Ball.Owner.Team;
		
		t.placeUnits(this.game.refs.placeHolders.conversionPlacement.FindChild("TeamShoot"), 1, true);
		t.placeUnit(this.game.refs.placeHolders.conversionPlacement.FindChild("ShootPlayer"), 0, true);
		Team.switchPlaces(t[0], game.Ball.Owner);
		t.opponent.placeUnits(this.game.refs.placeHolders.conversionPlacement.FindChild("TeamLook"), true);
		 
        Team opponent = game.Ball.Owner.Team.opponent;
		
		// Joueur face au look At
		Transform butPoint = t.opponent.But.transform.FindChild("Transformation LookAt");
		game.Ball.Owner.transform.LookAt(butPoint);
	}
	
	public void EnableTransformation(){
        TransformationManager tm = this.game.refs.managers.conversion;
		tm.enabled = true;
	}
	
	private void PlaceTransfoPlaceholders(){
		Team t = game.Ball.Owner.Team;
		float x = game.Ball.transform.position.x;
		
		Transform point = t.opponent.But.transformationPoint;
		point.transform.position = new Vector3(x, 0, point.transform.position.z);
		
		this.game.refs.placeHolders.conversionPlacement.transform.position = point.position;
		this.game.refs.placeHolders.conversionPlacement.transform.rotation = point.rotation;
	}
	
	public void OnEssai() {
		
			
		Team t = game.Ball.Owner.Team;
		
		t.fixUnits = t.opponent.fixUnits = true;			
		if(t.Player != null) t.Player.stopMove();
		if(t.opponent.Player != null) t.opponent.Player.stopMove();		
				
		
        t.nbPoints += game.settings.Global.Game.points_essai;
		Team opponent = game.Ball.Owner.Team.opponent;
		
		//super for try
		IncreaseSuper(game.settings.Global.Super.tryWinSuperPoints,t);
		IncreaseSuper(game.settings.Global.Super.tryLooseSuperPoints,opponent);
		
		TransformationManager tm = this.game.refs.managers.conversion;

		tm.ball = game.Ball;
		tm.gamer = t.Player;	
		
		tm.OnLaunch = () => {
			//this.game.cameraManager.sm.event_TransfoShot();	
		};
		
        // After the transformation is done, according to the result :
		tm.CallBack = delegate(TransformationManager.Result transformed) {			
			
			if(transformed == TransformationManager.Result.TRANSFORMED) {
				MyDebug.Log ("Transformation");
				t.nbPoints += game.settings.Global.Game.points_transfo;
				
				//transfo super
				IncreaseSuper(game.settings.Global.Super.conversionWinSuperPoints,t);
			}else{

				//transfo super
				IncreaseSuper(game.settings.Global.Super.conversionLooseSuperPoints,t);
			}
			IncreaseSuper(game.settings.Global.Super.conversionOpponentSuperPoints,t.opponent);

            if (TransfoRemiseAuCentre || transformed != TransformationManager.Result.GROUND)
            {
               
                //game.Ball.setPosition(Vector3.zero);
				
				UnitToGiveBallTo = opponent[0];
                //this.StartPlacement();
			}			
                  

            Timer.AddTimer(3, () => {
                if (t.Player != null) t.Player.enableMove();
                if (t.opponent.Player != null) t.opponent.Player.enableMove();
	            t.fixUnits = t.opponent.fixUnits = false;				    
            });
        };
		PlaceTransfoPlaceholders();
	}

    public void OnDropTransformed(But but)
    {
       
        // On donne les points
        but.Owner.opponent.nbPoints += this.game.settings.Global.Game.points_drop;

        // A faire en caméra :
        this.StartPlacement();
        this.game.Ball.Owner = but.Owner[0];

        //this.game.TimedDisableIA(3);
    }
	
	private void GiveBall(Unit _u){
		game.Ball.Owner = _u;
	}
	
    public void OnBallOut()
    {              
        
		Unit NewOwner = null;
        
        // Si on est du côté droit
        if (this.game.Ball.RightSide())
        {
            NewOwner = game.southTeam[0];
        }
        else
        {
            NewOwner = game.northTeam[0];
        }
		
		// Remise au centre, donne la balle aux perdants.
		UnitToGiveBallTo = NewOwner;
        this.StartPlacement();
        //this.game.TimedDisableIA(3);
    }

    public void StartPlacement()
    {	
        Transform t = game.refs.placeHolders.startPlacement;

        game.southTeam.placeUnits(t.Find("South"), true);
        game.northTeam.placeUnits(t.Find("North"), true);	

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
		
		
		//if(this.game.sm.st
       		TimeEllapsedSinceIntro += Time.deltaTime;
			if(TimeEllapsedSinceIntro > IntroDelayTime){			
				if(TimePaused == false)IngameTime += Time.deltaTime;
				if(IngameTime > GameTimeDuration){
					IngameTime = GameTimeDuration;
					this.game.OnGameEnd();
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
            if (Time.time - LastTackle > game.settings.Global.Game.timeToGetOutTackleAreaBeforeScrum)
            {
                LastTackle = -1;
                int right = 0, left = 0;
                for (int i = 0; i < this.game.Ball.scrumFieldUnits.Count; i++)
                {
                    if (this.game.Ball.scrumFieldUnits[i].Team == game.southTeam)
                        right++;
                    else
                        left++;
                }

                // TODO cte : 3 --> nb de joueurs de chaque equipe qui doivent etre dans la zone
                if (right >= game.settings.Global.Game.minPlayersEachTeamToTriggerScrum && 
                    left >= game.settings.Global.Game.minPlayersEachTeamToTriggerScrum)
                {
                    game.OnScrum();
                    //goScrum = true;
                    //
                }
            }
        }
    }
}
