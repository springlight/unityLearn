using FlappyBird;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brid : Entity
{

    private BirdData m_BirdData = null;

    /// <summary>
    /// 射击时间
    /// </summary>
    private float m_ShootTime = 10f;

    /// <summary>
    /// 射击计时器
    /// </summary>
    private float m_ShootTimer = 0f;

    private Rigidbody2D m_Rigidbody = null;
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_BirdData = (BirdData)userData;
        CachedTransform.localScale = Vector2.one * 2;
        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }
        m_ShootTimer = 10f;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameEntry.Sound.PlaySound(2);
            m_Rigidbody.velocity = new Vector2(0, m_BirdData.FlyForce);
        }

        m_ShootTimer += elapseSeconds;
        if (m_ShootTimer >= m_ShootTime && Input.GetKeyDown(KeyCode.J))
        {
            m_ShootTimer = 0;
            GameEntry.Sound.PlaySound(3);
            GameEntry.Entity.ShowBullet(new BulletData(GameEntry.Entity.GenerateSerialId(), 4, CachedTransform.position + CachedTransform.right, 6));

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameEntry.Sound.PlaySound(1);
        GameEntry.Entity.HideEntity(this);
        //派发小鸟死亡2019年10月23日14:28:13
        GameEntry.Event.Fire(this, ReferencePool.Acquire<BirdDeadEventArgs>());

    }

    
}
