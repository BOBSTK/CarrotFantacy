using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLoadPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        Invoke("LoadNextScene", 2);
    }

    private void LoadNextScene()
    {
        uiFacade.ChangeSceneState(new MainSceneState(uiFacade)); //切换场景状态到MainScene
    }
}
