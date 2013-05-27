using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/**
  * @class SwitchRendererRecursivelyEditor
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
[CustomEditor(typeof(SwitchRendererRecursively))]
public class SwitchRendererRecursivelyEditor : Editor {

    private Transform targetTransform;

    public void OnEnable()
    {
        targetTransform = (target as SwitchRendererRecursively).transform;
        Check();
    }

    private List<Renderer> enabledRenderers;
    private List<Renderer> disabledRenderers;

    public void Check()
    {
        List<Renderer> childRenderers = GetRendererChildren(targetTransform);
        this.enabledRenderers = GetEnabledRenderers(childRenderers);
        this.disabledRenderers = GetDisabledRenderers(childRenderers);
    }

    public override void OnInspectorGUI()
    {
        if (targetTransform == null)
        {
            return;
        }
               
        if (this.disabledRenderers.Count > 0)
        {
            if (GUILayout.Button("Enable all renderers within " + targetTransform.name))
            {
                Check();

                foreach (Renderer r in disabledRenderers)
                {
                    r.enabled = true;
                }

                enabledRenderers.AddRange(disabledRenderers);
                disabledRenderers.Clear();
            }
        }

        if (this.enabledRenderers.Count > 0)
        {
            if (GUILayout.Button("Disable all renderers within " + targetTransform.name))
            {
                Check();

                foreach (Renderer r in enabledRenderers)
                {
                    r.enabled = false;
                }

                disabledRenderers.AddRange(enabledRenderers);
                enabledRenderers.Clear();
            }
        }
    }

    public static List<Renderer> GetEnabledRenderers(List<Renderer> list)
    {
        List<Renderer> result = new List<Renderer>();

        if(list != null)
            foreach (var r in list)
                if (r.enabled)
                    result.Add(r);

        return result;
    }

    public static List<Renderer> GetDisabledRenderers(List<Renderer> list)
    {
        List<Renderer> result = new List<Renderer>();

        if (list != null)
            foreach (var r in list)
                if (!r.enabled)
                    result.Add(r);

        return result;
    }

    public static List<Renderer> GetRendererChildren(Transform t)
    {
        return GetRendererChildren(t, null);
    }

    public static List<Renderer> GetRendererChildren(Transform t, List<Renderer> renderers)
    {
        if (renderers == null)
        {
            renderers = new List<Renderer>();
        }

        if (t != null)
        {
            if (t.renderer != null)
            {
                renderers.Add(t.renderer);
            }

            foreach (Transform c in t)
            {
                renderers = GetRendererChildren(c, renderers);
            }
        }            

        return renderers;
    }
}
