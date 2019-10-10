using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 释放对象筛选方法
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="candidateObjects">要筛选的对象集合</param>
/// <param name="toReleaseCount">需要释放的对象数量</param>
/// <param name="expireTime">对象过期参考时间</param>
/// <returns>经筛选需要释放的对象集合</returns>
public delegate LinkedList<T> ReleaseObjectFilterCallback<T>(LinkedList<T> candidateObjects, int toReleaseCount, DateTime expireTime) where T : ObjectBase;
public class ObjectPool<T> : IObjectPool where T : ObjectBase
{

    private int m_Capacity;
    /// <summary>
    /// 对象池过期秒数
    /// </summary>
    private float m_ExpireTime;
    public string Name { get; private set; }


    private LinkedList<ObjectBase> m_Objects;

    public Type ObjectType
    {
        get { return typeof(T); }
    }

    public int Count { get { return m_Objects.Count; } }
    /// <summary>
    /// 池对象是否可多次获取
    /// </summary>

    public bool AllowMultiSpawn{ get; private set; }

    public int CanReleaseCount
    {
        get
        {
            return GetCanReleaseObjects().Count;
        }
    }


    public float AutoReleaseTime { get; private set; }

    public float AutoReleaseInterval { get; set; }


    public int Capacity
    {
        get { return m_Capacity; }
        set
        {
            if(value > 0 &&m_Capacity != value)
            {
                m_Capacity = value;
            }
        }
    }


    public float ExprieTime
    {
        get
        {
            return m_ExpireTime;
        }
        set
        {
            if(value >0 && m_ExpireTime != value)
            {
                m_ExpireTime = value;
            }
        }

    }


    public ObjectPool(string name,int capacity,float expireTime,bool allowMultiSpawn)
    {
        Name = name;
        m_Objects = new LinkedList<ObjectBase>();
        Capacity = capacity;
        AutoReleaseInterval = expireTime;
        ExprieTime = expireTime;
        AutoReleaseTime = 0f;
        AllowMultiSpawn = allowMultiSpawn;
    }
    /// <summary>
    /// 检查对象
    /// </summary>
    /// <param name="name">对象名称</param>
    /// <returns>要检查的对象是否存在</returns>
    public bool CanSpawn(string name)
    {
        foreach(ObjectBase obj in m_Objects)
        {
            if(obj.Name != name)
            {
                continue;
            }
            if(AllowMultiSpawn || !obj.IsInUse)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 注册对象
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="spawned"></param>
    public void Register(T obj,bool spawned = false)
    {
        //已被获取，就让计数 +1
        if (spawned)
        {
            obj.SpawnCount++;
        }
        m_Objects.AddLast(obj);
    }
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Spawn(string name = "")
    {
        foreach(ObjectBase obj in m_Objects)
        {
            if(obj.Name != name)
            {
                continue;
            }

            if(AllowMultiSpawn || !obj.IsInUse)
            {
                Debug.Log("获取了对象：" + typeof(T).FullName + "/" + obj.Name);
                return obj.Spawn() as T;
            }
        }
        return null;
    }
    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="obj"></param>
    public void Unspawn(ObjectBase obj)
    {
        Unspawn(obj.Target);
    }

    public void Unspawn(object target)
    {
        foreach(ObjectBase obj in m_Objects)
        {
            if(obj.Target == target)
            {
                obj.Unspawn();
                Debug.Log("对象被回收了：" + typeof(T).FullName + "/" + obj.Name);

                return;
            }
        }
        Debug.LogError("找不到要回收的对象：" + typeof(object).FullName);

    }
    /// <summary>
    /// 获取所有可释放的对象
    /// </summary>
    /// <returns></returns>

    private LinkedList<T> GetCanReleaseObjects()
    {
        LinkedList<T> canReleaseObjects = new LinkedList<T>();
        foreach(ObjectBase obj in m_Objects)
        {
            if (!obj.IsInUse)
            {
                canReleaseObjects.AddLast(obj as T);
            }
        }
        return canReleaseObjects;
    }


    /// <summary>
    /// 释放对象池中的可释放资源
    /// </summary>
    /// <param name="toReleaseCount">尝试释放的对象数量</param>
    /// <param name="releaseObjectFilterCallback">释放的筛选方法</param>
    public void Release(int toReleaseCount,ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
    {
        AutoReleaseTime = 0;
        if (toReleaseCount <= 0) return;

        //计算对象过期参考时间
        DateTime expireTime = DateTime.MinValue;
        if(m_ExpireTime < float.MaxValue)
        {
            //当前时间-过期秒数 = 过期参考时间
            expireTime = DateTime.Now.AddSeconds(-m_ExpireTime);
        }
        //获取可释放的对象和实际要释放的对象
        LinkedList<T> canReleaseObjects = GetCanReleaseObjects();
        LinkedList<T> toReleaseObjects = releaseObjectFilterCallback(canReleaseObjects, toReleaseCount, expireTime);

        if (toReleaseObjects == null || toReleaseObjects.Count <= 0) return;

        //遍历实际要释放的对象
        foreach(ObjectBase toReleaseObject in toReleaseObjects)
        {
            if (toReleaseObject == null) continue;
            foreach(ObjectBase obj in m_Objects)
            {
                if (obj != toReleaseObject) continue;
                m_Objects.Remove(obj);
                obj.Release();
                break;
            }
           
        }
    }


    /// <summary>
    /// 默认的释放对象筛选方法（未被使用且过期的对象）
    /// </summary>
    private LinkedList<T> DefaultReleaseObjectFilterCallBack(LinkedList<T> candidateObjects, int toReleaseCount, DateTime expireTime)
    {
        LinkedList<T> toReleaseObjects = new LinkedList<T>();

        if (expireTime > DateTime.MinValue)
        {
            LinkedListNode<T> current = candidateObjects.First;
            while (current != null)
            {
                //对象最后使用时间 <= 过期参考时间，就需要释放
                if (current.Value.LastUseTime <= expireTime)
                {
                    toReleaseObjects.AddLast(current.Value);
                    LinkedListNode<T> next = current.Next;
                    candidateObjects.Remove(current);
                    toReleaseCount--;
                    if (toReleaseCount <= 0)
                    {
                        return toReleaseObjects;
                    }
                    current = next;
                    continue;
                }

                current = current.Next;
            }

        }

        return toReleaseObjects;
    }

    /// <summary>
    /// 释放超出对象池容量的可释放对象
    /// </summary>
    public void Release()
    {
        Release(m_Objects.Count - m_Capacity, DefaultReleaseObjectFilterCallBack);
    }

    /// <summary>
    /// 释放指定数量的可释放对象
    /// </summary>
    /// <param name="toReleaseCount"></param>
    public void Release(int toReleaseCount)
    {
        Release(toReleaseCount, DefaultReleaseObjectFilterCallBack);
    }


    public void ReleaseAllUnused()
    {
        LinkedListNode<ObjectBase> cur = m_Objects.First;
        while(cur != null)
        {
            if (cur.Value.IsInUse)
            {
                cur = cur.Next;
                continue;
            }
            LinkedListNode<ObjectBase> next = cur.Next;
            m_Objects.Remove(cur);
            cur.Value.Release();
            cur = next;
        }
    }

    /// <summary>
    /// 对象池的定时释放
    /// </summary>
    public void Update(float elapseSeconds, float realElapseSeconds)
    {
        AutoReleaseTime += realElapseSeconds;
        if (AutoReleaseTime < AutoReleaseInterval)
        {
            return;
        }

        Release();
    }

    /// <summary>
    /// 清理对象池
    /// </summary>
    public void Shutdown()
    {
        LinkedListNode<ObjectBase> current = m_Objects.First;
        while (current != null)
        {

            LinkedListNode<ObjectBase> next = current.Next;
            m_Objects.Remove(current);
            current.Value.Release();
            Debug.Log("对象被释放了：" + current.Value.Name);
            current = next;
        }
    }



}
