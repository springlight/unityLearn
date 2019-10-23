using FlappyBird;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdData : EntityData
{
    public float FlyForce { get; private set; }

    public BirdData(int entityId,int typeId,float flyForce) : base(entityId, typeId)
    {
        FlyForce = flyForce;
    }
}
