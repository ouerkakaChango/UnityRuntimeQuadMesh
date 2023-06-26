using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeQuadMesh : MonoBehaviour
{
    //public Vector3 v0, v1, v2, v3;
    Vector3[] verts = new Vector3[4];
    Vector2[] uvs = new Vector2[4];
    int[] tris = new int[3 * 2];
    Vector3[] normals = new Vector3[4];
    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        InitDefaultQuad();

        //test
        Vector3[] tt = new Vector3[4];
        tt[0] = new Vector3(1, -0.5f, 0);
        tt[1] = new Vector3(2.5f, 0.5f, 0);
        tt[2] = new Vector3(3, 2, 0);
        tt[3] = new Vector3(1, 2, 0);
        SetWorldVerts(tt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //####################################################
    //逆时针，但生成mesh.tri的时候，按Unity Mesh顺时针
    void InitDefaultQuad()
    {
        verts[0] = new Vector3(-0.5f, -0.5f, 0);
        verts[1] = new Vector3(0.5f, -0.5f, 0);
        verts[2] = new Vector3(0.5f, 0.5f, 0);
        verts[3] = new Vector3(-0.5f, 0.5f, 0);
        mesh.vertices = verts;

        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);
        uvs[2] = new Vector2(1, 1);
        uvs[3] = new Vector2(0, 1);
        mesh.uv = uvs;

        tris[0] = 0;
        tris[1] = 2;
        tris[2] = 1;
        tris[3] = 0;
        tris[4] = 3;
        tris[5] = 2;
        mesh.triangles = tris;

        UpdateNormal();
        UpdateToMeshFiliter();
    }

    //void UpdateMesh()
    //{
    //    //mesh.vertices = ;
    //    //mesh.uv = ;
    //    //mesh.triangles = ;
    //    //mesh.normals = ;
    //}

    //允许2个三角形不共面。交点vert0,vert2处法线为0.5f*(n1+n2)
    void UpdateNormal()
    {
        //n1 (v10,v12)
        var n1 = Vector3.Cross(verts[0] - verts[1], verts[2] - verts[1]).normalized;
        //n2 (v32,v30)
        var n2 = Vector3.Cross(verts[2] - verts[3], verts[0] - verts[3]).normalized;
        normals[0] = 0.5f * (n1 + n2);
        normals[2] = normals[0];
        normals[1] = n1;
        normals[3] = n2;

        mesh.normals = normals;
    }

    void UpdateToMeshFiliter()
    {
        var mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;
    }

    void SetWorldVerts(Vector3[] wVerts)
    {
        var center = 0.25f * (wVerts[0] + wVerts[1] + wVerts[2] + wVerts[3]);
        transform.position = center;
        verts[0] = wVerts[0] - center;
        verts[1] = wVerts[1] - center;
        verts[2] = wVerts[2] - center;
        verts[3] = wVerts[3] - center;
        UpdateVertsMesh();
    }

    void UpdateVertsMesh()
    {
        mesh.vertices = verts;
        UpdateNormal();
        UpdateToMeshFiliter();
    }
}
