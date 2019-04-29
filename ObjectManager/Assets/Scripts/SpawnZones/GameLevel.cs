using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour {

    [SerializeField]
    SpawnZone spawnZone;
	void Start () {
        Game.Ins.SpawnZoneOfLevel = spawnZone;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
