using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 流程管理器
/// </summary>
public class ProcedureMgr : MgrBase
{
    /// <summary>
    /// 状态机管理器
    /// </summary>
    private FsmMgr m_FsmMgr;

    /// <summary>
    /// 流程的状态机
    /// </summary>
    private Fsm<ProcedureMgr> m_ProcedureFsm;
    /// <summary>
    /// 所有流程列表
    /// </summary>
    private List<ProcedureBase> m_Procudures;

    /// <summary>
    /// 入口流程
    /// </summary>
    private ProcedureBase m_EntranceProcedure;

    public ProcedureBase CurProcedure
    {
        get
        {
            if(m_ProcedureFsm == null)
            {
                return null;
            }
            return (ProcedureBase)m_ProcedureFsm.CurState;
        }
    }

    public override int Priority
    {
        get
        {
            return -10;
        }
    }

    public ProcedureMgr()
    {
        m_FsmMgr = FrameWorkEntry.Ins.GetMgr<FsmMgr>();
        m_ProcedureFsm = null;
        m_Procudures = new List<ProcedureBase>();
    }

    public override void Init()
    {
        
    }

    public override void ShutDown()
    {
       
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        
    }

    ///<summary>
    ///添加流程
    ///</summary>
    public void AddProcedure(ProcedureBase procedure)
    {
        m_Procudures.Add(procedure);
    }

    ///<summary>
    ///设置入口流程
    ///</summary>
    public void SetEntranceProcedure(ProcedureBase procedure)
    {
        m_EntranceProcedure = procedure;
    }
    ///<summary>创建流程状态机</summary>
    public void CreateProceduresFsm()
    {
        m_ProcedureFsm = m_FsmMgr.CreateFsm(this, "", m_Procudures.ToArray());
        if(m_EntranceProcedure == null)
        {
            Debug.LogError("入口流程为空，无法开始流程");
            return;
        }
        m_ProcedureFsm.Start(m_EntranceProcedure.GetType());
    }
}
