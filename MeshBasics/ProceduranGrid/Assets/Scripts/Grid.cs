using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    public int xSize, ySize;
    private Vector3[] vertices;
    private Mesh mesh;
    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        //WaitForSeconds wait = new WaitForSeconds(0.05f);
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];//顶点个数
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0,y = 0;y <=ySize; y++)
        {
            for(int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;

            //    yield return wait;
            }
        }
        //gei mesh指定顶点
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;
        ////画一个三角形
        //int[] triangles = new int[3];
        //triangles[0] = 0;
        //triangles[1] = xSize +1;
        //triangles[2] = 1;//指定下一行的第一个顶点
        //画一个矩形,四个矩形要六个顶点
        //int[] triangles = new int[6];
        //triangles[0] = 0;
        //triangles[4] = triangles[1] = xSize + 1;
        //triangles[3] = triangles[2] = 1;
        //triangles[5] = xSize + 2;

        //第一排10个矩形，10 * 6 = 60个顶点
        //int[] triangles = new int[xSize * 6];

        //for (int ti = 0, vi = 0, x = 0; x < xSize; x++, ti += 6, vi++)
        //{
        //    triangles[ti] = vi;
        //    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
        //    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
        //    triangles[ti + 5] = vi + xSize + 2;
        //    mesh.triangles = triangles;
        //    yield return wait;
        //}
        //填充所有的矩形
        int[] triangles = new int[xSize * ySize * 6];
        
        for (int ti = 0, vi = 0,y = 0; y < ySize; y++, vi++)
        {
            for(int x = 0; x <xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
               // yield return wait;
               
            }
           
          
        }
        mesh.triangles = triangles;
        //   yield return wait;
        mesh.RecalculateNormals();

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
