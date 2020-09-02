using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 塔的建造者
/// 输入towerID 和 towerLevel 调用GetProduct()
/// 输出 塔对象
/// </summary>
public class TowerBuilder : IBuilder<Tower>
{
    public int towerID;
    private GameObject towerGo;
    public int towerLevel; //塔的等级


    public void GetData(Tower productClass)
    {
        productClass.towerID = towerID;
    }

    public void GetOtherResource(Tower productClass)
    {
        productClass.GetTowerProperty();
    }

    public GameObject GetProduct()
    {
        //通过towerID 和towerLevel获取塔对象
        GameObject item = GameController.Instance.GetGameObject("Tower/ID"+towerID.ToString()+ "/TowerSet/"+towerLevel.ToString());
        Tower tower = GetProductClass(item);
        GetData(tower);
        GetOtherResource(tower);
        return item;
    }

    public Tower GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Tower>();
    }
}
