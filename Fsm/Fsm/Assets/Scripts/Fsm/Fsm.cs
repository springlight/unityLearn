using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fsm
{
    private Dictionary<int, FsmState> states = new Dictionary<int, FsmState>();

    private FsmState curState;

    public void AddState(FsmState state)
    {
        if (!states.ContainsKey(state.stateId))
        {
            states.Add(state.stateId, state);
        }
    }
	

    public void ChangeState(int stateId)
    {
        if (states.ContainsKey(stateId))
        {
            FsmState state = states[stateId];
            curState.OnLeave(this);
            curState = state;
            curState.OnEnter(this);
        }
    }

    public void Start(int stateId)
    {
        if (states.ContainsKey(stateId))
        {
            FsmState state = states[stateId];
            curState = state;
            curState.OnEnter(this);
        }
    }

    public void Update(float deltaTime)
    {
        curState.OnUpdate(this, deltaTime);
    }
}
