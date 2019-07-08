using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformer : MonoBehaviour {

    public float springForce = 20f;
    float uniformScale = 1f;
    public float damping = 5f;
    Mesh deformingMesh;
    Vector3[] oriVectices, displaceVertices;
    Vector3[] vertexVelocities;
	// Use this for initialization
	void Start () {

        deformingMesh = GetComponent<MeshFilter>().mesh;
        oriVectices = deformingMesh.vertices;

        displaceVertices = new Vector3[oriVectices.Length];
        vertexVelocities = new Vector3[oriVectices.Length];
        for(int i = 0; i <oriVectices.Length; i++)
        {
            displaceVertices[i] = oriVectices[i];
           
        }
	}
	
	public void AddDeformingForce(Vector3 point,float force)
    {
        Debug.DrawLine(Camera.main.transform.position, point);
        point = transform.InverseTransformPoint(point);
        for (int i = 0; i < displaceVertices.Length; i++)
        {
            AddForceToVertex(i, point, force);
        }
    }

    public void  AddForceToVertex(int i ,Vector3 point,float force)
    {
        Vector3 pointToVertex = displaceVertices[i] - point;
        pointToVertex *= uniformScale;
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    private void Update()
    {
        uniformScale = transform.localScale.x;
        for (int i = 0; i < displaceVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        deformingMesh.vertices = displaceVertices;
        deformingMesh.RecalculateNormals();
    }

    void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        Vector3 displacement = displaceVertices[i] - oriVectices[i];
        displacement *= uniformScale;
        velocity -= displacement * springForce * Time.deltaTime;
        velocity *= 1f - damping * Time.deltaTime;
        vertexVelocities[i] = velocity;
        displaceVertices[i] += velocity * (Time.deltaTime/uniformScale); 
    }
}
