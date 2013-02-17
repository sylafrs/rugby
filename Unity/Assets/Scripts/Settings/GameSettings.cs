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
    public InputSettings inputs;    // Controles J1
    public InputSettings inputs2;   // Controles J2

    public float timePlaque = 3;

    public float Vheight = 1;
    public float Vwidth = 1;
    public float LineSpace = 1;

    public float maxTimeHoldingPassButton = 3; // Seconds
}
