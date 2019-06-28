using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial  class GameLevel : PersistableObject {

    public static GameLevel Cur { get; private set; }

    [SerializeField]
    int populationLimit;

    public int PopulationLimit { get { return populationLimit; } }

    [UnityEngine.Serialization.FormerlySerializedAs("persistentObjects")]
    GameLevelObject[] levelObjects;

    [SerializeField]
    SpawnZone spawnZone;

    private void OnEnable()
    {
        Cur = this;
        if(levelObjects == null)
        {
            levelObjects = new GameLevelObject[0];
        }
    }

    //public Vector3 SpawnPoint
    //{
    //    get { return spawnZone.SpawnPoint; }
    //}
    void Start () {
       // Game.Ins.SpawnZoneOfLevel = spawnZone;
	}
	
	
    public void SpawnShape()
    {
         spawnZone.SpawnShape();
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(levelObjects.Length);
        for(int i =0; i < levelObjects.Length; i++)
        {
            levelObjects[i].Save(writer);
        }
    }
    public override void Load(GameDataReader reader)
    {
        int savedCnt = reader.ReadInt();
        for (int i = 0; i < savedCnt; i++)
        {
            levelObjects[i].Load(reader);
        }
    }

    public void GameUpdate()
    {
        for (int i = 0; i < levelObjects.Length; i++)
        {
            levelObjects[i].GameUpdate();
        }
    }

}
