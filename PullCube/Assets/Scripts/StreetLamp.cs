using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLamp : MonoBehaviour
{
    public bool lightStar = false;//判断等是否亮着
    public Material defaultMaterial;
    public Material selfMaterial;
    public GameObject selfLight;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == gameObject.tag)
        {
            selfLight.GetComponent<MeshRenderer>().material = selfMaterial;
            lightStar = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == gameObject.tag)
        {
            selfLight.GetComponent<MeshRenderer>().material = defaultMaterial;
            lightStar = false;
        }
    }
}
