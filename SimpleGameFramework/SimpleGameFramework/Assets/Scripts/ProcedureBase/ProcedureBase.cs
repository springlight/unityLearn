using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureBase : FsmState<ProcedureMgr>
{
    public override void OnEnter(Fsm<ProcedureMgr> fsm)
    {
        base.OnEnter(fsm);
        Debug.LogError("进入流程--》" + GetType().FullName);
    }

    public override void OnLeave(Fsm<ProcedureMgr> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        Debug.LogError("离开流程--》" + GetType().FullName);
    }

}
