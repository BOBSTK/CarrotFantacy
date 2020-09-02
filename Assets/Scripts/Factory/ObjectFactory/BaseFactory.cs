using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏物体工厂基类
/// </summary>
public class BaseFactory : IBaseFactory
{
    //当前拥有的GameObject类型的资源(UI,UIPanle,Game)
    protected Dictionary<string, GameObject> resourceDict = new Dictionary<string, GameObject>();

    //对象池
    protected Dictionary<string, Stack<GameObject>> poolDict = new Dictionary<string, Stack<GameObject>>(); //对象池列表

    //加载路径
    protected string loadPath;
    
    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }

    //从对象池取对象
    public GameObject GetItem(string itemName)
    {
        GameObject itemGO = null;
        if (poolDict.ContainsKey(itemName))
        {
            if(poolDict[itemName].Count > 0)  //对象池中有对象
            {
                itemGO = poolDict[itemName].Pop();
                itemGO.SetActive(true);
            }
            else                              //对象池中没有对象
            {
                GameObject go = GetResources(itemName);
                itemGO = GameManager.Instance.CreatItem(go);
            }
        }
        else   //没有相应的对象池
        {
            poolDict.Add(itemName,new Stack<GameObject>());
            GameObject go = GetResources(itemName);    
            itemGO = GameManager.Instance.CreatItem(go);
        }

        if(itemGO == null)
        {
            Debug.Log(itemName+" 的实例获取失败");
        }
        return itemGO;
    }

    //放入对象池
    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(GameManager.Instance.transform); //将隐藏的对象放在GameManager下面
        if (poolDict.ContainsKey(itemName))
        {
            poolDict[itemName].Push(item);
        }
        else
        {
            Debug.Log("当前对象池列表没有 "+itemName+" 的对象池!!");
        }
    }

    //取资源
    private GameObject GetResources(string itemName)
    {
        GameObject itemGo = null;
        string path = loadPath + itemName;
        if (resourceDict.ContainsKey(itemName))
        {
            itemGo = resourceDict[itemName];
        }
        else
        {
            itemGo = Resources.Load<GameObject>(path); //加载存储在名为 Resources 文件夹中的 path 处的资源
            resourceDict.Add(itemName, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log(itemName + " 的资源获取失败");
            Debug.Log("资源路径: " + path);
        }

        return itemGo;
    }
}
