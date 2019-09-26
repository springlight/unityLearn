using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameWorkEntry : ScripSingleton<FrameWorkEntry>
{
    /// <summary>
    /// 所有模块管理器的链表
    /// </summary>
    private LinkedList<MgrBase> m_Mgrs = new LinkedList<MgrBase>();

    private void Update()
    {
        foreach(MgrBase mgr in m_Mgrs)
        {
            mgr.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }
    }
    /// <summary>
    /// 清理所有的Mgr
    /// </summary>
    private void OnDestroy()
    {
        for(LinkedListNode<MgrBase> cur = m_Mgrs.Last;cur != null;cur = cur.Previous)
        {
            cur.Value.ShutDown();
        }
        m_Mgrs.Clear();
    }
    /// <summary>
    /// 对外提供找到指定管理器的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetMgr<T>()where T : MgrBase
    {
        Type mgrType = typeof(T);
        foreach(MgrBase mgr in m_Mgrs)
        {
            if(mgr.GetType() == mgrType)
            {
                return mgr as T;
            }
        }
        return CreateMgr(mgrType) as T;
    }

    private MgrBase CreateMgr(Type mgrType)
    {
        MgrBase mgr = (MgrBase)Activator.CreateInstance(mgrType);
        if(mgr == null)
        {
            Debug.LogError("模块管理器创建失败：" + mgr.GetType().FullName);
        }
        //根据模块优先级决定它在链表里的位置
        LinkedListNode<MgrBase> cur = m_Mgrs.First;
        while(cur != null){
            if(mgr.Priority > cur.Value.Priority)
            {
                break;
            }
            cur = cur.Next;
        }
        if(cur != null)
        {
            m_Mgrs.AddBefore(cur, mgr);
        }
        else
        {
            m_Mgrs.AddLast(mgr);
        }
        //初始化模块管理器
        mgr.Init();
        return mgr;
    }
}
