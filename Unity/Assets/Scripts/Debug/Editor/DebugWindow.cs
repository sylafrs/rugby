using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/**
  * @class DebugWindow 
  * @brief Fenetre de debug
  * @see Debugable
  * @author Sylvain Lafon 
  */
public class DebugWindow : EditorWindow {

    const string root_gameobject = "GameDesign";

    List<Component> toDebug = new List<Component>();
    Dictionary<System.Type, System.Boolean> registeredTypes = new Dictionary<System.Type, System.Boolean>();
    Dictionary<GameObject, System.Boolean> registeredGO = new Dictionary<GameObject, System.Boolean>();
    Dictionary<Debugable, System.Boolean> registeredD = new Dictionary<Debugable, System.Boolean>();

    [MenuItem("Component/Scripts/Debug Window")]
    public static void Init()
    {
        EditorWindow.GetWindow(typeof(DebugWindow));
    }

    Vector2 scrollPosition = Vector2.zero;

    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        toDebug.Clear();
        GameObject root = GameObject.Find(root_gameobject);
        if (root != null)
        {
            Search(root);
            Print();
        }
        else
        {
            EditorGUILayout.LabelField("Aucun GameObject detecte");
        }

        EditorGUILayout.EndScrollView();
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

    private static int CompareComponentByTypeThenGameObject(Component a, Component b)
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

    private static int CompareComponentByGameObjectThenType(Component a, Component b)
    {
        if (a == b)
            return 0;

        if (a == null)
            return 1;

        if (b == null)
            return -1;

        {
            int val = a.gameObject.name.CompareTo(b.gameObject.name);
            if (val != 0)
                return val;
        }

        return CompareTypes(a.GetType(), b.GetType());
    }

    enum SORT
    {
        COMPONENTS,
        GAMEOBJECTS
    }

    SORT sort = SORT.COMPONENTS;

    void Print()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Trier par : ", GUILayout.Width(65));    
        sort = (SORT)EditorGUILayout.EnumPopup(sort);
        GUILayout.EndHorizontal();

        switch (sort)
        {
            case SORT.COMPONENTS:
                PrintCompos();
                break;
            case SORT.GAMEOBJECTS:
                PrintGO();
                break;
        }
    }
    void PrintCompos() {

        toDebug.Sort(CompareComponentByTypeThenGameObject);
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

                if (!registeredD.ContainsKey(d))
                {
                    registeredD.Add(d, true);
                }

                registeredD[d] = EditorGUILayout.Foldout(registeredD[d], c.gameObject.name);
                if (registeredD[d])
                {
                    EditorGUI.indentLevel++;
                    d.ForDebugWindow();
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }            
        }
    }

    void PrintGO()
    {
        toDebug.Sort(CompareComponentByGameObjectThenType);
        GameObject go = null;

        foreach (var c in toDebug)
        {
            if (go != c.gameObject)
            {
                go = c.gameObject;
                if (!registeredGO.ContainsKey(go))
                {
                    registeredGO.Add(go, false);
                }
                registeredGO[go] = EditorGUILayout.Foldout(registeredGO[go], go.name);
            }

            if (registeredGO[go])
            {
                Debugable d = c as Debugable;

                EditorGUI.indentLevel++;
                if (!registeredD.ContainsKey(d))
                {
                    registeredD.Add(d, true);
                }

                registeredD[d] = EditorGUILayout.Foldout(registeredD[d], c.GetType().Name);
                if (registeredD[d])
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
