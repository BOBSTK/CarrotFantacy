using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RuntimeAnimatorController:可在运行时期间更改 Animator Controllerd
/// <summary>
/// 动画控制器资源工厂
/// </summary>
public class RuntimeAnimatorControllerFactory : IBaseResourceFactory<RuntimeAnimatorController>
{
    //当前拥有的RuntimeAnimatorController类型的资源
    protected Dictionary<string, RuntimeAnimatorController> resourceDict = new Dictionary<string, RuntimeAnimatorController>();
    protected string loadPath;

    public RuntimeAnimatorControllerFactory()
    {
        loadPath += "Animator/AnimatorController/";
    }
    public RuntimeAnimatorController GetSingleResource(string resourceName)
    {
        RuntimeAnimatorController itemAC = null;
        string path = loadPath + resourceName;
        if (resourceDict.ContainsKey(resourceName))
        {
            itemAC = resourceDict[resourceName];
        }
        else
        {
            itemAC = Resources.Load<RuntimeAnimatorController>(path);
            resourceDict.Add(resourceName, itemAC);
        }
        if (itemAC == null)
        {
            Debug.Log(resourceName + " 获取失败,失败路径：" + path);
        }
        return itemAC;
    }
}
