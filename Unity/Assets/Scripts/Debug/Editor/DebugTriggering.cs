using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(TriggeringDebugger))]
public class DebugTriggering : Editor {

    Dictionary<Triggering, bool> triggerings;
    Dictionary<Trigger, bool> toDisplay;

    void OnEnable()
    {
        triggerings = new Dictionary<Triggering, bool>();
        toDisplay = new Dictionary<Trigger, bool>();
    }

    public override void OnInspectorGUI()
    {
        Dictionary<Triggering, bool> newTriggerings = new Dictionary<Triggering, bool>(triggerings);
        Dictionary<Trigger, bool> newToDisplay = new Dictionary<Trigger, bool>(toDisplay);

        foreach (var k in triggerings.Keys)
        {
            newTriggerings[k] = EditorGUILayout.Foldout(triggerings[k], (k as Component).gameObject.name);
            if (newTriggerings[k])
            {
                EditorGUI.indentLevel++;
                foreach (var k2 in toDisplay.Keys)
                {
                    if (k2.triggering == k)
                    {
                        newToDisplay[k2] = EditorGUILayout.Toggle(k2.name, toDisplay[k2]);
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        toDisplay = newToDisplay;
        triggerings = newTriggerings;
        
        //if (GUILayout.Button("Refresh", GUILayout.Width(200)))
        //{
            GameObject root = GameObject.Find("Scene");
            if (root != null)
            {
                List<Trigger> found = new List<Trigger>();
                List<Triggering> targets = new List<Triggering>();
                Search(root, found, targets);
                
                CheckDico<Trigger, bool>(found, toDisplay, true);
                CheckDico<Triggering, bool>(targets, triggerings, false);
            }
        //}
    }
              
	void OnSceneGUI()
	{
        foreach (var t in toDisplay)
        {
            if (t.Value && triggerings[t.Key.triggering])
            {
                Vector3 pos = t.Key.transform.position;
                pos.y = 0;
                
                Handles.color = Color.red;
                Handles.DrawWireDisc(pos, Vector3.up, (t.Key.collider as SphereCollider).radius);
        
            }
        }

        SceneView.RepaintAll();
	}

    void Search(GameObject o, List<Trigger> triggers, List<Triggering> targets)
    {
        SphereCollider sc = o.GetComponent<SphereCollider>();
        if (sc != null)
        {
            if (sc.isTrigger)
            {
                Component[] components = o.GetComponents(typeof(Trigger));
                foreach (var c in components)
                {
                    Trigger t = c as Trigger;
                    if (t.triggering != null)
                    {
                        triggers.Add(t);
                        if (!targets.Contains(t.triggering))
                        {
                            targets.Add(t.triggering);
                        }
                    }
                }
            }
        }
        
        int nChild = o.transform.childCount;
        for (int i = 0; i < nChild; i++)
        {
            Search(o.transform.GetChild(i).gameObject, triggers, targets);
        }
    }

    void CheckDico<T, T2>(List<T> list, Dictionary<T, T2> dico, T2 def) {
        int nList = list.Count;
        for (int i = 0; i < nList; i++)
        {
            if (!dico.ContainsKey(list[i]))
            {
                dico.Add(list[i], def);
            }
        }

        List<T> keys = new List<T>(dico.Keys);

        int nKeys = keys.Count;
        for (int i = 0; i < nKeys; i++)
        {
            if (!list.Contains(keys[i]))
            {
                dico.Remove(keys[i]);
            }
        }
    }
}
