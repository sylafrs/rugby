using UnityEngine;
using System.Collections.Generic;

/**
  * @class Referee
  * @brief The Referee watch the game and apply rules.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
[AddComponentMenu("Scripts/Game/Referee")]
public partial class Referee : myMonoBehaviour {
		
	public Game game {get;set;}

    public float IngameTime { get; set; }
    private float GameTimeDuration;
	private float IntroDelayTime;
    private float TimeEllapsedSinceIntro;
	private bool  TimePaused;

    private float LastTackle = -1;
    public Unit UnitToGiveBallTo { get; set; }
	
	void Start(){

        if (!game)
            game = Game.instance;

		TimeEllapsedSinceIntro 	= 0;
		IngameTime	 			= 0;
		GameTimeDuration 		= game.settings.Global.Game.period_time;
		IntroDelayTime			= game.settings.GameStates.MainState.IntroState.timeToSleepAfterIntro;
		PauseIngameTime();
	}
	
	//when the game start after intro
    public void OnStart()
    {
        ResumeIngameTime();
    }
	
	//public Action OnFadingTouchCamera = null;

	private void GiveBall(Unit _u){
		game.Ball.Owner = _u;
	}
	
    public void OnBallOut()
    {           
        
		Unit NewOwner = null;
        
        // Si on est du côté droit
        if (this.game.Ball.RightSide())
        {
            NewOwner = game.southTeam[2];
        }
        else
        {
            NewOwner = game.northTeam[2];
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

        this.UpdateTackle(); // Referee_tackle.cs
    }
}