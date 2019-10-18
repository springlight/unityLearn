using FlappyBird;
using GameFramework.Event;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

public class ProcedureMenu : ProcedureBase {

	public bool IsStartGame { get; set; }
    private MenuForm m_MenuForm = null;

    protected override void OnEnter(ProcedureOwner owner)
    {
        base.OnEnter(owner);
        IsStartGame = false;
        FlappyBird.GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormSuccess);
        FlappyBird.GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
    }
    
    protected override void OnUpdate(ProcedureOwner owner,float elapseSec,float realElasp)
    {
        base.OnUpdate(owner, elapseSec, realElasp);
        if (IsStartGame)
        {
            owner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, FlappyBird.GameEntry.Config.GetInt("Scene.Main"));
            ChangeState<ProcedureChangeScene>(owner);
        }
    }

    protected override void OnLeave(ProcedureOwner owner,bool isShutdown)
    {
        base.OnLeave(owner, isShutdown);
        if(m_MenuForm != null)
        {
            m_MenuForm.Close(isShutdown);
            m_MenuForm = null;
        }
        FlappyBird.GameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormSuccess);
    }

    private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
    {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        m_MenuForm = (MenuForm)ne.UIForm.Logic;
    }

}
