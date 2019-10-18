using FlappyBird;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgData : EntityData {


    public float MoveSpeed { get; private set; }
    /// <summary>
    /// 到达此目的时，产生新的实体
    /// </summary>
    public float SpawnTarget { get; private set; }
    /// <summary>
    /// 到达次目标是隐藏自身
    /// </summary>
    public float HideTarget { get; private set; }
    /// <summary>
    /// 移动起始点
    /// </summary>
    public float StartPos { get;private set; }
	public BgData(int entityId,int typeId,float moveSpeed,float startPos) : base(entityId, typeId)
    {
        MoveSpeed = moveSpeed;
        SpawnTarget = -8.66f;
        HideTarget = -26.4f;
        StartPos = startPos;
    }
}
