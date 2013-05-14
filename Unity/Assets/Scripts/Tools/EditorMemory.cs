using UnityEngine;
using System.Collections.Generic;

/**
  * @class MiniMemory
  * @brief Permet de stocker quelques infos dans la scène pour l'éditeur.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
[AddComponentMenu("Code Tools/EditorMemory")]
public class EditorMemory : MonoBehaviour
{
    public string DebugWindowFilter = string.Empty;

    public static EditorMemory Get()
    {
        GameObject g = GameObject.Find("EditorMemory");
        if (g == null)
            return null;

        return g.GetComponent<EditorMemory>();
    }
}
