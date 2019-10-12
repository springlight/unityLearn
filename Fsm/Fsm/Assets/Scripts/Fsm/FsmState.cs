using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FsmState
{
    public int stateId;
    protected FsmState(int stateId)
    {
        this.stateId = stateId;
    }

    public abstract void OnEnter(Fsm fsm);

    public abstract void OnUpdate(Fsm fsm, float deltaTime);

    public abstract void OnLeave(Fsm fsm);

    public void ChangeState<T>(int stateId,Fsm fsm)where T : FsmState
    {
        
    }

}
