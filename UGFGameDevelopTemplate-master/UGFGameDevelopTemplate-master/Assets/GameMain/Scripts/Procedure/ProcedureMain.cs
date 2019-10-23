using FlappyBird;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

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

    /// <summary>
    /// 结束界面ID
    /// </summary>
    private int m_ScoreFormId = -1;

    private bool m_IsReturnMenu = false;

    protected override void OnEnter(IFsm<IProcedureManager> proce)
    {
        base.OnEnter(proce);

        m_ScoreFormId = GameEntry.UI.OpenUIForm(UIFormId.ScoreForm).Value;
        GameEntry.Entity.ShowBg(new BgData(GameEntry.Entity.GenerateSerialId(), 1, 1f, 0));
        GameEntry.Entity.ShowBird(new BirdData(GameEntry.Entity.GenerateSerialId(), 3, 5f));
        m_PipeSpawnTime = Random.Range(3f, 5f);
        GameEntry.Event.Subscribe(ReturnMenuEvenArgs.EventId, OnReturnMenu);


    }
    private void OnReturnMenu(object sender, GameEventArgs e)
    {
        m_IsReturnMenu = true;
    }

    protected override void OnUpdate(IFsm<IProcedureManager> proce,float elapseSeconds,float realElapseSeconds)
    {
        base.OnUpdate(proce, elapseSeconds, realElapseSeconds);
        m_PipeSpawnTimer += elapseSeconds;
        if(m_PipeSpawnTimer >= m_PipeSpawnTime)
        {
            m_PipeSpawnTimer = 0;
            m_PipeSpawnTime = Random.Range(3f, 5f);
            FlappyBird.GameEntry.Entity.ShowPipe(new PipeData(FlappyBird.GameEntry.Entity.GenerateSerialId(), 2, 1f));
        }

        //切换场景
        if (m_IsReturnMenu)
        {
            m_IsReturnMenu = false;
            proce.SetData<VarInt>(Constant.ProcedureData.NextSceneId, FlappyBird.GameEntry.Config.GetInt("Scene.Menu"));
            ChangeState<ProcedureChangeScene>(proce);
        }

    }
    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        GameEntry.UI.CloseUIForm(m_ScoreFormId);
        GameEntry.Event.Unsubscribe(ReturnMenuEvenArgs.EventId, OnReturnMenu);

    }



}
