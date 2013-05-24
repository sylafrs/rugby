using UnityEngine;
using System.Collections;
using XInputDotNetPure;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 *@author Maxens Dubois 
 */
[AddComponentMenu("Scripts/MiniGames/Scrum")]
public class ScrumManager : myMonoBehaviour, Debugable {
	
    public Transform ScrumBloc                      { private get; set; }   // Object to move   (parameter)
    public Vector3 InitialPosition                  { private get; set; }   // Unity unit       (parameter)
    public System.Action<Team, Vector3> callback    { private get; set; }   // At the end.      (parameter, optionnal)	

    public  bool ChronoLaunched { get; private set; }                       // First smash      (variable, readonly)
    public  float TimeRemaining { get; private set; }                       // Time to play     (variable, readonly)
    public float currentPosition { get; private set; }                      // -1 to 1          (variable, readonly)
    private int CurrentWinner;                                              // Winner           (variable, private)
    public float SuperLoading { get; private set; }                         // 0 to 1           (variable, readonly)
    public float FeedSuperPerSmash { get; private set; }                    // 0 to 1           (variable, readonly)

    private  ScrumingStateSettings settings;                                // Tweaks           (reference)
    private  Game game;                                                     // Game             (reference)    

    void OnEnable()
    {
        if (this.game == null || this.settings == null)
        {
            this.game = Game.instance;

            this.settings = game.settings.GameStates
                                          .MainState
                                          .PlayingState
                                          .GameActionState
                                          .ScrumingState;
            if (this.game == null)
            {
                throw new UnityException("[scrumController] : I need a game to work !");
            }
        }

        if (this.ScrumBloc == null)
        {
            GameObject o = GameObject.Find("SCRUM");
            if(o == null)
                o = new GameObject("SCRUM");

            this.ScrumBloc = o.transform;
        }

        this.currentPosition = 0;                                  		   // Current score.
        this.TimeRemaining = this.settings.MaximumDuration;                // Decreased by time after if chrono launched.
        this.ChronoLaunched = false;                             		   // Launched at first smash.
        this.CurrentWinner = 0;                                   		   // Changed at first smash.
        this.SuperLoading = 0;                                    		   // Super Loading    
    }

    void Update()
    {
        if (this.UpdateChrono())
        {
            MyDebug.Log("[scrumController] : Chrono !");
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
                    MyDebug.Log("[scrumController] : Position !");
                    this.Finish();
                }
            }
        }
    }

    private bool UpdateChrono()
    {
        if (this.ChronoLaunched)
        {
            float feedsuper = this.settings.FeedSuperPerSecond * Time.deltaTime;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + feedsuper);

            this.TimeRemaining -= Time.deltaTime;
            return (this.TimeRemaining <= 0);
        }

        return false;
    }
        
    private float GetSmashResult()
    {
        float smash = 0;

        if (game.southTeam.Player.XboxController.GetButtonDown(game.settings.Inputs.smashButton.xbox) || 
			Input.GetKeyDown(game.settings.Inputs.smashButton.keyboard(game.southTeam)))
        {
            smash += this.settings.SmashValue;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + this.settings.FeedSuperPerSmash);
        }

        if (game.northTeam.Player.XboxController.GetButtonDown(game.settings.Inputs.smashButton.xbox) ||
			Input.GetKeyDown(game.settings.Inputs.smashButton.keyboard(game.northTeam)))
        {
            smash -= this.settings.SmashValue;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + this.settings.FeedSuperPerSmash);
        }

        if (this.SuperLoading == 1)
        {
            int super = 0;
            bool used = false;
	
            if (game.southTeam.Player.XboxController.GetButtonDown(game.settings.Inputs.superButton.xbox) 
				|| Input.GetKeyDown(game.settings.Inputs.superButton.keyboard(game.southTeam)))
            {
                super += 1;
                used = true;
            }

            if (game.northTeam.Player.XboxController.GetButtonDown(game.settings.Inputs.superButton.xbox) 
				|| Input.GetKeyDown(game.settings.Inputs.superButton.keyboard(game.northTeam)))
            {
                super -= 1;
                used = true;
            }

            if (used)
            {
                this.SuperLoading = 0;
                smash += super * this.settings.SuperMultiplicator * this.settings.SmashValue;
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
        pos.z += currentPosition * this.settings.MaximumDistance;

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

    public Team GetWinner()
    {   
        if (CurrentWinner > 0)
        {
            return game.southTeam;
        }

        if (CurrentWinner < 0)
        {
            return game.northTeam;
        }

        throw new UnityException("[scrumController] : Must NEVER happen EVER"); 
    }

    void Finish()
    {
        Team winner = this.GetWinner();
        MyDebug.Log("[scrumController] : Winner is : " + winner);        

        if (callback != null)
        {
            callback(winner, ScrumBloc.transform.position);
        }

        this.enabled = false;
    }

    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        EditorGUILayout.LabelField("Chrono", this.ChronoLaunched ? (((int)this.TimeRemaining).ToString() + " seconds") : "Waiting");
#endif
    }
}