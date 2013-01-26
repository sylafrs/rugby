using UnityEngine;
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
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode left = KeyCode.RightArrow;
    public KeyCode right = KeyCode.LeftArrow;
	public KeyCode pass = KeyCode.Alpha1;
    public KeyCode drop = KeyCode.Alpha2;
	public KeyCode plaquer = KeyCode.Alpha3;
}

/**
 * @class GameSettings
 * @brief Classe de reglages
 * @author Sylvain Lafon
 */
[AddComponentMenu("Settings/GameSettings")]
public class GameSettings : MonoBehaviour {

    public ScoreSettings score;     // Attribution des Points
    public InputSettings inputs;    // Controles J1
    public InputSettings inputs2;   // Controles J2

    public float timePlaque = 3;

    public float Vheight = 1;
    public float Vwidth = 1;
    public float LineSpace = 1;
}
