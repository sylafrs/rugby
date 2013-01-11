using UnityEngine;
using System.Collections;

[System.Serializable]
public class ScoreSettings
{
    public int points_drop = 5;
    public int points_essai = 5;
}

[System.Serializable]
public class InputSettings
{
    public KeyCode up = KeyCode.Z;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.Q;
    public KeyCode right = KeyCode.D;
}

[AddComponentMenu("Settings/GameSettings")]
public class GameSettings : MonoBehaviour {

    public ScoreSettings score;
    public InputSettings inputs;   

    public static GameSettings settings = null;

    void Start()
    {
        settings = this;
    }
}
