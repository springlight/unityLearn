using FlappyBird;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class GameOverForm : UGuiForm {

    public Text scoreTxt;
    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        int score = FlappyBird.GameEntry.DataNode.GetNode("Score").GetData<VarInt>();
        scoreTxt.text = "你的总分： " + score;
    }

    protected override void OnClose(object userData)
    {
        base.OnClose(userData);
        scoreTxt.text = string.Empty;
    }

    public void OnRestartGame()
    {
        Close(true);
        FlappyBird.GameEntry.Event.Fire(this, ReferencePool.Acquire<RestartEventArgs>());
        FlappyBird.GameEntry.Entity.ShowBird(new BirdData(FlappyBird.GameEntry.Entity.GenerateSerialId(), 3, 5f));
    }

    public void OnReturnButtonClick()
    {
        Close(true);
        //派发返回菜单场景事件
        FlappyBird.GameEntry.Event.Fire(this, ReferencePool.Acquire<ReturnMenuEvenArgs>());
    }

}
