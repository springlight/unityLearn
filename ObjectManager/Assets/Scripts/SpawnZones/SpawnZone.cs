using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZone : GameLevelObject {
    
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
            public bool uniformLifecycles;
        }

        public SatelliteConfiguration satellite;

        [System.Serializable]
        public struct LifecycleConfiguration
        {
            [FloatRangeSlider(0f, 2f)]
            public FloatRange growingDuration;

            [FloatRangeSlider(0f, 2f)]
            public FloatRange dyingDuration;
            [FloatRangeSlider(0f, 100f)]
            public FloatRange adultDuration;

            public Vector3 RandomDuration
            {
                get
                {
                    return new Vector3(growingDuration.RandomValueInRange, adultDuration.RandomValueInRange, dyingDuration.RandomValueInRange);
                }
            }
        }
        public LifecycleConfiguration lifecycle;
       


    }

    [SerializeField]
    SpwanConfiguration spwanConfig;

    //[SerializeField]
    //SpawnMovementDirection spawnMovementDirection;
    //[SerializeField]
    //FloatRange spwanSpeed;

    [SerializeField, Range(0f, 50f)]
    float spawnSpeed;
    float spawnProgress;
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
        // float growingDuraion = spwanConfig.lifecycle.growingDuration.RandomValueInRange;
        Vector3 lifecycleDurations = spwanConfig.lifecycle.RandomDuration;
        int satelliteCnt = spwanConfig.satellite.amount.RandomValueInRange;
        for(int i = 0; i <satelliteCnt; i++)
        {
            CreateSatelliteFor(shape, spwanConfig.satellite.uniformLifecycles ?
                    lifecycleDurations : spwanConfig.lifecycle.RandomDuration);
        }
        SetUpLifecycle(shape, lifecycleDurations);
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
    void CreateSatelliteFor(Shape focalShape, Vector3 lifecycleDurations)
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
        SetUpLifecycle(shape, lifecycleDurations);
    }

    void SetUpLifecycle(Shape shape, Vector3 durations)
    {
        if (durations.x > 0f)
        {
            if (durations.y > 0f || durations.z > 0f)
            {
                shape.AddBehavior<LifecycleShapeBehavior>().Initialize(
                    shape, durations.x, durations.y, durations.z
                );
            }
            else
            {
                shape.AddBehavior<GrowingShapeBehavior>().Initiallize(
                    shape, durations.x
                );
            }
        }
        else if (durations.y > 0f)
        {
            shape.AddBehavior<LifecycleShapeBehavior>().Initialize(
                shape, durations.x, durations.y, durations.z
            );
        }
        else if (durations.z > 0f)
        {
            shape.AddBehavior<DyingShapeBehavior>().Initiallize(
                shape, durations.z
            );
        }
    }

    public override void GameUpdate()
    {
        spawnProgress += Time.deltaTime * spawnSpeed;
        while(spawnProgress >= 1f)
        {
            spawnProgress -= 1f;
            SpawnShape();
        }
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(spawnProgress);
    }

    public override void Load(GameDataReader reader)
    {
        spawnProgress = reader.ReadFloat();
    }
}
