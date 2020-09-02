using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 精灵资源工厂
/// </summary>
public class SpriteFactory : IBaseResourceFactory<Sprite>
{
    //当前拥有的Sprite类型的资源
    protected Dictionary<string, Sprite> resourceDict = new Dictionary<string, Sprite>();
    protected string loadPath;

    public SpriteFactory()
    {
        loadPath += "Pictures/";
    }
    public Sprite GetSingleResource(string resourceName)
    {
        Sprite itemAC = null;
        string path = loadPath + resourceName;
        if (resourceDict.ContainsKey(resourceName))
        {
            itemAC = resourceDict[resourceName];
        }
        else
        {
            itemAC = Resources.Load<Sprite>(path);
            resourceDict.Add(resourceName, itemAC);
        }
        if (itemAC == null)
        {
            Debug.Log(resourceName + " 获取失败");
        }
        return itemAC;
    }
}
