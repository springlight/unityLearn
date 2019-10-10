using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    /// <summary>
    /// 对象池名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 对象池对象类型
    /// </summary>
    Type ObjectType { get; }
    /// <summary>
    /// 对象池中对象的数量
    /// </summary>
    int Count { get; }

    /// <summary>
    /// 对象池中能被释放的对象的数量
    /// </summary>
    int CanReleaseCount { get; }

    /// <summary>
    /// 对象池自动释放可释放对象的间隔秒数（隔几秒进行一次自动释放）
    /// </summary>
    float AutoReleaseInterval { get; set; }

    /// <summary>
    /// 对象池的容量
    /// </summary>
    int Capacity { get; set; }


    /// <summary>
    /// 对象池对象过期秒数（被回收几秒钟视为过期，需要被释放）
    /// </summary>
    float ExprieTime { get; set; }

    /// <summary>
    /// 释放超出对象池容量的可释放对象
    /// </summary>
    void Release();

    void Release(int toReleaseCount);

    void ReleaseAllUnused();

    void Update(float elapseSec, float realElapseSec);


    void Shutdown();
}
