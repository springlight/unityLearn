using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeBehaviorType
{
    Movement,
    Rotation,
    Oscillation,
    Statellite
}



public static class ShapeBehaviorTypeMethods
{
    public static ShapeBehavior GetInstance (this ShapeBehaviorType type)
    {
        switch (type)
        {
            case ShapeBehaviorType.Movement:
                return ShapeBehaviorPool<MovementShapeBehavior>.Get();
            case ShapeBehaviorType.Rotation:
                return ShapeBehaviorPool<RotationShapeBehavior>.Get();
            case ShapeBehaviorType.Oscillation:
                return ShapeBehaviorPool<OscillationShapeBehavior>.Get();
            case ShapeBehaviorType.Statellite:
                return ShapeBehaviorPool< SatelliteShapeBehavior>.Get();
                
        }
        return null;
    }
}

public abstract class ShapeBehavior
#if UNITY_EDITOR
    : ScriptableObject
#endif
{

   
    public abstract void GameUpdate(Shape shape);
    public abstract void Save(GameDataWriter write);
    public abstract void Load(GameDataReader reader);
    public abstract ShapeBehaviorType BehaviorType { get; }

    public abstract void Recycle();
#if UNITY_EDITOR
    public bool IsReclaimed { get; set; }

    private void OnEnable()
    {
        if (IsReclaimed)
        {
            Recycle();
        }
    }
#endif
}


