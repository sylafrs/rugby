using System.Collections;

[System.Serializable]
public class GlobalSettings
{
	public GameSettings  Game;
	public SuperSettings Super;
}

[System.Serializable]
public class GameSettings
{
	public int 	points_drop = 5;
    public int 	points_essai = 5;
	public int 	points_transfo = 3;
	public float period_time = 120f;
	
	public float timePlaque = 3;

    public float Vheight = 1;
    public float Vwidth = 1;
    public float LineSpace = 1;

    public float maxTimeHoldingPassButton = 3; // Seconds
    public float timeToSleepAfterIntro = 3; // Seconds (precision : miliseconds)

    public float timeToGetOutTackleAreaBeforeScrum = 2;
    public int	 minPlayersEachTeamToTriggerScrum = 3;
	
	public float FeedSuperPerSmash;    // 0 to 1           (tweak)
    public float FeedSuperPerSecond;   // 0 to 1           (tweak)
    public float MaximumDistance;      // Unity            (tweak)
    public float MaximumDuration;      // Seconds          (tweak)                                                        
    public float SmashValue;           // 0 to 1           (tweak)
    public float SuperMultiplicator;   // Mult             (tweak)  
}

[System.Serializable]
public class SuperSettings
{
    public int superGaugeOffensiveLimitBreak   	= 200;
	public int superGaugeDefensiveLimitBreak   	= 200;
	public int superGaugeMaximum				= 200;
	//for the effect
	//Duration in s
	public float OffensiveSuperDurationTime	= 10f;
	public float DefensiveSuperDurationTime	= 10f;
	public float superSpeedScale            = 1.5f;
	public float superTackleBoxScale        = 1.5f;
	public bool  superUnblockable           = true;
	//passe
	public int passInterceptSuperPoints = 20;
    public int passWinSuperPoints  		= 10;
	public int passLooseSuperPoints  	= 5;
	//drop
	public int dropWinSuperPoints  		= 20;
	public int dropLooseSuperPoints  	= 10;
	//try = essais
	public int tryWinSuperPoints  		= 30;
	public int tryLooseSuperPoints  	= 20;
	//conversion = transformation
	public int conversionWinSuperPoints    = 10;
	public int conversionLooseSuperPoints  = 5;
	public int conversionOpponentSuperPoints  = 10;
	//touche = touch
	public int touchWinSuperPoints  	 = 20;
	public int touchInterceptSuperPoints = 20;
	public int touchLooseSuperPoints  	 = 5;
	//tackle = plaquage
	public int tackleWinSuperPoints    = 20;
	public int tackleLooseSuperPoints  = 5;
	//scrum = melee
	public int scrumWinSuperPoints  	= 30;
	public int scrumLooseSuperPoints  	= 10;
}