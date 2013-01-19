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
 */
[System.Serializable]
public class InputSettings
{
    public KeyCode up = KeyCode.Z;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.Q;
    public KeyCode right = KeyCode.D;
    public KeyCode change = KeyCode.Space;
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
}
