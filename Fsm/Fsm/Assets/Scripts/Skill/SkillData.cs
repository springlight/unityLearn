using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 招式数据
/// </summary>
public class SkillData
{
    ///指令数组中存储的是指令对应按键的KeyCode枚举所对应的整数
    public static Dictionary<int, int[]> instructions = new Dictionary<int, int[]>();

    static SkillData()
    {
        //  //100-D 115-S 106-J
        instructions.Add(1, new int[] { 100, 115, 100, 106 });
        instructions.Add(2, new int[] { 115, 100, 106 });
    }
	
}
