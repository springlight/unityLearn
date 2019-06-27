using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifecycleShapeBehavior : ShapeBehavior
{
    float adultDuration, dyingDuration, dyingAge;

    public override ShapeBehaviorType BehaviorType { get { return ShapeBehaviorType.Growing; } }

    public override bool GameUpdate(Shape shape)
    {
        if (shape.Age >= dyingAge)
        {
            if (dyingDuration <= 0f)
            {
                shape.Die();
                return true;
            }
            if (!shape.IsMarkedAsDying)
            {
                shape.AddBehavior<DyingShapeBehavior>().Initiallize(
                    shape, dyingDuration + dyingAge - shape.Age
                );
            }
            return false;
        }
        return true;
    }

    public override void Load(GameDataReader reader)
    {
        adultDuration = reader.ReadFloat();
        dyingDuration = reader.ReadFloat();
        dyingAge = reader.ReadFloat();
    }

    public override void Recycle()
    {
        ShapeBehaviorPool<LifecycleShapeBehavior>.Reclaim(this);
    }

    public override void Save(GameDataWriter write)
    {
        write.Write(adultDuration);
        write.Write(dyingDuration);
        write.Write(dyingAge);
    }

    public void Initialize(
         Shape shape,
         float growingDuration, float adultDuration, float dyingDuration
     )
    {
        //originalScale = shape.transform.localScale;
        //this.duration = duration;
        this.adultDuration = adultDuration;
        this.dyingDuration = dyingDuration;
        dyingAge = growingDuration + adultDuration;

        if (growingDuration > 0f)
        {
            shape.AddBehavior<GrowingShapeBehavior>().Initiallize(
                shape, growingDuration
            );
        }
    }
}
