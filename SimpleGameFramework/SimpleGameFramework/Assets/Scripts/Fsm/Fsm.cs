

using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 状态机Fsm管理状态FsmState,一个状态机有很多状态
/// </summary>
/// <typeparam name="T"></typeparam>
public class Fsm<T> : IFsm where T : class
{


    public Fsm(string name,T owner,params FsmState<T> [] states)
    {
        if(owner == null)
        {
            Debug.LogError("状态机持有者为空");
        }
        if(states == null || states.Length < 1)
        {
            Debug.LogError("没有要添加进状态机的状态");
        }
        Name = name;
        Owner = owner;
        m_States = new Dictionary<string, FsmState<T>>();
        m_Datas = new Dictionary<string, object>();
        foreach(FsmState<T> state in states)
        {
            if(state == null)
            {
                Debug.LogError("要添加进状态机的状态为空");
                break;
            }

            string stateName = state.GetType().FullName;
            Debug.LogError("状态机full name is ---" + stateName);
            if (m_States.ContainsKey(stateName))
            {
                Debug.LogError("要加入的状态机已经存在" + stateName);
            }
            m_States.Add(stateName, state);
            state.OnInit(this);
        }
        CurStateTime = 0.0f;
        CurState = null;
        IsDestroy = false;
    }
    public string Name { get; private set; }
  

    public Type OwnerType
    {
        get { return typeof(T); }
    }

    public bool IsDestroy { get; private set; }

    public float CurStateTime { get; private set; }


    /// <summary>
    /// 状态机里所有状态的词典
    /// </summary>
    private Dictionary<string, FsmState<T>> m_States;

    private Dictionary<string ,object> m_Datas;

    public FsmState<T> CurState { get; private set; }
    /// <summary>
    /// 状态持有者
    /// </summary>
    public T Owner { get; private set; }



    public void Shutdown()
    {
        if(CurState != null)
        {
            CurState.OnLeave(this, true);
            CurState = null;
            CurStateTime = 0.0f;
        }

        foreach(KeyValuePair<string,FsmState<T>> state in m_States)
        {
            state.Value.OnDestroy(this);
        }
        m_States.Clear();
        m_Datas.Clear();
        IsDestroy = true;
    }

    public void Update(float elapseSec, float realElapseSec)
    {
        if (CurState == null) return;
        CurStateTime += elapseSec;
        CurState.OnUpdate(this, elapseSec, realElapseSec);
    }

    /// <summary>
    /// 获取状态
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <returns></returns>
    public TState GetState<TState>() where TState : FsmState<T>
    {
        return GetState(typeof(TState)) as TState;
    }

    private FsmState<T> GetState(Type stateType)
    {
        if (stateType == null) return null;
        if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
        {
            return null;
        }

        FsmState<T> state = null;
        if(m_States.TryGetValue(stateType.FullName,out state))
        {
            return state;
        }
        return null;
    }

    /// <summary>
    /// 切换状态机状态
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public void ChangeState<TState>() where TState : FsmState<T>
    {
        ChangeState(typeof(TState));
    }

    public void ChangeState(Type type)
    {
        if (CurState == null) return;

        FsmState<T> state = GetState(type);
        if (state == null) return;
        CurState.OnLeave(this, false);
        CurStateTime = 0.0f;
        CurState = state;
        CurState.OnEnter(this);
    }

    /// <summary>
    /// 开启状态机
    /// </summary>
    /// <typeparam name="TStart"></typeparam>
    public void Start<TState>()where TState : FsmState<T>
    {
        Start(typeof(TState));
    }

    public void Start(Type stateType)
    {
        if(CurState != null)
        {
            Debug.LogError("状态机已经开始，无法再次开始");
            return;
        }
        if(stateType == null)
        {
            Debug.LogError("要开始的状态机为空，无法开启");
            return;
        }
        FsmState<T> state = GetState(stateType);
        if (state == null) return;

        CurStateTime = 0.0f;
        CurState = state;
        CurState.OnEnter(this);
    }

    /// <summary>
    /// 抛出状态机事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="evtId"></param>
    public void FrieEvent(object sender,int evtId)
    {
        if (CurState == null) return;
        CurState.OnEvent(this, sender, evtId, null);
    }



    public bool HasData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }
        return m_Datas.ContainsKey(name);
    }
    /// <summary>
    /// 获取状态机数据
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public TData GetData<TData>(string name)
    {
        return (TData)GetData(name);
    }

    public object GetData(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        object data = null;

        m_Datas.TryGetValue(name, out data);
        return data;
    }
    /// <summary>
    /// 设置状态机数据
    /// </summary>
    /// <param name="name"></param>
    /// <param name="data"></param>
    public void SetData(string name,object data)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        m_Datas[name] = data;
    }

    public bool RemoveData(string name)
    {
        return m_Datas.Remove(name);
    }
}
