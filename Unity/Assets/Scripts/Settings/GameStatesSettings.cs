using System.Collections;
using UnityEngine;

[System.Serializable]
public class GameStatesSettings
{
	public MainStateSettings MainState;
}

[System.Serializable]
public class MainStateSettings
{
	public IntroStateSettings 	IntroState;
	public PlayingStateSettings PlayingState;
	public EndStateSettings 	EndState;
}

[System.Serializable]
public class IntroStateSettings
{
	public float timeToSleepAfterIntro    = 3; // Seconds (precision : miliseconds)
}

[System.Serializable]
public class EndStateSettings
{
		
}

[System.Serializable]
public class PlayingStateSettings
{
	public MainGameStateSettings 	MainGameState;
	public WaitingStateSettings  	WaintingState;
	public GameActionStateSettings	GameActionState;
}

[System.Serializable]
public class MainGameStateSettings
{
	public RunningStateSettings 	RunningState;
	public PassingStateSettings 	PassingState;
	public TacklingStateSettings 	TacklingState;
}

[System.Serializable]
public class RunningStateSettings
{
	public BallHandlingStateSettings 	BallHandlingState;
	public BallFreeStateSettings		BallFreeState;
}

[System.Serializable]
public class BallHandlingStateSettings
{
	public GainGroundingStateSettings 	GainGroundingState;
	public DodgingStateSettings			DodgingState;
}

[System.Serializable]
public class GainGroundingStateSettings
{
		
}

[System.Serializable]
public class DodgingStateSettings
{
		
}

[System.Serializable]
public class BallFreeStateSettings
{
	public BallFlyingStateSettings BallFlyingState;
	public GroundBallStateSettings GroundBallState;
}

[System.Serializable]
public class BallFlyingStateSettings
{
	public float angleDropKick = 45f;
	public BallDropStateSettings 		BallDropState;
	public BallUpAndUnderStateSettings	BallUpAndUnderState;
}

[System.Serializable]
public class BallDropStateSettings
{
	
}

[System.Serializable]
public class BallUpAndUnderStateSettings
{
	public float angleDropUpAndUnder = 70f;
}

[System.Serializable]
public class GroundBallStateSettings
{
}

[System.Serializable]
public class PassingStateSettings
{
	public float maxTimeHoldingPassButton = 3; // Seconds
}

[System.Serializable]
public class TacklingStateSettings
{
	public float tackledTime = 3;	
}

[System.Serializable]
public class WaitingStateSettings
{
		
}

[System.Serializable]
public class GameActionStateSettings
{
	public ConvertingStateSettings 	ConvertingState;
	public ScrumingStateSettings	ScrumingState;
	public TouchingStateSettings	TouchingSgtate;
	
}

[System.Serializable]
public class ConvertingStateSettings
{
	public bool TransfoRemiseAuCentre = false;
	public AimingConversionStateSettings 	AimingConversion;
	public ConversionFlyStateSettings		ConversionFly;
}

[System.Serializable]
public class AimingConversionStateSettings
{
		
}

[System.Serializable]
public class ConversionFlyStateSettings
{
		
}

[System.Serializable]
public class ScrumingStateSettings
{
	public float timeToGetOutTackleAreaBeforeScrum = 2;
    public int	 minPlayersEachTeamToTriggerScrum = 3;
	public float FeedSuperPerSmash;    // 0 to 1           (tweak)
    public float FeedSuperPerSecond;   // 0 to 1           (tweak)
    public float test;
    public float MaximumDistance;      // Unity Distance   (tweak)
    public float MaximumDuration { get { return test; } set { MyDebug.Log(value); test = value; } }       // Seconds          (tweak)                                                        
    public float SmashValue;           // 0 to 1           (tweak)
    public float SuperMultiplicator;   // Mult             (tweak)

    public Vector3 offsetCamera = new Vector3(1, 1, 0);
}

[System.Serializable]
public class TouchingStateSettings
{
	public bool ToucheRemiseAuCentre = false;
}