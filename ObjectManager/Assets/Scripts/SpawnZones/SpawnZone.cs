using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZone : PersistableObject {
    
    [System.Serializable]
    public struct SpwanConfiguration
    {
        public enum MovementDirection
        {
            Forward,
            Upward,
            Outward,
            Random
        }
        public ShapeFactory[] factories;
        public MovementDirection movementDirection;
        public FloatRange speed;
        public FloatRange angularSpeed;
        public FloatRange scale;
        public ColorRangeHSV color;
        public bool uniformColor;
    }

    [SerializeField]
    SpwanConfiguration spwanConfig;
   
    //[SerializeField]
    //SpawnMovementDirection spawnMovementDirection;
    //[SerializeField]
    //FloatRange spwanSpeed;

	public abstract Vector3 SpawnPoint
    {
        get;
      
    }

    //public virtual void ConfigureSpawn(Shape shape)
    public virtual Shape SpawnShape()
    {
        int factoryIdx = Random.Range(0, spwanConfig.factories.Length);
        Shape shape = spwanConfig.factories[factoryIdx].GetRandom();
        Transform t = shape.transform;

        t.localPosition = SpawnPoint;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * spwanConfig.scale.RandomValueInRange;
        // shape.SetColor(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.25f, 1f, 1f, 1f));
        //   shape.AngularVelocity = Random.onUnitSphere * Random.Range(5f, 90f);
        if (spwanConfig.uniformColor)
        {
            shape.SetColor(spwanConfig.color.RandomInRange);
        }
        else
        {
            for(int i = 0; i <shape.ColorCount; i++)
            {
                shape.SetColor(spwanConfig.color.RandomInRange, i);
            }
        }
       
        shape.AngularVelocity = Random.onUnitSphere*spwanConfig.angularSpeed.RandomValueInRange;

        Vector3 direction;
        switch (spwanConfig.movementDirection)
        {
            case SpwanConfiguration.MovementDirection.Upward:
                direction = transform.up;
                break;
            case SpwanConfiguration.MovementDirection.Outward:
                direction = (t.localPosition - transform.position).normalized;
                break;
            case SpwanConfiguration.MovementDirection.Random:
                direction = Random.onUnitSphere;
                break;
            default:
                direction = transform.forward;
                break;
        }
        
        shape.Velocity = direction * spwanConfig.speed.RandomValueInRange;
        return shape;
    }
   
    
}
