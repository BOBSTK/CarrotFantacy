using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Builder接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBuilder <T>  //可以产生Monster或者Tower，所以使用泛型
{
    //获取游戏物体身上的脚本对象
    T GetProductClass(GameObject gameObject);

    //获取具体的游戏对象(通过工厂)
    GameObject GetProduct();

    //获取对象的数据信息
    void GetData(T productClass);

    //获取特定的资源（比如塔的子弹）
    void GetOtherResource(T productClass);
}
