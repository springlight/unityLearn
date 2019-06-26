using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 实现shape scale from zero to the scale that gave it 
/// </summary>
public class GrowingShapeBehavior : ShapeBehavior
{
    Vector3 oriScale;
    float duration;
    public override ShapeBehaviorType BehaviorType { get { return ShapeBehaviorType.Growing; } }

    public override bool GameUpdate(Shape shape)
    {
        if(shape.Age < duration)
        {
            float s = shape.Age / duration;
            shape.transform.localScale = s * oriScale;
            return true;        
        }
        shape.transform.localScale = oriScale;
        return false;
    }

    public override void Load(GameDataReader reader)
    {
        oriScale = reader.ReadVector3();
        duration = reader.ReadFloat();
    }

    public override void Recycle()
    {
        ShapeBehaviorPool<GrowingShapeBehavior>.Reclaim(this);
    }

    public override void Save(GameDataWriter write)
    {
        write.Write(oriScale);
        write.Write(duration);
    }

    public void Initiallize(Shape shape,float duration)
    {
        oriScale = shape.transform.localScale;
        this.duration = duration;
        shape.transform.localScale = Vector3.zero;
    }
}
