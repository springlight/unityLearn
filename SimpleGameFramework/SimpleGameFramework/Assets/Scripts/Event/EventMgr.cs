using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMgr : MgrBase
{
    private EventPool<GlobalEventArgs> m_EventPool;

    public override int Priority
    {
        get
        {
            return 100;
        }
    }

    public EventMgr()
    {
        m_EventPool = new EventPool<GlobalEventArgs>();
    }

    public override void Init()
    {
       
    }

    public override void ShutDown()
    {
        m_EventPool.Shutdown();
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        m_EventPool.Update(elapseSeconds, realElapseSeconds);
    }


    public bool Check(int id,EventHandler<GlobalEventArgs> handler)
    {
        return m_EventPool.Check(id, handler);
    }

    public void Subscribe(int id,EventHandler<GlobalEventArgs> handler)
    {
        m_EventPool.SubScribe(id, handler);
    }

    public void Unsubscribe(int id,EventHandler<GlobalEventArgs> handler)
    {
        m_EventPool.Unsubscribe(id, handler);
    }

    public void Fire(object sender,GlobalEventArgs e)
    {
        m_EventPool.Fire(sender, e);
    }

    public void FireNow(object sender,GlobalEventArgs e)
    {
        m_EventPool.FireNow(sender, e);
    }
}
