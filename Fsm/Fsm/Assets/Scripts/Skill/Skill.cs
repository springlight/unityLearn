using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 招式基类
/// </summary>
public class Skill
{

    protected int skillID;
    /// <summary>
    /// 每一个招式都是一个状态机
    /// </summary>
    protected Fsm fsm = new Fsm();

    protected Player player;//招式持有者

    protected int maxSkillStateId;

    protected int stateId = 0;

    public Skill(Player player)
    {
        this.player = player;
        skillID = GetSkillId();
        Init();
        fsm.Start(1);
    }

    protected void Init()
    {
        if (SkillData.instructions.ContainsKey(skillID))
        {
            int[] instructions = null;
            SkillData.instructions.TryGetValue(skillID, out instructions);

            maxSkillStateId = instructions.Length;

            for(int i =0; i < instructions.Length; i++)
            {
                if(i == instructions.Length - 1)
                {
                    AddInstructionsState((KeyCode)instructions[i], SkillFight);
                }
                else
                {
                    AddInstructionsState((KeyCode)instructions[i]);
                }
            }


        }
    }


    protected void AddInstructionsState(KeyCode keyCode,UnityAction action = null,float inputWaitTime = 0.5f)
    {
        fsm.AddState(new InstructionsState(++stateId, inputWaitTime, keyCode, action, maxSkillStateId));
    }

    protected virtual void SkillFight()
    {
        player.ResetSkill();
    }

    protected virtual int GetSkillId() { return -1; }

    public void Update(float deltaTime)
    {
        fsm.Update(deltaTime);
    }

    public void Reset()
    {
        fsm.ChangeState(1);
    }
}
