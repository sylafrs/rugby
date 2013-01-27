using UnityEngine;
using UnityEditor;
using System.Collections;

/**
  * @class KnowMInput 
  * @brief Affiche la derniere touche appuyée
  * @author Sylvain Lafon
  */
public class KnowMyInput : EditorWindow
{
    [MenuItem("Component/Scripts/KnowMyInput Window")]
    public static void Init()
    {
        EditorWindow.GetWindow(typeof(KnowMyInput));
    }
         
    void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey && e.type == EventType.keyDown && e.keyCode != KeyCode.None)
        {
            last = e.keyCode;
        }

        EditorGUILayout.LabelField(last.ToString());
    }

    KeyCode last = KeyCode.None;   
    void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
