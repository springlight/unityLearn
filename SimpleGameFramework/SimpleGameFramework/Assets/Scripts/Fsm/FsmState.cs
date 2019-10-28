
using System;
using System.Collections.Generic;

public delegate void FsmEventHanlder<T>(Fsm<T> fsm, object sender, object userData) where T : class;
///
public class FsmState<T> where T:class
{
    /// <summary>
    /// 每一个状态都有需要监听的事件
    /// </summary>
    private Dictionary<int, FsmEventHanlder<T>> m_EvtHandlers = new Dictionary<int, FsmEventHanlder<T>>();
    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="evtId"></param>
    /// <param name="evtHanlder"></param>
    protected void SubscribeEvent(int evtId,FsmEventHanlder<T> evtHanlder)
    {
        if (evtHanlder == null) return;
        if (!m_EvtHandlers.ContainsKey(evtId))
        {
            m_EvtHandlers[evtId] = evtHanlder;
        }
        else
        {
            m_EvtHandlers[evtId] += evtHanlder;
        }
    }
    /// <summary>
    /// 取消事件
    /// </summary>
    /// <param name="evtId"></param>
    /// <param name="evtHandler"></param>
    protected void UnsubscribeEvent(int evtId,FsmEventHanlder<T> evtHandler)
    {
        if (evtHandler == null) return;
        if (m_EvtHandlers.ContainsKey(evtId))
        {
            m_EvtHandlers[evtId] -= evtHandler;
        }
    }
    /// <summary>
    /// 相应状态事件
    /// </summary>
    /// <param name="fsm"></param>
    /// <param name="sender"></param>
    /// <param name="evtId"></param>
    /// <param name="userData"></param>
    public void OnEvent(Fsm<T> fsm,object sender,int evtId,object userData)
    {
        FsmEventHanlder<T> evtHandlers = null;
        if(m_EvtHandlers.TryGetValue(evtId,out evtHandlers))
        {
            if(evtHandlers != null)
            {
                evtHandlers(fsm, sender, userData);
            }
        }
    }

    /// <summary>
    /// 初始化状态机
    /// </summary>
    /// <param name="fsm"></param>
    public virtual void OnInit(Fsm<T> fsm)
    {

    }
    /// <summary>
    /// 状态机进入时调用
    /// </summary>
    /// <param name="fsm"></param>
    public virtual void OnEnter(Fsm<T> fsm)
    {

    }
    /// <summary>
    /// 状态机轮询时调用
    /// </summary>
    /// <param name="fsm"></param>
    /// <param name="elapseSec"></param>
    /// <param name="realElapseSec"></param>
    public virtual void OnUpdate(Fsm<T> fsm,float elapseSec,float realElapseSec)
    {

    }
    /// <summary>
    /// 状态机离开时调用
    /// </summary>
    /// <param name="fsm"></param>
    /// <param name="isShutdown"></param>
    public virtual void OnLeave(Fsm<T> fsm,bool isShutdown)
    {

    }

    /// <summary>
    /// 状态机销毁时调用
    /// </summary>
    /// <param name="fsm"></param>
    public virtual void OnDestroy(Fsm<T> fsm)
    {
        m_EvtHandlers.Clear();
    }
    //protected void ChangeState(FsmState<T> state)
    //{

    //}
    protected void ChangeState<TState>(Fsm<T> fsm) where TState : FsmState<T>
    {
        ChangeState(fsm, typeof(TState));
    }

    protected void ChangeState(Fsm<T> fsm,Type type)
    {
        fsm.ChangeState(type);
    }

}
