using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureOver : ProcedureBase {

    public override void OnUpdate(Fsm<ProcedureMgr> fsm, float elapseSec, float realElapseSec)
    {
        base.OnUpdate(fsm, elapseSec, realElapseSec);
        if (Input.GetMouseButtonDown(0))
        {
            fsm.ChangeState(typeof(ProcedureStart));
          //  ChangeState<ProcedureStart>(fsm);
        }

    }
}
