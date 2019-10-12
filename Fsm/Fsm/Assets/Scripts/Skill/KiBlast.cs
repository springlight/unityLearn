using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//SDJ
public class KiBlast : Skill
{

	public KiBlast(Player player) : base(player)
    {

    }

    protected override int GetSkillId()
    {
        return 2;
    }
}
