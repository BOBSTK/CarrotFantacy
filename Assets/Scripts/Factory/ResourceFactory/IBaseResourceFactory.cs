using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源工厂的接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseResourceFactory <T>
{
    T GetSingleResource(string resourceName);//获取单个资源
}
