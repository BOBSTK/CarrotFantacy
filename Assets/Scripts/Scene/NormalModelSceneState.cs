using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏场景
/// </summary>
public class NormalModelSceneState : BaseSceneState
{
    public NormalModelSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        uiFacade.AddPanelToDict(StringManager.GameLoadPanel);     //游戏加载面板
        uiFacade.AddPanelToDict(StringManager.NormalModelPanel);  //冒险模式游戏面板
        GameManager.Instance.audioSourceManager.CloseBGMusic(); //关闭背景音乐
        base.EnterScene();
    }

    public override void ExitScene()
    {
        base.ExitScene();
        GameManager.Instance.audioSourceManager.OpenBGMusic(); //打开背景音乐
    }
}
