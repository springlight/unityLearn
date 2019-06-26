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

        public MovementDirection oscillationDirection;

        public FloatRange oscillationAmplitude;

        public FloatRange oscillationFrequency;
        [System.Serializable]
        public struct SatelliteConfiguration
        {
            public IntRange amount;

            [FloatRangeSlider(0.1f, 1f)]
            public FloatRange relativeScale;
            public FloatRange orbitRadius;//半径
            public FloatRange orbitFrequency;//速度
        }

        public SatelliteConfiguration satellite;

        [System.Serializable]
        public struct LifecycleConfiguration
        {
            [FloatRangeSlider(0f, 2f)]
            public FloatRange growingDuration;
        }
        public LifecycleConfiguration lifecycle;
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
    public virtual void SpawnShape()
    {
        int factoryIdx = Random.Range(0, spwanConfig.factories.Length);
        Shape shape = spwanConfig.factories[factoryIdx].GetRandom();
        Transform t = shape.transform;

        t.localPosition = SpawnPoint;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * spwanConfig.scale.RandomValueInRange;
        // shape.SetColor(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.25f, 1f, 1f, 1f));
        //   shape.AngularVelocity = Random.onUnitSphere * Random.Range(5f, 90f);
        SetupColor(shape);
        float angularSpeed = spwanConfig.angularSpeed.RandomValueInRange;
        if (angularSpeed != 0f)
        {
            var rotation = shape.AddBehavior<RotationShapeBehavior>();

            rotation.AngularVelocity = Random.onUnitSphere * angularSpeed;
        }


        float speed = spwanConfig.speed.RandomValueInRange;
        if (speed != 0f)
        {

            var movement = shape.AddBehavior<MovementShapeBehavior>();

            movement.Velocity = GetDirectionVector(spwanConfig.movementDirection, t) * speed;
        }


        SetupOscillation(shape);
        float growingDuraion = spwanConfig.lifecycle.growingDuration.RandomValueInRange;
        int satelliteCnt = spwanConfig.satellite.amount.RandomValueInRange;
        for(int i = 0; i <satelliteCnt; i++)
        {
            CreateSatelliteFor(shape, growingDuraion);
        }
        SetUpLifecycle(shape, growingDuraion);
        // return shape;
    }

    private void SetupColor(Shape shape)
    {
        if (spwanConfig.uniformColor)
        {
            shape.SetColor(spwanConfig.color.RandomInRange);
        }
        else
        {
            for (int i = 0; i < shape.ColorCount; i++)
            {
                shape.SetColor(spwanConfig.color.RandomInRange, i);
            }
        }
    }

    void SetupOscillation(Shape shape)
    {
        float amplitude = spwanConfig.oscillationAmplitude.RandomValueInRange;
        float frequency = spwanConfig.oscillationFrequency.RandomValueInRange;
        if (amplitude == 0f || frequency == 0f)
        {
            return;
        }
        var oscillation = shape.AddBehavior<OscillationShapeBehavior>();
        oscillation.Offset = GetDirectionVector(
            spwanConfig.oscillationDirection, shape.transform
        ) * amplitude;
        oscillation.Frequency = frequency;
    }

    Vector3 GetDirectionVector(SpwanConfiguration.MovementDirection direction, Transform t)
    {
        switch (direction)
        {
            case SpwanConfiguration.MovementDirection.Upward:
                return transform.up;
            case SpwanConfiguration.MovementDirection.Outward:
                return (t.localPosition - transform.position).normalized;
            case SpwanConfiguration.MovementDirection.Random:
                return Random.onUnitSphere;
            default:
                return transform.forward;
        }
    }
    /// <summary>
    /// 添加卫星
    /// </summary>
    /// <param name="focalShape"></param>
    void CreateSatelliteFor(Shape focalShape,float growingDuration)
    {
        int factoryIdx = Random.Range(0, spwanConfig.factories.Length);
        Shape shape = spwanConfig.factories[factoryIdx].GetRandom();
        Transform t = shape.transform;
        t.localRotation = Random.rotation;

        t.localScale = focalShape.transform.localScale * spwanConfig.satellite.relativeScale.RandomValueInRange;
        //t.localPosition = focalShape.transform.localPosition + Vector3.up;
        //shape.AddBehavior<MovementShapeBehavior>().Velocity = Vector3.up;
        SetupColor(shape);
        shape.AddBehavior<SatelliteShapeBehavior>().Initialize(shape,focalShape,spwanConfig.satellite.orbitRadius.RandomValueInRange,spwanConfig.satellite.orbitFrequency.RandomValueInRange);
        SetUpLifecycle(shape, growingDuration);
    }
    /// <summary>
    /// 从scale 0到配置大小逐渐缩放
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="growingDuration"></param>
    void SetUpLifecycle(Shape shape,float growingDuration)
    {
        if(growingDuration > 0f)
        {
            shape.AddBehavior<GrowingShapeBehavior>().Initiallize(shape, growingDuration);
        }
    }
}
