using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteShapeBehavior : ShapeBehavior
{
    ShapeInstance focalShape;

    Vector3 prePos;
    //  Shape focalShape;
    float frequency;
    Vector3 cosOffset;
    Vector3 sinOffset;
    public override ShapeBehaviorType BehaviorType
    {
        get { return ShapeBehaviorType.Statellite; }
    }

    public override bool GameUpdate(Shape shape)
    {
        if (focalShape.IsValid)
        {
            float t = 2f * Mathf.PI * frequency * shape.Age;
            prePos = shape.transform.localPosition;
            shape.transform.localPosition = focalShape.Shape.transform.localPosition + cosOffset * Mathf.Cos(t) + sinOffset * Mathf.Sin(t);
           
            shape.AddBehavior<MovementShapeBehavior>().Velocity = (shape.transform.localPosition - prePos)/Time.deltaTime;
            return true;
        }
        return false;
    }

    public override void Load(GameDataReader reader)
    {
        focalShape = reader.ReadShapeInstance();
        frequency = reader.ReadFloat();
        cosOffset = reader.ReadVector3();
        sinOffset = reader.ReadVector3();
        prePos = reader.ReadVector3();
    }

    public override void Recycle()
    {
        ShapeBehaviorPool<SatelliteShapeBehavior>.Reclaim(this);
    }

    public override void Save(GameDataWriter write)
    {
        write.Write(focalShape);
        write.Write(frequency);
        write.Write(cosOffset);
        write.Write(sinOffset);
        write.Write(prePos);
    }

    public override void ResolveShapeInstances()
    {
       focalShape.Resolve();
    }

    public void Initialize (Shape shape,Shape focalShape,float radius,float frequency)
    {
        this.focalShape = focalShape;
        this.frequency = frequency;
        Vector3 orbitAxis = Random.onUnitSphere;
        do
        {
            cosOffset = Vector3.Cross(orbitAxis, Random.onUnitSphere).normalized;

        } while (cosOffset.sqrMagnitude < 0.1f);//防止生成的向量过小

        //cosOffset = Vector3.right;
        //sinOffset = Vector3.forward;
        sinOffset = Vector3.Cross(cosOffset, orbitAxis);
        cosOffset *= radius;
        sinOffset *= radius;
        //好好消化这块
        shape.AddBehavior<RotationShapeBehavior>().AngularVelocity =
            -360f * frequency * shape.transform.InverseTransformDirection(orbitAxis);

       
        GameUpdate(shape);
        prePos = shape.transform.localPosition;
    }
}
