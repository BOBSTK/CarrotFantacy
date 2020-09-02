using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIPanel接口
/// </summary>
public interface IBasePanel 
{
    void InitPanel();   //初始化面板
    void EnterPanel(); //进入面板
    void ExitPanel(); //退出面板
    void UpdatePanel(); //更新面板
}
