using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理UI的管理者
/// </summary>
public class UIManager
{
    public UIFacade uiFacade;  //中介
    public Dictionary<string, GameObject> currentScenePanelDict; //面板字典
    private GameManager gameManager; 

    public UIManager()
    {
        gameManager = GameManager.Instance;
        currentScenePanelDict = new Dictionary<string, GameObject>();
        uiFacade = new UIFacade(this);  //把自身传递给UIFacade
        uiFacade.currentSceneState = new StartLoadSceneState(uiFacade); //设置初始状态
    }

    //将Panel放回工厂
    public void PushUIPanel(string uiPanelName, GameObject uiPanel)
    {
        gameManager.PushGameObjectToFactory(FactoryType.UIPanelFactory, uiPanelName, uiPanel);
    }

    //清空字典
    public void ClearDict()
    {
        foreach (var item in currentScenePanelDict)
        {
            PushUIPanel(item.Value.name.Substring(0,item.Value.name.Length-7),item.Value);  ///
        }
        currentScenePanelDict.Clear();
    }
}
