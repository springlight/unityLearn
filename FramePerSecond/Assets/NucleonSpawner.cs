using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonSpawner : MonoBehaviour
{
    public float timeBetweenSpwans;
    public float spawnDistance;
    public Nucleon[] nucleonPrefabs;
    private float timeSinceLastSpwan;
    
    private void FixedUpdate()
    {
        timeSinceLastSpwan += Time.deltaTime;
        if (timeSinceLastSpwan >= timeBetweenSpwans)
        {
            timeSinceLastSpwan -= timeBetweenSpwans;
            SpwanNucleon();
        }
    }

    void SpwanNucleon()
    {
        Nucleon prefab = nucleonPrefabs[Random.Range(0, nucleonPrefabs.Length)];
        Nucleon spwan = Instantiate<Nucleon>(prefab);
        spwan.transform.localPosition = Random.onUnitSphere * spawnDistance;
    }
   
}
