using System.Collections;

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
		
}

[System.Serializable]
public class GroundBallStateSettings
{
		
}

[System.Serializable]
public class PassingStateSettings
{
		
}

[System.Serializable]
public class TacklingStateSettings
{
		
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
		
}

[System.Serializable]
public class TouchingStateSettings
{
		
}