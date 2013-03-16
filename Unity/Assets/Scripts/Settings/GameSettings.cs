using UnityEngine;
using XInputDotNetPure; 
using System.Collections;

/**
 * @class ScoreSettings
 * @brief Reglages des points
 * @author Sylvain Lafon
 */
[System.Serializable]
public class ScoreSettings
{
    public int points_drop = 5;
    public int points_essai = 5;
}

/**
 * @class SuperSettings
 * @brief Variab les for settings
 * @author Maxens Dubois
 */
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
	public bool  superUnblockable           = true;
	//passe
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
	//touche = touch
	public int touchWinSuperPoints  	= 20;
	public int touchLooseSuperPoints  	= 10;
	//tackle = plaquage
	public int tackleWinSuperPoints    = 20;
	public int tackleLooseSuperPoints  = 5;
	//scrum = melee
	public int scrumWinSuperPoints  	= 30;
	public int scrumLooseSuperPoints  	= 10;
}

/**
 * @class InputSettings
 * @brief Reglages des entrées pour un Gamer
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[System.Serializable]
public class InputSettings
{    
    public InputTouch passLeft, passRight, drop, tackle, reset, enableIA, scrumNormal, scrumExtra, put;
    public InputDirection move;
}

/**
 * @class GameSettings
 * @brief Classe de reglages
 * @author Sylvain Lafon
 */
[AddComponentMenu("Settings/GameSettings")]
public class GameSettings : myMonoBehaviour {

    public ScoreSettings score;     // Attribution des Points
	public SuperSettings super;     // for super settings
    public InputSettings inputs;    // Controles J1
    public InputSettings inputs2;   // Controles J2

    public float timePlaque = 3;

    public float Vheight = 1;
    public float Vwidth = 1;
    public float LineSpace = 1;

    public float maxTimeHoldingPassButton = 3; // Seconds
}
