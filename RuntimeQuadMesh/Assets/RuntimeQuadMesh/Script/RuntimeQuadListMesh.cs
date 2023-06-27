using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RuntimeQuadListMesh : MonoBehaviour
{

    bool inited = false;
    MeshFilter mf;
    Mesh mesh;
    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uvs = new List<Vector2>();
    List<Vector3> normals = new List<Vector3>();

    void Start()
    {
        if(!inited)
        {
            Init();
        }

        //test
        AddQuadRaw(new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 0));

        AddQuadRaw(new Vector3(1, 1, 0),
            new Vector3(2, 1, 0),
            new Vector3(2, 2, 0),
            new Vector3(1, 2, 0));
        Apply();
    }

    void Update()
    {
        
    }

    public void Init()
    {
        mf = GetComponent<MeshFilter>();
        if(mf == null)
        {
            mf = gameObject.AddComponent<MeshFilter>();
        }
        mesh = new Mesh();
        mf.mesh = mesh;
        inited = true;
    }

    public void AddQuadRaw(Vector3 v0,Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int inx0 = verts.Count;
        verts.Add(v0);
        verts.Add(v1);
        verts.Add(v2);
        verts.Add(v3);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(0, 1));

        tris.Add(inx0 + 0);
        tris.Add(inx0 + 2);
        tris.Add(inx0 + 1);
        tris.Add(inx0 + 0);
        tris.Add(inx0 + 3);
        tris.Add(inx0 + 2);

        //n1 (v10,v12)
        var n1 = Vector3.Cross(verts[inx0+0] - verts[inx0+1], verts[inx0+2] - verts[inx0+1]).normalized;
        //n2 (v32,v30)
        var n2 = Vector3.Cross(verts[inx0+2] - verts[inx0+3], verts[inx0+0] - verts[inx0+3]).normalized;
        normals.Add(0.5f * (n1 + n2));
        normals.Add(normals[inx0+0]);
        normals.Add(n1);
        normals.Add(n2);
    }

    public void Apply()
    {
        mesh.vertices = verts.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.normals = normals.ToArray();
        mf.mesh = mesh;
    }
}
