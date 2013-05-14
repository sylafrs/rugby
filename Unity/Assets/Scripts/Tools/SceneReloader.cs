using UnityEngine;
using System.Collections.Generic;

/**
  * @class SceneReloader
  * @brief Description.
  * @author Sylvain Lafon
  */
public class SceneReloader : MonoBehaviour {

    const string SceneName = "reloadScene";
    private static string ToLoad;
    
    public static void Go()
    {
        ToLoad = Application.loadedLevelName;
        Application.LoadLevel(SceneName);
    }

    public void Start()
    {
        Gamer.initGamerId();
        Application.LoadLevel(ToLoad);
    }
}
