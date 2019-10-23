using FlappyBird;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity {


    private BulletData m_BulletData = null;

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_BulletData = (BulletData)userData;
        CachedTransform.SetLocalScaleX(1.8f);
        CachedTransform.position = m_BulletData.ShootPos;

        //监听小鸟死亡事件
        GameEntry.Event.Subscribe(BirdDeadEventArgs.EventId, OnBirdDead);

    }

    private void OnBirdDead(object sender, GameEventArgs e)
    {
        //小鸟死亡后立即隐藏自身
        GameEntry.Entity.HideEntity(this);
    }


    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        CachedTransform.Translate(Vector2.right * m_BulletData.FlySpeed * elapseSeconds, Space.World);
        if(CachedTransform.position.x >= 9.1f)
        {
            GameEntry.Entity.HideEntity(this);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //隐藏管道和自身
        GameEntry.Sound.PlaySound(1);
        other.gameObject.SetActive(false);
        GameEntry.Entity.HideEntity(this);
    }

}
