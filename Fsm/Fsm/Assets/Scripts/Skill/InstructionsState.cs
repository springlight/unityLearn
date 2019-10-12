using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstructionsState : FsmState
{
    private float inputWaitTime;

    private float timer;
    private KeyCode keyCode;
    private UnityAction action;
    private int maxStateID;

    public InstructionsState(int stateId,float inputWaitTime,KeyCode keyCode,UnityAction action,int maxStateId):base(stateId)
    {
        this.inputWaitTime = inputWaitTime;
        timer = 0f;
        this.keyCode = keyCode;
        this.action = action;
        this.maxStateID = maxStateId;
    }

    public override void OnEnter(Fsm fsm)
    {
      
    }

    public override void OnLeave(Fsm fsm)
    {
       
    }

    public override void OnUpdate(Fsm fsm, float deltaTime)
    {
       timer += deltaTime;
        //输入指令等待耗尽，重置指令状态
        if(timer >= inputWaitTime)
        {
            timer = 0;
            fsm.ChangeState(1);
        }

        if (Input.anyKeyDown)
        {
            timer = 0;
            if (Input.GetKeyDown(keyCode))
            {
                Debug.LogError("玩家按下了--->" + keyCode.ToString());
                //该指令要执行的方法
                if(action != null)
                {
                    action();
                }
                //最后一个状态
                if(stateId == maxStateID)
                {
                    fsm.ChangeState(1);
                }
                else
                {
                    //不是最后一个状态,切换到下一个指令
                    fsm.ChangeState(stateId + 1);
                }
            }
            else
            {
                fsm.ChangeState(1);
            }
        }
    }

}
