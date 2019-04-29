using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeSpawnZone : SpawnZone
{
    [SerializeField]
    SpawnZone[] spawnZones;
    public override Vector3 SpawnPoint
    {
        get
        {
            int idx = Random.Range(0, spawnZones.Length);
            return spawnZones[idx].SpawnPoint;
        }
    }
}
