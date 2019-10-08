﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureStart : ProcedureBase {

    public override void OnUpdate(Fsm<ProcedureMgr> fsm, float elapseSec, float realElapseSec)
    {
        base.OnUpdate(fsm, elapseSec, realElapseSec);
        if (Input.GetMouseButton(0))
        {
            ChangeState<ProcedurePlay>(fsm);
        }
    }
}
