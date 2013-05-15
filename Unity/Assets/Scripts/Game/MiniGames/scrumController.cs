using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/*
 *@author Maxens Dubois 
 */
[AddComponentMenu("Scripts/MiniGames/Scrum"),
	RequireComponent(typeof(Game))]
public class scrumController : myMonoBehaviour {
	
    public Transform ScrumBloc                      { private get; set; }   // Object to move   (parameter)
    public Vector3 InitialPosition                  { private get; set; }   // Unity unit       (parameter)
    public System.Action<Team, Vector3> callback    { private get; set; }   // At the end.      (parameter, optionnal)	

    public TweakSettings tweakSettings;                                     // Tweaks
    public InputSettings inputSettings;                                     // Inputs
    public GUISettings guiSettings;                                         // Elems' positions (tweak)

    public  bool ChronoLaunched { get; private set; }                       // First smash      (variable, readonly)
    public  float TimeRemaining { get; private set; }                       // Time to play     (variable, readonly)
    private float currentPosition;                                          // -1 to 1          (variable)
    private int CurrentWinner;                                              // Winner           (variable)
    private float SuperLoading;                                             // 0 to 1           (variable)

    private Game Game;                                                      // Game             (reference)    

    void Start()
    {
        this.Game = this.GetComponent<Game>();
        if (this.Game == null)
        {
            throw new UnityException("[scrumController] : I need a game to work !");
        }

        this.StartGUI();
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

        this.currentPosition = 0;                                   // Current score.
        this.TimeRemaining = this.tweakSettings.MaximumDuration;    // Decreased by time after if chrono launched.
        this.ChronoLaunched = false;                                // Launched at first smash.
        this.CurrentWinner = 0;                                     // Changed at first smash.
        this.SuperLoading = 0;                                      // Super Loading
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
            float feedsuper = this.tweakSettings.FeedSuperPerSecond * Time.deltaTime;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + feedsuper);

            this.TimeRemaining -= Time.deltaTime;
            return (this.TimeRemaining <= 0);
        }

        return false;
    }
        
    private float GetSmashResult()
    {
        float smash = 0;

        if (Game.right.Player.XboxController.GetButtonDown(this.inputSettings.rightSmashButton.xbox) || Input.GetKeyDown(this.inputSettings.rightSmashButton.keyboard))
        {
            smash += this.tweakSettings.SmashValue;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + this.tweakSettings.FeedSuperPerSmash);
        }

        if (Game.left.Player.XboxController.GetButtonDown(this.inputSettings.leftSmashButton.xbox) || Input.GetKeyDown(this.inputSettings.leftSmashButton.keyboard))
        {
            smash -= this.tweakSettings.SmashValue;
            this.SuperLoading = Mathf.Min(1, this.SuperLoading + this.tweakSettings.FeedSuperPerSmash);
        }

        if (this.SuperLoading == 1)
        {
            int super = 0;
            bool used = false;

            if (Game.right.Player.XboxController.GetButtonDown(this.inputSettings.rightSuperButton.xbox) || Input.GetKeyDown(this.inputSettings.rightSuperButton.keyboard))
            {
                super += 1;
                used = true;
            }

            if (Game.left.Player.XboxController.GetButtonDown(this.inputSettings.leftSuperButton.xbox) || Input.GetKeyDown(this.inputSettings.leftSuperButton.keyboard))
            {
                super -= 1;
                used = true;
            }

            if (used)
            {
                this.tweakSettings.FeedSuperPerSmash = 0;
                smash += super * this.tweakSettings.SuperMultiplicator * this.tweakSettings.SmashValue;
            }
        }

        return smash;
    }

    private void MoveForSmash(float smash)
    {
        currentPosition += smash;

        Vector3 pos = InitialPosition;
        pos.z += currentPosition * this.tweakSettings.MaximumDistance;

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
            return Game.right;
        }

        if (CurrentWinner < 0)
        {
            return Game.left;
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

    [System.Serializable]
    public class GUISettings {
        public Rect ScrumSpecialRect;
        public Texture2D ScrumSpecialButton;
        public Rect ScrumBarRect;
        public Texture2D ScrumRightBar;
        public Texture2D ScrumLeftBar;
        public Texture2D ScrumEmptyBar;
    }

    [System.Serializable]
    public class TweakSettings
    {
        public float FeedSuperPerSmash;    // 0 to 1           (tweak)
        public float FeedSuperPerSecond;   // 0 to 1           (tweak)
        public float MaximumDistance;      // Unity            (tweak)
        public float MaximumDuration;      // Seconds          (tweak)                                                        
        public float SmashValue;           // 0 to 1           (tweak)
        public float SuperMultiplicator;   // Mult             (tweak)  
    }

    [System.Serializable]
    public class InputSettings
    {
        public InputTouch rightSmashButton;
        public InputTouch leftSmashButton; 
        public InputTouch rightSuperButton;
        public InputTouch leftSuperButton; 
    }

    void StartGUI()
    {
        Rect rect;        
       
        rect = gameUIManager.screenRelativeRect(this.guiSettings.ScrumSpecialRect);
        this.guiSettings.ScrumSpecialRect = rect;

        rect = gameUIManager.screenRelativeRect(this.guiSettings.ScrumBarRect);
        this.guiSettings.ScrumBarRect = rect;    
    }

    void OnGUI()
    {
        if (!ChronoLaunched)
        {
            GUILayout.Label("BEGIN !");
        }
        else
        {
            GUILayout.Label("SMASH UNTIL " + (int)TimeRemaining + " !");
        }

        this.DrawBar();

        if (SuperLoading == 1)
        {
            GUI.DrawTexture(guiSettings.ScrumSpecialRect, guiSettings.ScrumSpecialButton, ScaleMode.ScaleToFit);
        }
    }

    void DrawBar()
    {       
        float leftPercent = (1 + currentPosition) / 2;
        float leftWidth = leftPercent * guiSettings.ScrumBarRect.width;

        Rect rightRect = guiSettings.ScrumBarRect;
        Rect leftRect = guiSettings.ScrumBarRect;

        leftRect.width = leftWidth;
        rightRect.width -= leftWidth;
        rightRect.x += leftWidth;
        
        GUI.DrawTexture(rightRect, guiSettings.ScrumRightBar);
        GUI.DrawTexture(leftRect, guiSettings.ScrumLeftBar);
    }
    /*
    void OnDrawGizmos()
    {
        InitialPosition = Vector3.zero;

        Color prev = Gizmos.color;
 
        Vector3 pos = InitialPosition;
        pos.z += currentPosition * this.tweakSettings.MaximumDistance * 100;
       
        Vector3 posLeft = InitialPosition;
        pos.z -= this.tweakSettings.MaximumDistance * 100;

        Vector3 posRight = InitialPosition;
        pos.z += this.tweakSettings.MaximumDistance * 100;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(posLeft, pos);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(posRight, pos);
        
        Gizmos.color = prev;
    }
    */
}