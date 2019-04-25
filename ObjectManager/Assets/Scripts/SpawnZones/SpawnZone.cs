using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour {

	public Vector3 SpawnPoint
    {
        get
        {
            return transform.TransformPoint(Random.insideUnitSphere);
        }
    }
}
