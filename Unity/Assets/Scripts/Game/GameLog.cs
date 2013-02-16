using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * @clasee GameLog
 * @brief un log dans la fenetre de debug
 * @author Lafon Sylvain
 */
[AddComponentMenu("Scripts/Debug/Game Log")]
public class GameLog : myMonoBehaviour, Debugable
{
    Queue<string> log;

    public GameLog ()
    {
        log = new Queue<string>();
        Size = 10;
	}

    private int _size;
    public int Size
    {
        get
        {
            return _size;
        }
        set
        {
            if (value > 0)
            {
                _size = value;
                resize();
            }
        }
    }

    void resize()
    {
        for (int i = Size; i < log.Count; i++)
        {
            log.Dequeue();
        }
    }

    public void Add(string str)
    {
        if (log.Count > Size)
            log.Dequeue(); 

        log.Enqueue(str);
    }

    public void ForDebugWindow() {
        #if UNITY_EDITOR
            string[] log = this.log.ToArray();

            foreach (var entry in log)
            {
                EditorGUILayout.LabelField(entry);
            }
        #endif
    }
}
