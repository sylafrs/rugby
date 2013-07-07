using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/**
  * @class PivotObjectEditor
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
[CustomEditor(typeof(PivotObject))]
public class PivotObjectEditor : Editor {

    PivotObject po;

    public void OnEnable()
    {
        po = target as PivotObject;
        po.InitInfos();

        uv = po.uvRect;
        size = po.size;
        pivot = po.pivot;
    }

    Rect uv;
    Vector2 size;
    PivotObject.PIVOT_POSITION pivot;

    public override void OnInspectorGUI()
    {
        uv = EditorGUILayout.RectField("Uv rect", uv);
        if (uv != po.uvRect)
        {
            po.uvRect = uv;
        }

        size = EditorGUILayout.Vector2Field("Size", size);
        if (size != po.size)
        {
            po.size = size;
        }

        pivot = (PivotObject.PIVOT_POSITION)EditorGUILayout.EnumPopup("Pivot", pivot);
        if (pivot != po.pivot)
        {
            po.pivot = pivot;
        }
    }

    [UnityEditor.MenuItem("Tools/Create PivotObject")]
    public static void CreatePivotObject()
    {
        Mesh mesh = new Mesh();

        const float h = 1, w = 1;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-w / 2, h / 2, 0);
        vertices[1] = new Vector3(w / 2, h / 2, 0);
        vertices[2] = new Vector3(-w / 2, -h / 2, 0);
        vertices[3] = new Vector3(w / 2, -h / 2, 0);

        int[] triangles = new int[] {
            0, 1, 3,
            0, 3, 2
        };

        Rect r = new Rect(0, 0, 1, 1);
        Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(r.x, r.y + r.height);
        uvs[1] = new Vector2(r.x + r.width, r.y + r.height);
        uvs[2] = new Vector2(r.x, r.y);
        uvs[3] = new Vector2(r.x + r.width, r.y);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        GameObject g = new GameObject("PivotObject");
        g.AddComponent<MeshFilter>().sharedMesh = mesh;
        g.AddComponent<MeshRenderer>();
        PivotObject p = g.AddComponent<PivotObject>();
        p.pivot = PivotObject.PIVOT_POSITION.CENTER;
    }
}
