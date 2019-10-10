using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对象池基类
/// 所有需要被对象池管理的对象都需要继承该类
/// </summary>
public abstract class ObjectBase
{
    /// <summary>
    /// 对象名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 对象(实际使用的对象放到这里)
    /// </summary>
    public object Target { get; set; }


    public DateTime LastUseTime { get; private set; }
    /// <summary>
    /// 对象的获取计数
    /// </summary>
    public int SpawnCount { get; set; }
    /// <summary>
    /// 对象是否在使用
    /// </summary>
    public bool IsInUse
    {
        get
        {
            return SpawnCount > 0;
        }
    }

    public ObjectBase(object target,string name = "")
    {
        Name = name;
        Target = target;
    }

    /// <summary>
    /// 获取对象时
    /// </summary>
    protected virtual void OnSpawn()
    {

    }
    /// <summary>
    /// 回收对象时
    /// </summary>
    protected virtual void OnUnspawn()
    {

    }

    /// <summary>
    /// 释放对象时
    /// </summary>
    public abstract void Release();
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <returns></returns>

    public ObjectBase Spawn()
    {
        SpawnCount++;
        LastUseTime = DateTime.Now;
        OnSpawn();
        return this;
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    public void Unspawn()
    {
        OnUnspawn();
        LastUseTime = DateTime.Now;
        SpawnCount--;
    }


}
