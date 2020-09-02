using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 怪物窝场景脚本
/// </summary>
public class MonsterNestSceneState : BaseSceneState
{
    public MonsterNestSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        //播放怪物窝的背景音乐
        uiFacade.AddPanelToDict(StringManager.GameLoadPanel);
        uiFacade.AddPanelToDict(StringManager.MonsterNetPanel);
        base.EnterScene();
        GameManager.Instance.audioSourceManager.PlayBGMusic(GameManager.Instance.GetAudioClip("MonsterNest/BGMusic"));
    }

    public override void ExitScene()
    {
        SceneManager.LoadScene(1); //加载主场景
        base.ExitScene();
    }
}
