using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 奖品
/// </summary>
public class Prize : MonoBehaviour
{

    private void Update()
    {
        if (GameController.Instance.isGameOver) //游戏结束时销毁场上的物品
        {
            GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
        }
    }
    private void OnMouseDown()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/GiftGot");
        //GameController.Instance.isPause = true; //游戏暂停
#if Game
        GameController.Instance.ShowPrizePage();
#endif
        GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
    }
}
