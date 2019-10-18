using FlappyBird;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bg : Entity
{

    /// <summary>
    /// 背景实体数据
    /// </summary>
    private BgData m_BgData = null;
    private bool m_IsSpawn = false;

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_BgData = (BgData)userData;
        CachedTransform.SetLocalPositionX(m_BgData.StartPos);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        //控制背景实体移动
        CachedTransform.Translate(Vector3.left * m_BgData.MoveSpeed * elapseSeconds, Space.World);
        if(CachedTransform.position.x <=m_BgData.SpawnTarget && m_IsSpawn == false)
        {
            //显示背景色实体
            GameEntry.Entity.ShowBg(new BgData(GameEntry.Entity.GenerateSerialId(), m_BgData.TypeId, m_BgData.MoveSpeed, m_BgData.StartPos));
            m_IsSpawn = true;
        }
        if(CachedTransform.position.x <= m_BgData.HideTarget)
        {
            GameEntry.Entity.HideEntity(this);
        }

        

    }

    protected override void OnHide(object userData)
    {
        base.OnHide(userData);
        m_IsSpawn = false;
    }
}
