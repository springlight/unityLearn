using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : PersistableObject {

    public static GameLevel Cur { get; private set; }

    [SerializeField]
    PersistableObject[] persistableObjects;

    [SerializeField]
    SpawnZone spawnZone;

    private void OnEnable()
    {
        Cur = this;
        if(persistableObjects == null)
        {
            persistableObjects = new PersistableObject[0];
        }
    }

    //public Vector3 SpawnPoint
    //{
    //    get { return spawnZone.SpawnPoint; }
    //}
    void Start () {
       // Game.Ins.SpawnZoneOfLevel = spawnZone;
	}
	
	
    public void ConfigureSpawn(Shape shape)
    {
        spawnZone.ConfigureSpawn(shape);
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(persistableObjects.Length);
        for(int i =0; i < persistableObjects.Length; i++)
        {
            persistableObjects[i].Save(writer);
        }
    }
    public override void Load(GameDataReader reader)
    {
        int savedCnt = reader.ReadInt();
        for (int i = 0; i < savedCnt; i++)
        {
            persistableObjects[i].Load(reader);
        }
    }
}
