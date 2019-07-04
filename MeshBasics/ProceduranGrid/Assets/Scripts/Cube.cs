using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    public int xSize, ySize,zSize;
    private Vector3[] vertices;
    private Mesh mesh;
    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        //  WaitForSeconds wait = new WaitForSeconds(0.1f);
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";

        CreateVertices();
        CreateTriangles();



    }
    /// <summary>
    /// 创建顶点
    /// </summary>

    private void CreateVertices()
    {
        int cornerVertices = 8;//8个顶点
        //12条边去掉顶点的每个边的顶点，每个轴对应4条边，所以乘以4
        int edgeVertices = (xSize - 1 + ySize - 1 + zSize - 1) * 4;

        int faceVertices = ((xSize - 1) * (ySize - 1) + (xSize - 1) * (zSize - 1) + (ySize - 1) * (zSize - 1)) * 2;

        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];//顶点个数

        int v = 0;
        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[v++] = new Vector3(x, y, 0);
                //yield return wait;
            }

            for (int z = 1; z <= zSize; z++)
            {
                vertices[v++] = new Vector3(xSize, y, z);
                /*                yield return wait*/
                ;
            }

            for (int x = xSize - 1; x >= 0; x--)
            {
                vertices[v++] = new Vector3(x, y, zSize);
                //yield return wait;
            }

            for (int z = zSize - 1; z > 0; z--)
            {
                vertices[v++] = new Vector3(0, y, z);
                //yield return wait;
            }
        }

        //上面
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, ySize, z);
                //yield return wait;
            }
        }
        //下面
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, 0, z);
                //yield return wait;
            }
        }
        mesh.vertices = vertices;
    }
    /// <summary>
    /// 创建三角形
    /// </summary>
    private void CreateTriangles()
    {
        //面数
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        //一个面两个三角形，六个点
        int[] triangles = new int[quads * 6];
        int ring = (xSize + zSize) * 2;
        int t = 0;
        int v = 0;
        //四周的环
        for(int y = 0; y < ySize; y++, v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }
            //最后一块
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }
        t = CreateTopFace(triangles, t, ring);
        mesh.triangles = triangles;
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
        int v = ring * ySize;
        for(int x = 0; x < xSize - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }
        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);
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
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

}
