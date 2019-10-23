using FlappyBird;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class ScoreForm : UGuiForm {

    public Text scoreTxt;

    private int m_Score = 0;

    private float m_ScoreTimer = 0;//积分计时器

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        FlappyBird.GameEntry.Event.Subscribe(BirdDeadEventArgs.EventId, OnBirdDead);
        FlappyBird.GameEntry.Event.Subscribe(AddScoreEventArgs.EvtId, OnAddScore);
    }
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        m_ScoreTimer += elapseSeconds;
        if(m_ScoreTimer >= 2)
        {
            m_ScoreTimer = 0;
            m_Score += 1;
            scoreTxt.text = "总分：" + m_Score;
        }
    }
    protected override void OnClose(object userData)
    {
        base.OnClose(userData);
        FlappyBird.GameEntry.Event.Unsubscribe(BirdDeadEventArgs.EventId, OnBirdDead);
        FlappyBird.GameEntry.Event.Unsubscribe(AddScoreEventArgs.EvtId, OnAddScore);
    }
    protected override void OnPause()
    {
        base.OnPause();
        m_ScoreTimer = 0;
        m_Score = 0;
        scoreTxt.text = "总分：" + m_Score;
    }
    private void OnBirdDead(object sender, GameEventArgs e)
    {
        FlappyBird.GameEntry.DataNode.GetOrAddNode("Score").SetData<VarInt>(m_Score);
        FlappyBird.GameEntry.UI.OpenUIForm(UIFormId.GameOverForm);
    }

    private void OnAddScore(object sender,GameEventArgs e)
    {
        AddScoreEventArgs ase = e as AddScoreEventArgs;
        m_Score += ase.AddCount;
        scoreTxt.text = "总分：" + m_Score;
    }
}
