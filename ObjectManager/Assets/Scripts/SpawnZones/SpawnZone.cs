using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZone : PersistableObject {

   
	public abstract Vector3 SpawnPoint
    {
        get;
      
    }

    public virtual void ConfigureSpawn(Shape shape)
    {
        Transform t = shape.transform;

        t.localPosition = SpawnPoint;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        shape.SetColor(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.25f, 1f, 1f, 1f));
        shape.AngularVelocity = Random.onUnitSphere * Random.Range(5f, 90f);
        // shape.Velocity = Random.onUnitSphere * Random.Range(0, 2f);
        shape.Velocity = transform.forward * Random.Range(0f, 2f);
    }
    
}
