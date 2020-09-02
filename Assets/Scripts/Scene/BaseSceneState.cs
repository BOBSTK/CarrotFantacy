using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//场景基类
public class BaseSceneState : IBaseSecenState
{
    protected UIFacade uiFacade;

    public BaseSceneState(UIFacade uiFacade)
    {
        this.uiFacade = uiFacade;
    }
    public virtual void EnterScene()   //进入场景的方法
    {
        uiFacade.InitPanel();   //初始化面板字典，后续操作由子类完成 主要是加载场景所需要的UIPanel
    }

    public virtual void ExitScene()    //离开场景的方法 还要切换到下一个场景
    {
        uiFacade.ClearDict();   //清空场景相关的字典 包括UIFacade和UIManager中保存的当前场景的Panel对象
    }

}
