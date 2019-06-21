using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteShapeBehavior : ShapeBehavior
{
    Shape focalShape;
    float frequency;
    Vector3 cosOffset;
    Vector3 sinOffset;
    public override ShapeBehaviorType BehaviorType
    {
        get { return ShapeBehaviorType.Statellite; }
    }

    public override void GameUpdate(Shape shape)
    {
        float t = 2f * Mathf.PI * frequency * shape.Age;
        shape.transform.localPosition = focalShape.transform.localPosition + cosOffset * Mathf.Cos(t) + sinOffset * Mathf.Sin(t);
    }

    public override void Load(GameDataReader reader)
    {
        
    }

    public override void Recycle()
    {
        ShapeBehaviorPool<SatelliteShapeBehavior>.Reclaim(this);
    }

    public override void Save(GameDataWriter write)
    {
       
    }


    public void Initialize (Shape shape,Shape focalShape,float radius,float frequency)
    {
        this.focalShape = focalShape;
        this.frequency = frequency;
        cosOffset = Vector3.right;
        sinOffset = Vector3.forward;
        cosOffset *= radius;
        sinOffset *= radius;

        GameUpdate(shape);
    }
}
