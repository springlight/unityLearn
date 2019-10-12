using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//DSDJ
public class FirePunch : Skill
{
    
    public FirePunch(Player player) : base(player)
    {

    }

    protected override int GetSkillId()
    {
        return 1;
    }

    protected override void SkillFight()
    {
        base.SkillFight();
        Debug.LogError("升龙拳");

    }
}
