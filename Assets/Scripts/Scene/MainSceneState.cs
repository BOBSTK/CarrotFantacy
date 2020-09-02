using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 主界面场景状态
/// </summary>
public class MainSceneState : BaseSceneState
{
    public MainSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        //加载主界面要用到的面板
        uiFacade.AddPanelToDict(StringManager.MainPanel);
        uiFacade.AddPanelToDict(StringManager.SetPanel);
        uiFacade.AddPanelToDict(StringManager.HelpPanel);
        uiFacade.AddPanelToDict(StringManager.GameLoadPanel);
        base.EnterScene();
        //播放背景音乐
        GameManager.Instance.audioSourceManager.PlayBGMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResource("Main/BGMusic"));
    }

    public override void ExitScene()
    {
        base.ExitScene();
        //反射
        if (uiFacade.currentSceneState.GetType() == typeof(GameNormalOptionPanel)) 
        {
            SceneManager.LoadScene(2); //加载冒险模式场景
        }
        else if(uiFacade.currentSceneState.GetType() == typeof(GameBossOptionPanel))
        {
            SceneManager.LoadScene(4); //加载Boss模式场景
        }
        else
        {
            SceneManager.LoadScene(6);//加载怪物窝模式场景
        }
    }
}
