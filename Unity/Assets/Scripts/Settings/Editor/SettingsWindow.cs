using UnityEngine;
using UnityEditor;
using System.Collections;

public class SettingsWindow : EditorWindow
{
    [MenuItem("Designers/Reglages")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SettingsWindow>();
    }

    bool toogleInput = false, 
         toogleScore = false;

    void OnGUI()
    {
        toogleInput = EditorGUILayout.BeginToggleGroup("Inputs", toogleInput);
        if (toogleInput)
        {
            GameSettings.up = (KeyCode)EditorGUILayout.EnumPopup("Up", (System.Enum)GameSettings.up);
            GameSettings.down = (KeyCode)EditorGUILayout.EnumPopup("Down", (System.Enum)GameSettings.down);
            GameSettings.right = (KeyCode)EditorGUILayout.EnumPopup("Right", (System.Enum)GameSettings.right);
            GameSettings.left = (KeyCode)EditorGUILayout.EnumPopup("Left", (System.Enum)GameSettings.left);
        }
        EditorGUILayout.EndToggleGroup();

        toogleScore = EditorGUILayout.BeginToggleGroup("Score", toogleScore);
        if (toogleScore)
        {
            GameSettings.points_drop = EditorGUILayout.IntField("Points pour un drop", GameSettings.points_drop);
        }
        EditorGUILayout.EndToggleGroup();
    }
}