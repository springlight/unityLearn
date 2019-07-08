using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class CubeSphere : MonoBehaviour
{

    public int  gridSize;
    public float radius = 1;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Color32[] cubeUV;
    private Mesh mesh;
    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        //  WaitForSeconds wait = new WaitForSeconds(0.1f);
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Sphere";

        CreateVertices();
        CreateTriangles();
        CreateColliders();


    }

    private void CreateColliders()
    {

        gameObject.AddComponent<SphereCollider>();
    }


  
    /// <summary>
    /// 创建顶点
    /// </summary>

    private void CreateVertices()
    {
        int cornerVertices = 8;//8个顶点
        //12条边去掉顶点的每个边的顶点，每个轴对应4条边，所以乘以4
        int edgeVertices = (gridSize - 1 + gridSize - 1 + gridSize - 1) * 4;

        int faceVertices = ((gridSize - 1) * (gridSize - 1) + (gridSize - 1) * (gridSize - 1) + (gridSize - 1) * (gridSize - 1)) * 2;

        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];//顶点个数
        normals = new Vector3[vertices.Length];
        cubeUV = new Color32[vertices.Length];
        int v = 0;
        for (int y = 0; y <= gridSize; y++)
        {
            for (int x = 0; x <= gridSize; x++)
            {
                SetVertex(v++, x, y, 0);
            
            }

            for (int z = 1; z <= gridSize; z++)
            {
                SetVertex(v++,gridSize, y, z);
               
            }

            for (int x = gridSize - 1; x >= 0; x--)
            {
                SetVertex(v++,x, y, gridSize);
                //yield return wait;
            }

            for (int z = gridSize - 1; z > 0; z--)
            {
                SetVertex(v++, 0, y, z);
                //yield return wait;
            }
        }

        //上面
        for (int z = 1; z < gridSize; z++)
        {
            for (int x = 1; x < gridSize; x++)
            {
                SetVertex(v++, x, gridSize, z);
                //yield return wait;
            }
        }
        //下面
        for (int z = 1; z < gridSize; z++)
        {
            for (int x = 1; x < gridSize; x++)
            {
                SetVertex(v++, x, 0, z);
                //yield return wait;
            }
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.colors32 = cubeUV;
    }

    private void SetVertex(int i,int x,int y,int z)
    {
        Vector3 v  = new Vector3(x, y, z)*2f/gridSize - Vector3.one;

        float x2 = v.x * v.x;
        float y2 = v.y * v.y;
        float z2 = v.z * v.z;
        Vector3 s;
        s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
        s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
        s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);

        normals[i] = s;
        vertices[i] = normals[i]*radius;
  
        cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z,0);
    }
    /// <summary>
    /// 创建三角形
    /// </summary>
    private void CreateTriangles()
    {
        int[] triangleZ = new int[(gridSize * gridSize) * 12];
        int[] triangleX = new int[(gridSize * gridSize) * 12];
        int[] triangleY = new int[(gridSize * gridSize) * 12];
        //面数
        int quads = (gridSize * gridSize + gridSize * gridSize + gridSize * gridSize) * 2;
        //一个面两个三角形，六个点
        int[] triangles = new int[quads * 6];
        int ring = (gridSize + gridSize) * 2;
        int t = 0;
        int v = 0;
        int tZ = 0;
        int tX = 0;
        int tY = 0;
        //四周的环
        for(int y = 0; y < gridSize; y++, v++)
        {
            for(int q = 0; q < gridSize; q++, v++)
            {
                tZ = SetQuad(triangleZ, tZ, v, v + 1, v + ring, v + ring + 1);
            }

            for(int q =0; q < gridSize; q++, v++)
            {
                tX = SetQuad(triangleX, tX, v, v + 1, v + ring, v + ring + 1);
            }
            for(int q =0;q < gridSize; q++, v++)
            {
                tZ = SetQuad(triangleZ, tZ, v, v + 1, v + ring, v + ring + 1);
            }
            for (int q = 0; q < gridSize - 1; q++, v++)
            {
                tX = SetQuad(triangleX, tX, v, v + 1, v + ring, v + ring + 1);
            }
            tX = SetQuad(triangleX, tX, v, v - ring + 1, v + ring, v + 1);
            //for (int q = 0; q < ring - 1; q++, v++)
            //{
            //    t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            //}
            ////最后一块
            //t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }
        tY = CreateTopFace(triangleY, tY, ring);
        tY = CreateBottomFace(triangleY, tY, ring);
        mesh.subMeshCount = 3;
        mesh.SetTriangles(triangleZ, 0);
        mesh.SetTriangles(triangleX, 1);
        mesh.SetTriangles(triangleY, 2);
        //mesh.triangles = triangles;
    }
    /// <summary>
    /// 计算上面
    /// </summary>
    /// <param name="triangles"></param>
    /// <param name="t"></param>
    /// <param name="ring"></param>
    /// <returns></returns>
    private int CreateTopFace(int [] triangles,int t,int ring)
    {
        int v = ring * gridSize;
        for(int x = 0; x < gridSize - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }
        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

        int vMin = ring * (gridSize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;
        for(int z = 1; z < gridSize -1;z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + gridSize - 1);
            for (int x = 1; x < gridSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid, vMid + 1, vMid + gridSize - 1, vMid + gridSize);
            }

            t = SetQuad(triangles, t, vMid, vMax, vMid + gridSize - 1, vMax + 1);
        }
        //最后一行的第一个
        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < gridSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }
        //最后一块
        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);
        return t;
    }
    private int CreateBottomFace(int[] triangles, int t, int ring)
    {
        int v = 1;
        int vMid = vertices.Length - (gridSize - 1) * (gridSize - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < gridSize - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

        int vMin = ring - 2;
        vMid -= gridSize - 2;
        int vMax = v + 2;

        for (int z = 1; z < gridSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid + gridSize - 1, vMin + 1, vMid);
            for (int x = 1; x < gridSize - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid + gridSize - 1, vMid + gridSize, vMid, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + gridSize - 1, vMax + 1, vMid, vMax);
        }

        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        for (int x = 1; x < gridSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }
        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

        return t;
    }

    private static int SetQuad(int [] triangles,int i,int v00,int v10,int v01,int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }
    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        Gizmos.color = Color.black;

        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }

}
