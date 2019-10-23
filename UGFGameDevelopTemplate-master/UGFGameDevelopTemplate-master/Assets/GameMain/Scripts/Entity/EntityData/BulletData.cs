using FlappyBird;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : EntityData {
    /// <summary>
    /// 发射位置
    /// </summary>
    public Vector2 ShootPos { get; private set; }
    public float FlySpeed { get; private set; }
    public BulletData(int entityId,int typeId,Vector2 shootpos,float flyspeed) : base(entityId, typeId)
    {
        ShootPos = shootpos;
        FlySpeed = flyspeed;
    }
}
