using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitUIPanel : MonoBehaviour
{
    private string loadPath = "Prefabs/UIPanel/";
    public Transform canvasTransform;//获取Canvas(只有一个)
    void Start()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
        InitUI("StartLoadPanel");
    }

   
    //实例化UI
    public GameObject InitUI(string uiName)
    {
        GameObject item;
        item = GetPanel (uiName);
        //item.transform.SetParent(canvasTransform);    //将UI组件放到Canvas下
        //item.transform.localPosition = Vector3.zero;  //坐标修正
        item.SetActive(true);
        return item;
    }

    private GameObject GetPanel(string itemName)
    {
        GameObject itemGo = null;
        string path = loadPath + itemName;
        itemGo = Resources.Load<GameObject>(path); //加载存储在名为 Resources 文件夹中的 path 处的资源
        if (itemGo == null)
        {
            Debug.Log(itemName + " 的资源获取失败");
            Debug.Log("资源路径: " + path);
        }

        return itemGo;
    }
}
