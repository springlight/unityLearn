using FlappyBird;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureMain : ProcedureBase
{
    /// <summary>
    /// 管道产生时间
    /// </summary>
    private float m_PipeSpawnTime = 0f;
    /// <summary>
    /// 管道产生计时器
    /// </summary>
    private float m_PipeSpawnTimer = 0f;
	protected override void OnEnter(IFsm<IProcedureManager> proce)
    {
        base.OnEnter(proce);
        GameEntry.Entity.ShowBg(new BgData(GameEntry.Entity.GenerateSerialId(), 1, 1f, 0));
        m_PipeSpawnTime = Random.Range(3f, 5f);

    }

    protected override void OnUpdate(IFsm<IProcedureManager> proce,float elapseSeconds,float realElapseSeconds)
    {
        base.OnUpdate(proce, elapseSeconds, realElapseSeconds);
        m_PipeSpawnTimer += elapseSeconds;
        if(m_PipeSpawnTimer >= m_PipeSpawnTime)
        {
            m_PipeSpawnTimer = 0;
            m_PipeSpawnTime = Random.Range(3f, 5f);
            GameEntry.Entity.ShowPipe(new PipeData(GameEntry.Entity.GenerateSerialId(), 2, 1f));
        }
    }


}
