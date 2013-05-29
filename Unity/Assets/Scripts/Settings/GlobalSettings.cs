using System.Collections;

[System.Serializable]
public class GlobalSettings
{
	public GameSettings Game;
	public SuperSettings Super;
	public TeamSettings Team;
	public GlobalCameraSettings	GlobalCamera;
}

[System.Serializable]
public class GameSettings
{
	public int points_drop = 5;
	public int points_essai = 5;
	public int points_transfo = 3;
	public float period_time = 120f;
}

[System.Serializable]
public class TeamSettings
{
	//formation
	public float Vheight = 5;
	public float Vwidth = 5;
	public float LineSpace = 5;

	public float dMinDefensePlayer = 10f; //sur X
	public float dMaxDefensePlayer = 15f; //sur X
	public float dMinControlledDefense = 15f; //sur Z
	public float dMaxControlledDefense = 25f; //sur Z

	public float dMinOffensivePlayer = 5f; //sur X
	public float dMaxOffensivePlayer = 15f; //sur X
	public float dMinControlledOffensive = 5f; //sur Z
	public float dMaxControlledOffensive = 10f; //sur Z

	public float nbOffensivePlayer;
}

[System.Serializable]
public class SuperSettings
{
	public int superGaugeOffensiveLimitBreak = 200;
	public int superGaugeDefensiveLimitBreak = 200;
	public int superGaugeMaximum = 200;
	//for the effect
	//Duration in s

	public float SuperDashDurationTime = 10f;
	public float SuperStunDurationTime = 2f;

	//public float OffensiveSuperDurationTime	= 10f;
	//public float DefensiveSuperDurationTime	= 10f;
	public float superSpeedScale = 1.5f;
	public float superTackleBoxScale = 1.5f;
	public bool superUnblockable = true;
	//passe
	public int passInterceptSuperPoints = 20;
	public int passWinSuperPoints = 10;
	public int passLooseSuperPoints = 5;
	//drop
	public int dropWinSuperPoints = 20;
	public int dropLooseSuperPoints = 10;
	//try = essais
	public int tryWinSuperPoints = 30;
	public int tryLooseSuperPoints = 20;
	//conversion = transformation
	public int conversionWinSuperPoints = 10;
	public int conversionLooseSuperPoints = 5;
	public int conversionOpponentSuperPoints = 10;
	//touche = touch
	public int touchWinSuperPoints = 20;
	public int touchInterceptSuperPoints = 20;
	public int touchLooseSuperPoints = 5;
	//tackle = plaquage
	public int tackleWinSuperPoints = 20;
	public int tackleLooseSuperPoints = 5;
	//scrum = melee
	public int scrumWinSuperPoints = 30;
	public int scrumLooseSuperPoints = 10;
}

[System.Serializable]
public class GlobalCameraSettings
{
	public float flipSmoothTime;
	public float flipMovingStep;
}