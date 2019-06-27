using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 实现shape scale from zero to the scale that gave it 
/// </summary>
public class GrowingShapeBehavior : ShapeBehavior
{
    Vector3 oriScale;
    float duration,dyingAge;
    public override ShapeBehaviorType BehaviorType { get { return ShapeBehaviorType.Growing; } }

    public override bool GameUpdate(Shape shape)
    {
        float dyingDuration = shape.Age - dyingAge;
        if(dyingDuration < duration)
        {
            float s = 1 - dyingDuration / duration;
            s = (3f - 2f * s) * s * s;//实现平滑增长，只是个数学公式
            shape.transform.localScale = s * oriScale;
            return true;
        }
        shape.Die();
     //   shape.transform.localScale = Vector3.zero;
        return true;
    }

    public override void Load(GameDataReader reader)
    {
        oriScale = reader.ReadVector3();
        duration = reader.ReadFloat();
        dyingAge = reader.ReadFloat();
    }

    public override void Recycle()
    {
        ShapeBehaviorPool<GrowingShapeBehavior>.Reclaim(this);
    }

    public override void Save(GameDataWriter write)
    {
        write.Write(oriScale);
        write.Write(duration);
        write.Write(dyingAge);
    }

    public void Initiallize(Shape shape,float duration)
    {
        oriScale = shape.transform.localScale;
        this.duration = duration;
        dyingAge = shape.Age;
        shape.MarkAsDying();
        //shape.transform.localScale = Vector3.zero;
    }
}
