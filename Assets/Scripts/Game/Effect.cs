using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制特效的脚本
/// </summary>
public class Effect : MonoBehaviour
{
    public float animationTime; //特效播放时长
    public string resourceName;

    private void OnEnable()
    {
        Invoke("DestroyEffect", animationTime); //在animationTime秒后调用DestroyEffect方法
    }
  
    //销毁特效
    private void DestroyEffect()
    {
        GameController.Instance.PushGameObjectToFactory(resourceName,gameObject);//放入对象池
    }
}
