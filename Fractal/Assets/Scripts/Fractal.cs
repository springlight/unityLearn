using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh [] meshes;
    public Material material;
    public int maxDepth;
    private int depth;
    public float localScale = 0.5f;
    private Vector3 direction;
    private Material[,] materials;
    public float spawnProbability;
    // Use this for initialization
    private static Vector3[] childDirection =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };
    private static Quaternion[] childOrientation =
    {
        Quaternion.identity,
        Quaternion.Euler(0f,0f,-90f),
         Quaternion.Euler(0f,0f,90f),
         Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };
	void Start ()
    {

        if(materials == null)
        {
            InitializeMaterials();
        }
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth,Random.Range(0,2)];
        if (depth < maxDepth)
        {

            StartCoroutine(CreateChildren());
        }
	}
    private void Update()
    {
        transform.Rotate(0f, 30f * Time.deltaTime, 0f);
    }

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1,2];
        for(int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i,0] = new Material(material);
            materials[i,0].color = Color.Lerp(Color.white, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        materials[maxDepth,0].color = Color.magenta;
        materials[maxDepth, 0].color = Color.red;
    }

    private IEnumerator CreateChildren()
    {
        for(int i = 0; i < childDirection.Length; i++)
        {
            if(Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().Init(this, i);
            }

        }
    }
    private void Init(Fractal parent,int idx)
    {
        spawnProbability = parent.spawnProbability;
        meshes = parent.meshes;
        material = parent.material;
        depth = parent.depth + 1;
        maxDepth = parent.maxDepth;
        transform.parent = parent.transform;
        localScale = parent.localScale;
        transform.localScale = Vector3.one * localScale;
        transform.localPosition = childDirection[idx] * (0.5f + 0.5f * localScale);
        transform.localRotation = childOrientation[idx];
        materials = parent.materials;
    }
}
