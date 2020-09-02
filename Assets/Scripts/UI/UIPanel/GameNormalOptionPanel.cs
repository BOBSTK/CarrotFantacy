using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 冒险模式关卡选择背景面板
/// </summary>
public class GameNormalOptionPanel : BasePanel
{
    [HideInInspector] //在Inspecter中隐藏
    public bool isInBigLevelPanel = true;   //判断是否在大关卡面板，涉及按钮功能

    //返回按钮的功能
    public void ReturnToLastPanel()
    {
        if (isInBigLevelPanel)
        {
            uiFacade.ChangeSceneState(new MainSceneState(uiFacade)); //如果是在大关卡面板，返回主界面
        }
        else
        {
            //如果是在小关卡，返回大关卡面板
            uiFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel].ExitPanel();
            uiFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].EnterPanel();  
        }
        uiFacade.PlayButtonAudioClip();
        isInBigLevelPanel = true;
    }

    //进入HelpPanel
    public void ToHelpPanel()
    {
        uiFacade.PlayButtonAudioClip();
        uiFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }
    
}
