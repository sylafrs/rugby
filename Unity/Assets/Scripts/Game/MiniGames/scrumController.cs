using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/*
 *@author Maxens Dubois 
 */
[AddComponentMenu("Scripts/MiniGames/Scrum")]
public class scrumController : myMonoBehaviour {
	
    public Transform ScrumBloc                      { private get; set; }   // Object to move   (parameter)
    public Vector3 InitialPosition                  { private get; set; }   // Unity unit       (parameter)
    public System.Action<Team, Vector3> callback    { private get; set; }   // At the end.      (parameter, optionnal)	

    public  bool ChronoLaunched { get; private set; }                       // First smash      (variable, readonly)
    public  float TimeRemaining { get; private set; }                       // Time to play     (variable, readonly)
    public float currentPosition;                                          // -1 to 1          (variable)
    private int CurrentWinner;                                              // Winner           (variable)
    public float SuperLoading;                                             // 0 to 1           (variable)

    private Game Game;                                                      // Game             (reference)    

    void Start()
    {
        this.Game = Game.instance;
        if (this.Game == null)
        {
            throw new UnityException("[scrumController] : I need a game to work !");
        }
    }
    
    void OnEnable()
    {
        if (this.ScrumBloc == null)
        {
            GameObject o = GameObject.Find("SCRUM");
            if(o == null)
                o = new GameObject("SCRUM");

            this.ScrumBloc = o.transform;
        }

        this.currentPosition = 0;                                  		   // Current score.
        this.TimeRemaining =  Game.settings.Global.Game.MaximumDuration;   // Decreased by time after if chrono launched.
        this.ChronoLaunched = false;                             		   // Launched at first smash.
        this.CurrentWinner = 0;                                   		   // Changed at first smash.
        this.SuperLoading = 0;                                    		   // Super Loading
    }

    void Update()
    {
        if (this.UpdateChrono())
        {
            Finish();
        }
        else
        {
            float smash = this.GetSmashResult();
            if (smash != 0)
            {
                this.ChronoLaunched = true;
                this.MoveForSmash(smash);
                this.UpdateWinner();

                if (currentPosition >= 1 || currentPosition <= -1)
                {
                    this.Finish();
                }
            }
        }
    }

    private bool UpdateChrono()
    {
        if (this.ChronoLaunched)
        {
            float feedsuper = Game.settings.Global.Game.FeedSuperPerSecond * Time.deltaTime;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + feedsuper);

            this.TimeRemaining -= Time.deltaTime;
            return (this.TimeRemaining <= 0);
        }

        return false;
    }
        
    private float GetSmashResult()
    {
        float smash = 0;

        if (Game.southTeam.Player.XboxController.GetButtonDown(Game.settings.Inputs.rightSmashButton.xbox) || 
			Input.GetKeyDown(Game.settings.Inputs.rightSmashButton.keyboard(Game.southTeam)))
        {
            smash += Game.settings.Global.Game.SmashValue;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + Game.settings.Global.Game.FeedSuperPerSmash);
        }

        if (Game.northTeam.Player.XboxController.GetButtonDown( Game.settings.Inputs.leftSmashButton.xbox) ||
			Input.GetKeyDown(Game.settings.Inputs.leftSmashButton.keyboard(Game.northTeam)))
        {
            smash -= Game.settings.Global.Game.SmashValue;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + Game.settings.Global.Game.FeedSuperPerSmash);
        }

        if (this.SuperLoading == 1)
        {
            int super = 0;
            bool used = false;
	
            if (Game.southTeam.Player.XboxController.GetButtonDown(Game.settings.Inputs.rightSuperButton.xbox) 
				|| Input.GetKeyDown(Game.settings.Inputs.rightSuperButton.keyboard(Game.southTeam)))
            {
                super += 1;
                used = true;
            }

            if (Game.northTeam.Player.XboxController.GetButtonDown(Game.settings.Inputs.leftSuperButton.xbox) 
				|| Input.GetKeyDown(Game.settings.Inputs.leftSuperButton.keyboard(Game.northTeam)))
            {
                super -= 1;
                used = true;
            }

            if (used)
            {
                this.SuperLoading = 0;
                smash += super * Game.settings.Global.Game.SuperMultiplicator * Game.settings.Global.Game.SmashValue;
            }
        }

        return smash;
    }

    private void MoveForSmash(float smash)
    {
        currentPosition += smash;
        if (currentPosition > 1)
            currentPosition = 1;
        if (currentPosition < -1)
            currentPosition = -1;

        Vector3 pos = InitialPosition;
        pos.z += currentPosition * Game.settings.Global.Game.MaximumDistance;

        ScrumBloc.transform.position = pos;
    }

    private void UpdateWinner()
    {
        if (currentPosition > 0)
        {
            CurrentWinner = 1;
        }

        if (currentPosition < 0)
        {
            CurrentWinner = -1;
        }
    }

    private Team GetWinner()
    {   
        if (CurrentWinner > 0)
        {
            return Game.southTeam;
        }

        if (CurrentWinner < 0)
        {
            return Game.northTeam;
        }

        throw new UnityException("[scrumController] : Must NEVER happen EVER"); 
    }

    void Finish()
    {
        Team winner = this.GetWinner();

        if (callback != null)
        {
            callback(winner, ScrumBloc.transform.position);
        }

        this.enabled = false;
    }
}