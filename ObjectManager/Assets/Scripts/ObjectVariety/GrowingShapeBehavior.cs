using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 实现shape scale from zero to the scale that gave it 
/// </summary>
public class DyingShapeBehavior : ShapeBehavior
{
    Vector3 oriScale;
    float duration;
    public override ShapeBehaviorType BehaviorType { get { return ShapeBehaviorType.Dying; } }

    public override bool GameUpdate(Shape shape)
    {
        if(shape.Age < duration)
        {
            float s = shape.Age / duration;
            s = (3f - 2f * s) * s * s;//实现平滑增长，只是个数学公式
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
        ShapeBehaviorPool<DyingShapeBehavior>.Reclaim(this);
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
