using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadingInWorldSpace : MonoBehaviour
{
    public GameObject other;
    Renderer rend;
    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(other != null)
        rend.sharedMaterial.SetVector("_Point", other.transform.position);
    }
}
