using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr : MgrBase
{
    private const int DefaultCapacity = int.MaxValue;

    /// <summary>
    /// 默认对象过期秒数
    /// </summary>
    private const float DefaultExprieTime = float.MaxValue;

    private Dictionary<string, IObjectPool> m_ObjectPools;

    public override int Priority
    {
        get
        {
            return 90;
        }
    }

    public int Cont { get { return m_ObjectPools.Count; } }

    public ObjectPoolMgr()
    {
        m_ObjectPools = new Dictionary<string, IObjectPool>();
    }

    public override void Init()
    {

    }

    public override void ShutDown()
    {
        foreach (IObjectPool objectPool in m_ObjectPools.Values)
        {
            objectPool.Shutdown();
        }
        m_ObjectPools.Clear();
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        foreach (IObjectPool objectPool in m_ObjectPools.Values)
        {
            objectPool.Update(elapseSeconds, realElapseSeconds);
        }
    }

    public bool HasObjectPool<T>()where T : ObjectBase
    {
        return m_ObjectPools.ContainsKey(typeof(T).FullName);
    }


    public ObjectPool<T> CreateObjectPool<T>(int capacity = DefaultCapacity,float exprireTime = DefaultExprieTime,bool allowMultiSpawn = false) where T : ObjectBase
    {
        string name = typeof(T).FullName;
        if (HasObjectPool<T>())
        {
            return null;
        }

        ObjectPool<T> objectPool = new ObjectPool<T>(name, capacity, exprireTime, allowMultiSpawn);
        m_ObjectPools.Add(name, objectPool);
        return objectPool;
    }

    public ObjectPool<T> GetObjectPool<T>() where T : ObjectBase
    {
        IObjectPool objectPool = null;
        m_ObjectPools.TryGetValue(typeof(T).FullName, out objectPool);
        return objectPool as ObjectPool<T>;
    }
    /// <summary>
    /// 销毁对象池
    /// </summary>
    public bool DestroyObjectPool<T>()
    {
        IObjectPool objectPool = null;
        if (m_ObjectPools.TryGetValue(typeof(T).FullName, out objectPool))
        {
            objectPool.Shutdown();
            return m_ObjectPools.Remove(typeof(T).FullName);
        }

        return false;
    }
    /// <summary>
    /// 释放所有对象池中的可释放对象。
    /// </summary>
    public void Release()
    {
        foreach (IObjectPool objectPool in m_ObjectPools.Values)
        {
            objectPool.Release();
        }
    }

    /// <summary>
    /// 释放所有对象池中的未使用对象。
    /// </summary>
    public void ReleaseAllUnused()
    {
        foreach (IObjectPool objectPool in m_ObjectPools.Values)
        {
            objectPool.ReleaseAllUnused();
        }
    }

}
