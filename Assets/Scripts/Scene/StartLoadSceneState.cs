using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoadSceneState : BaseSceneState
{
    public StartLoadSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        uiFacade.AddPanelToDict(StringManager.StartLoadPanel); //将面板从对象池中取出放入UIManager的UI面板字典
        base.EnterScene();
    }

    public override void ExitScene()
    {
        base.ExitScene();
        //运行时的场景管理
        SceneManager.LoadScene(1); //加载MainScene
    }


}
