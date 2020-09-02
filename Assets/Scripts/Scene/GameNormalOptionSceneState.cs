using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNormalOptionSceneState : BaseSceneState
{
    public GameNormalOptionSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        uiFacade.AddPanelToDict(StringManager.GameNormalOptionPanel);
        uiFacade.AddPanelToDict(StringManager.GameNormalBigLevelPanel);
        uiFacade.AddPanelToDict(StringManager.GameNormalLevelPanel);
        uiFacade.AddPanelToDict(StringManager.HelpPanel);
        uiFacade.AddPanelToDict(StringManager.GameLoadPanel);
        base.EnterScene();
    }

    public override void ExitScene()
    {
        //ToDo 优化 异步加载场景
        GameNormalOptionPanel gameNormalOptionPanel = uiFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
        if (gameNormalOptionPanel.isInBigLevelPanel) //如果是在大关卡选择面板
        {
            SceneManager.LoadScene(1); //加载主场景
        }
        else
        {
            SceneManager.LoadScene(3); //加载游戏场景
        }
        gameNormalOptionPanel.isInBigLevelPanel = true; //无论是哪种情况，gameNormalOptionPanel的isInBigLevelPanel都应该设为true

        base.ExitScene();
    }

}
