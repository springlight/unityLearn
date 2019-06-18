using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationShapeBehavior : ShapeBehavior
{
    public Vector3 AngularVelocity { get; set; }
    public override void GameUpdate(Shape shape)
    {
        shape.transform.Rotate(AngularVelocity * Time.deltaTime);
    }

    public override void Load(GameDataReader reader)
    {
        AngularVelocity = reader.ReadVector3();
    }

    public override void Save(GameDataWriter write)
    {
        write.Write(AngularVelocity);
    }
}
