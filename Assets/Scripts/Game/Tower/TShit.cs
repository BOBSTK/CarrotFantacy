using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 便便塔特性脚本
/// 攻击时不需要转向
/// </summary>
public class TShit : TowerSpecificProperty
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameController.Instance.isPause || targetTrans == null || GameController.Instance.isGameOver)  //游戏暂停或者丢失目标
        {
            return;
        }

        if (timeVal >= attackCD / GameController.Instance.gameSpeed) //可以攻击
        {
            timeVal = 0;  //重置计时器
            Attack();     //攻击
        }
        else      //攻击CD中
        {
            timeVal += Time.deltaTime;
        }

    }
}
