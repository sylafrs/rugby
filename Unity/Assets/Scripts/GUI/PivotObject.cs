using UnityEngine;
using System.Collections.Generic;

/**
  * @class PivotTexture
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class PivotObject : MonoBehaviour {    

    public enum PIVOT_POSITION
    {
        CENTER, LEFT, RIGHT, TOP, BOTTOM
    }

    public PIVOT_POSITION _pivot;
    public PIVOT_POSITION pivot
    {
        get
        {
            if (mesh == null) return PIVOT_POSITION.CENTER;
            return _pivot;
        }
        set
        {
            if (mesh == null) return;
            this.ChangePivot(value);
        }
    }

    public Rect _uvRect;
    public Rect uvRect
    {
        get
        {
            if (mesh == null) return new Rect(0, 0, 0, 0);
            return _uvRect;
        }
        set
        {
            if (mesh == null) return;
            this.ChangeUv(value);
        }
    }

    public Vector2 _size;
    public Vector2 size
    {
        get
        {
            if (mesh == null) return Vector2.zero;
            return _size;
        }
        set
        {
            if (mesh == null) return;
            this.ChangeSize(value);
        }
    }

    Mesh mesh;

    void Start()
    {
        InitInfos();
    }

    public void InitInfos()
    {
        MeshFilter mf = this.GetComponent<MeshFilter>();
        mesh = mf.sharedMesh;

        if (mesh == null)
        {
            mesh = new Mesh();

            float w = _size.x, h = _size.y;
            Vector3[] vertices = new Vector3[4];

            if (pivot == PIVOT_POSITION.CENTER)
            {
                vertices[0] = new Vector3(-w / 2, h / 2, 0);
                vertices[1] = new Vector3(w / 2, h / 2, 0);
                vertices[2] = new Vector3(-w / 2, -h / 2, 0);
                vertices[3] = new Vector3(w / 2, -h / 2, 0);
            }
            else if (pivot == PIVOT_POSITION.BOTTOM)
            {
                vertices[0] = new Vector3(-w / 2, h, 0);
                vertices[1] = new Vector3(w / 2, h, 0);
                vertices[2] = new Vector3(-w / 2, 0, 0);
                vertices[3] = new Vector3(w / 2, 0, 0);
            }
            else
            {
                throw new UnityException("NOT IMPLEMENTED YET");
            }

            int[] triangles = new int[] {
                0, 1, 3,
                0, 3, 2
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            Vector2[] uvs = new Vector2[4];

            uvs[0] = new Vector2(_uvRect.x, _uvRect.y + _uvRect.height);
            uvs[1] = new Vector2(_uvRect.x + _uvRect.width, _uvRect.y + _uvRect.height);
            uvs[2] = new Vector2(_uvRect.x, _uvRect.y);
            uvs[3] = new Vector2(_uvRect.x + _uvRect.width, _uvRect.y);

            mesh.uv = uvs;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            mf.sharedMesh = mesh;
        }
    }

    public void ChangePivot(PIVOT_POSITION pivot)
    {
        if (_pivot == pivot) return;

        float w = size.x, h = size.y;
        Vector3[] vertices = new Vector3[4];

        if (pivot == PIVOT_POSITION.CENTER)
        {
            vertices[0] = new Vector3(-w / 2, h / 2, 0);
            vertices[1] = new Vector3(w / 2, h / 2, 0);
            vertices[2] = new Vector3(-w / 2, -h / 2, 0);
            vertices[3] = new Vector3(w / 2, -h / 2, 0);
        }
        else if (pivot == PIVOT_POSITION.BOTTOM)
        {
            vertices[0] = new Vector3(-w / 2, h, 0);
            vertices[1] = new Vector3(w / 2, h, 0);
            vertices[2] = new Vector3(-w / 2, 0, 0);
            vertices[3] = new Vector3(w / 2, 0, 0);
        }
        else
        {
            throw new UnityException("NOT IMPLEMENTED YET");
        }

        mesh.vertices = vertices;

        Vector2[] uvs = new Vector2[4];
        Rect r = _uvRect;

        uvs[0] = new Vector2(r.x, r.y + r.height);
        uvs[1] = new Vector2(r.x + r.width, r.y + r.height);
        uvs[2] = new Vector2(r.x, r.y);
        uvs[3] = new Vector2(r.x + r.width, r.y);

        mesh.uv = uvs;
                
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        _pivot = pivot;
    }

    public void ChangeUv(Rect r)
    {
        Vector2[] uvs = new Vector2[4];

        uvs[0] = new Vector2(r.x, r.y + r.height);
        uvs[1] = new Vector2(r.x + r.width, r.y + r.height);
        uvs[2] = new Vector2(r.x, r.y);
        uvs[3] = new Vector2(r.x + r.width, r.y);

        _uvRect = r;

        mesh.uv = uvs;
    }

    public void ChangeSize(Vector2 s)
    {
        if (s.x == 0 || s.y == 0) return;

        float h = s.y, w = s.x;

        Vector3[] vertices = new Vector3[4];

        if (_pivot == PIVOT_POSITION.CENTER)
        {
            vertices[0] = new Vector3(-w / 2, h / 2, 0);
            vertices[1] = new Vector3(w / 2, h / 2, 0);
            vertices[2] = new Vector3(-w / 2, -h / 2, 0);
            vertices[3] = new Vector3(w / 2, -h / 2, 0);
        }
        else if (pivot == PIVOT_POSITION.BOTTOM)
        {
            vertices[0] = new Vector3(-w / 2, h, 0);
            vertices[1] = new Vector3(w / 2, h, 0);
            vertices[2] = new Vector3(-w / 2, 0, 0);
            vertices[3] = new Vector3(w / 2, 0, 0);
        }
        else
        {
            throw new UnityException("NOT IMPLEMENTED YET");
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        _size = s;
    }
}
