using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 心脏特效脚本
/// </summary>
public class Heart : MonoBehaviour
{
    public float animationTime;
    public string resourcePath;

    private void OnEnable()
    {
        Invoke("DestroyEffect",animationTime);
    }

    private void DestroyEffect()
    {
        GameManager.Instance.PushGameObjectToFactory(FactoryType.UIFactory, resourcePath, gameObject);
    }
}
