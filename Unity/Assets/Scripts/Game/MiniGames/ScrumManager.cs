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
	
    public ScrumBloc ScrumBloc                      { private get; set; }   // Object to move   (parameter)
    public Vector3 InitialPosition                  { private get; set; }   // Unity unit       (parameter)
    public System.Action<Team, Vector3> callback    { private get; set; }   // At the end.      (parameter, optionnal)	

    public  bool ChronoLaunched { get; private set; }                       // First smash      (variable, readonly)
    public  float TimeRemaining { get; private set; }                       // Time to play     (variable, readonly)
    public float currentPosition { get; private set; }                      // -1 to 1          (variable, readonly)
    private int CurrentWinner;                                              // Winner           (variable, private)
    public float SuperLoading { get; private set; }                         // 0 to 1           (variable, readonly)
    public float FeedSuperPerSmash { get; private set; }                    // 0 to 1           (variable, readonly)
    public float InvincibleTime { get; private set; }                       // Time without malus (variable, readonly)
    public float MalusSouth { get; private set; }                           // when south failed  (variable, readonly)
    public float MalusNorth { get; private set; }                           // when north failed  (variable, readonly)    

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

            this.ScrumBloc = o.GetComponent<ScrumBloc>();
        }

        ScrumBloc.transform.position = InitialPosition;
        ScrumBloc.smoothPosition = true;

        this.currentPosition = 0;                                  		   // Current score.
        this.TimeRemaining = this.settings.MaximumDuration;                // Decreased by time after if chrono launched.
        this.ChronoLaunched = false;                             		   // Launched at first smash.
        this.CurrentWinner = 0;                                   		   // Changed at first smash.
        this.SuperLoading = 0;                                    		   // Super Loading    

        this.MalusNorth = -1;
        this.MalusSouth = -1;
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
            bool superUsed;
            float smash = this.GetSmashResult(out superUsed);
            if (smash != 0)
            {
                this.ChronoLaunched = true;
                this.MoveForSmash(smash);
                if (superUsed)
                    ScrumBloc.transform.position = ScrumBloc.idealPosition;

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
        
    private float GetSmashResult(out bool used)
    {
        float smash = 0;
        used = false; 

        InputSettings inputs = game.settings.Inputs;
        XboxInputs.Controller southCtrl = game.southTeam.Player.XboxController;
        XboxInputs.Controller northCtrl = game.northTeam.Player.XboxController;

        bool smashSouth = southCtrl.GetButtonDown(inputs.smashButton.xbox) || Input.GetKeyDown(inputs.smashButton.keyboard(game.southTeam));
        bool smashNorth = northCtrl.GetButtonDown(inputs.smashButton.xbox) || Input.GetKeyDown(inputs.smashButton.keyboard(game.northTeam));

        bool superSouth = southCtrl.GetButtonDown(inputs.superButton.xbox) || Input.GetKeyDown(inputs.superButton.keyboard(game.southTeam));
        bool superNorth = northCtrl.GetButtonDown(inputs.superButton.xbox) || Input.GetKeyDown(inputs.superButton.keyboard(game.northTeam));
        
        if (smashSouth)
        {
            smash += this.settings.SmashValue;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + this.settings.FeedSuperPerSmash);            
            this.ScrumBloc.FeedBackSmash(game.southTeam);
        }

        if (smashNorth)
        {
            smash -= this.settings.SmashValue;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + this.settings.FeedSuperPerSmash);            
            this.ScrumBloc.FeedBackSmash(game.northTeam);
        }

        if (this.SuperLoading == 1)
        {
            int super = 0;       

            if (superSouth)
            {
                super += 1;
                used = true;
            }

            if (superNorth)
            {
                super -= 1;
                used = true;
            }

            if (used)
            {
                this.InvincibleTime = settings.InvincibleCooldown;
                this.SuperLoading = 0;
                smash += super * this.settings.SuperMultiplicator * this.settings.SmashValue;

                if (super != 0)
                {
                    Team t = game.southTeam;
                    if (super != 1)
                    {
                        t = game.northTeam;
                    }

                    game.refs.gameObjects.ScrumBloc.PushFor(t);
                }
            }
        }
        else 
        {
            if (InvincibleTime > 0)
            {
                InvincibleTime -= Time.deltaTime;
            }
            else 
            {
                if (superSouth)
                {
                    this.MalusSouth = Time.time;
                    smash -= this.settings.SmashValue * this.settings.MalusValue;
                    southCtrl.SetLeftVibration(0.8f, 0.4f);
                }

                if (superNorth)
                {
                    this.MalusNorth = Time.time;
                    smash += this.settings.SmashValue * this.settings.MalusValue;
                    northCtrl.SetLeftVibration(0.8f, 0.4f);
                }
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

        ScrumBloc.idealPosition = pos;
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