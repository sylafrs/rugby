using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DebugWindow : EditorWindow {

    static DebugWindow w;

    List<Component> toDebug = new List<Component>();
    Dictionary<System.Type, System.Boolean> registeredTypes = new Dictionary<System.Type, System.Boolean>();

    [MenuItem("Component/Scripts/Debug Window")]
    public static void Init()
    {
        w = EditorWindow.GetWindow(typeof(DebugWindow)) as DebugWindow;
    }

    void OnGUI()
    {       
        toDebug.Clear();
        GameObject root = GameObject.Find("Scene");
        if (root != null)
        {
            Search(root);
            Print();
        }
        else
        {
            EditorGUILayout.LabelField("Aucun GameObject detecte");
        }
    }

    List<Component> Search(GameObject o)
    {        
        Component[] components = o.GetComponents(typeof(Debugable));
        if (components != null)
        {
            foreach (var c in components)
            {
                toDebug.Add(c);
            }
        }

        int nChild = o.transform.childCount;
        if (nChild > 0)
        {
            for (int i = 0; i < nChild; i++)
            {
                Search(o.transform.GetChild(i).gameObject);
            }
        }

        return toDebug;
    }

    private static int CompareTypes(System.Type a, System.Type b)
    {
        if (a == b) 
            return 0;

        if (a == null) 
            return 1;

        if (b == null) 
            return -1;

        return (a.Name.CompareTo(b.Name));
    }

    private static int CompareComponent(Component a, Component b)
    {
        if (a == b)
            return 0;

        if (a == null)
            return 1;

        if (b == null)
            return -1;

        {
            int val = CompareTypes(a.GetType(), b.GetType());
            if (val != 0)
                return val;
        }

        return a.gameObject.name.CompareTo(b.gameObject.name);
    }

    void Print()
    {
        toDebug.Sort(CompareComponent);
        System.Type t = null;

        foreach(var c in toDebug)
        {            
            if (t != c.GetType())
            {
                t = c.GetType();
                if (!registeredTypes.ContainsKey(t))
                {
                    registeredTypes.Add(t, false);
                }
                registeredTypes[t] = EditorGUILayout.Foldout(registeredTypes[t], t.Name);
            }
            
            if (registeredTypes[t])
            {               
                Debugable d = c as Debugable;

                EditorGUI.indentLevel++;
                d.setToogled(!EditorGUILayout.Foldout(!d.getToogled(), c.gameObject.name));
                if (!d.getToogled())
                {
                    EditorGUI.indentLevel++;
                    d.ForDebugWindow();
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }            
        }
    }

    void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
