using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPool<T> where T : ITask
{
    /// <summary>
    /// 可用任务代理
    /// </summary>
    private Stack<ITaskAgent<T>> m_FreeAgents;
    /// <summary>
    /// 工作中的任务代理
    /// </summary>
    private LinkedList<ITaskAgent<T>> m_WorkingAgents;

    private LinkedList<T> m_WaitingTasks;

    public int FreeAgentsCnt { get { return m_FreeAgents.Count; } }

    public int WorkingAgentCount { get { return m_WorkingAgents.Count; } }

    public int TotalAgentCount { get { return FreeAgentsCnt + WorkingAgentCount; } }

    public int WaitingTaskCount { get { return m_WorkingAgents.Count; } }

    public TaskPool()
    {
        m_FreeAgents = new Stack<ITaskAgent<T>>();
        m_WorkingAgents = new LinkedList<ITaskAgent<T>>();
        m_WaitingTasks = new LinkedList<T>();
    }


    public void AddAgent(ITaskAgent<T> agent)
    {
        if (agent == null) return;
        agent.Initialize();
        m_FreeAgents.Push(agent);
    }

    public void AddTask(T task)
    {
        m_WaitingTasks.AddLast(task);
    }

    public T RemoveTask(int id)
    {
        foreach(T task in m_WaitingTasks)
        {
            if(task.SerialId == id)
            {
                m_WaitingTasks.Remove(task);
                return task;
            }
        }

        foreach(ITaskAgent<T> agent in m_WorkingAgents)
        {
            if(agent.Task.SerialId == id)
            {
                agent.Reset();
                m_FreeAgents.Push(agent);
                m_WorkingAgents.Remove(agent);
                return agent.Task;
            }
        }

        return default(T);
    }
    /// <summary>
    /// 移除所有任务
    /// </summary>
    public void RemoveAllTasks()
    {
        m_WaitingTasks.Clear();
        //重置所有工作中任务代理
        foreach (ITaskAgent<T> workingAgent in m_WorkingAgents)
        {
            workingAgent.Reset();
            m_FreeAgents.Push(workingAgent);
        }
        m_WorkingAgents.Clear();

    }
    /// <summary>
    /// 任务池轮询
    /// </summary>
    public void Update(float elapseSeconds, float realElapseSeconds)
    {
        //获取第一个工作中任务代理
        LinkedListNode<ITaskAgent<T>> current = m_WorkingAgents.First;
        while (current != null)
        {
            //如果当前工作中任务代理已完成任务
            if (current.Value.Task.Done)
            {
                //就让它重置并从工作中任务代理中移除
                LinkedListNode<ITaskAgent<T>> next = current.Next;
                current.Value.Reset();
                m_FreeAgents.Push(current.Value);
                m_WorkingAgents.Remove(current);
                current = next;
                continue;
            }

            //未完成就轮询任务代理
            current.Value.Update(elapseSeconds, realElapseSeconds);
            current = current.Next;
        }

        //有可用任务代理并且有等待中任务
        while (FreeAgentsCnt > 0 && WaitingTaskCount > 0)
        {
            //出栈一个任务代理
            ITaskAgent<T> agent = m_FreeAgents.Pop();
            //添加到工作中任务代理
            LinkedListNode<ITaskAgent<T>> agentNode = m_WorkingAgents.AddLast(agent);

            //获取一个等待中的任务
            T task = m_WaitingTasks.First.Value;
            m_WaitingTasks.RemoveFirst();

            //开始处理这个任务
            agent.Start(task);
            if (task.Done)
            {
                agent.Reset();
                m_FreeAgents.Push(agent);
                m_WorkingAgents.Remove(agentNode);
            }
        }
    }


    /// <summary>
    /// 关闭并清理任务池
    /// </summary>
    public void Shutdown()
    {
        while (FreeAgentsCnt > 0)
        {
            m_FreeAgents.Pop().Shutdown();
        }

        foreach (ITaskAgent<T> workingAgent in m_WorkingAgents)
        {
            workingAgent.Shutdown();
        }
        m_WorkingAgents.Clear();

        m_WaitingTasks.Clear();
    }

}
