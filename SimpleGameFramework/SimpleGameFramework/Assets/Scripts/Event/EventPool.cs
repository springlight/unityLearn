using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// ventPool负责将某一类型的GlobalEventArgs封装后进行管理
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventPool<T> where T:GlobalEventArgs
{
    /// <summary>
    /// 事件结点
    /// </summary>
    private class Event
    {
        public Event(object sender ,T e)
        {
            Sender = sender;
            EventArgs = e;
        }


        public object Sender { get; private set; }
        public T EventArgs{ get; private set; }
    }

    private Queue<Event> m_Events;

    private Dictionary<int, EventHandler<T>> m_EventHanlders;

    public EventPool()
    {
        m_EventHanlders = new Dictionary<int, EventHandler<T>>();
        m_Events = new Queue<Event>();
    }



    public bool Check(int id,EventHandler<T> hander)
    {
        if(hander == null)
        {
            Debug.LogError("事件处理方法为空");
            return false;
        }

        EventHandler<T> handlers = null;
        if(!m_EventHanlders.TryGetValue(id,out handlers))
        {
            return false;
        }

        if (handlers == null) return false;

        ///遍历委托里所有的方法
        foreach(EventHandler<T> i in handlers.GetInvocationList())
        {
            if (i == hander) return true;
        }

        return false;
    }


    public void SubScribe(int id,EventHandler<T> handler)
    {
        if (handler == null) return;
        EventHandler<T> evtHandler = null;
        if(!m_EventHanlders.TryGetValue(id,out evtHandler)|| evtHandler == null)
        {
            m_EventHanlders[id] = handler;
        }

        else if (Check(id, handler))
        {
            Debug.LogError("要订阅的事件处理方法已经存在");
        }
        else
        {
            evtHandler += handler;
            m_EventHanlders[id] = evtHandler;
        }
    }


    public void Unsubscribe(int id,EventHandler<T> handler)
    {
        if (handler!= null && m_EventHanlders.ContainsKey(id))
        {
            m_EventHanlders[id] -= handler;
        }
    }


    private void HanldeEvent(object sender,T e)
    {
        int evtId = e.Id;
        EventHandler<T> handlers = null;
        if(m_EventHanlders.TryGetValue(evtId,out handlers))
        {
            if(handlers != null)
            {
                handlers(sender, e);
            }
            else
            {
                Debug.LogError("事件没有对应处理方法：" + evtId);

            }

        }
        ///享引用池归还事件引用
        ReferencePool.Release(e);
    }

    /// <summary>
    /// 事件池轮询（用于处理线程安全的事件）
    /// </summary>

    public void Update(float elapseSec,float realElaspeSec)
    {
        while(m_Events.Count > 0)
        {
            Event e = null;
            lock (m_Events)
            {
                e = m_Events.Dequeue();
            }
            HanldeEvent(e.Sender, e.EventArgs);
        }
    }

    /// <summary>
    /// 抛出事件（线程安全）
    /// </summary>
    /// <param name="sender">事件源。</param>

    public void Fire(object sender,T e)
    {
        Event evtNode = new Event(sender, e);
        lock (m_Events)
        {
            m_Events.Enqueue(evtNode);
        }
    }


    /// <summary>
        /// 抛出事件（线程不安全）
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
    public void FireNow(object sender, T e)
    {
        HanldeEvent(sender, e);
    }
    /// <summary>
    /// 清理事件。
    /// </summary>
    public void Clear()
    {
        lock (m_Events)
        {
            m_Events.Clear();
        }
    }

    /// <summary>
    /// 关闭并清理事件池。
    /// </summary>
    public void Shutdown()
    {
        Clear();
        m_EventHanlders.Clear();
    }



}
