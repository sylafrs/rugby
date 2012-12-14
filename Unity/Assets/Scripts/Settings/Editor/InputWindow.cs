using UnityEngine;
using UnityEditor;
using System.Collections;

public class InputWindow : EditorWindow
{
    [MenuItem("Reglages - GD/Inputs")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<InputWindow>();
    }

    void OnGUI()
    {
        InputSettings.up = (KeyCode)EditorGUILayout.EnumPopup("Up", (System.Enum)InputSettings.up);
        InputSettings.down = (KeyCode)EditorGUILayout.EnumPopup("Down", (System.Enum)InputSettings.down);
        InputSettings.right = (KeyCode)EditorGUILayout.EnumPopup("Right", (System.Enum)InputSettings.right);
        InputSettings.left = (KeyCode)EditorGUILayout.EnumPopup("Left", (System.Enum)InputSettings.left);
    }
}