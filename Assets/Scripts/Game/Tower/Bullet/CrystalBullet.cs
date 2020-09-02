using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔法球子弹脚本
/// </summary>
public class CrystalBullet : Bullet
{
    //属性
    private float attackTimeVal;  //子弹计时器
    private bool canTakeDamage;   //是否应该造成伤害

    protected override void Update()
    {
        //游戏结束时销毁场上的所有子弹
        if (GameController.Instance.isGameOver)
        {
            DestroyBullet();
        }
        if (GameController.Instance.isPause) //暂停
        {
            return;
        }
        //如果没有攻击目标或者目标已经失活
        if (targetTrans == null || !targetTrans.gameObject.activeSelf)
        {
            DestroyBullet();
            return;
        }

        //子弹移动与转向(物品) 
        if (targetTrans.gameObject.tag == "Item")
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else if (targetTrans.gameObject.tag == "Monster")
        {
            //方向
            transform.LookAt(targetTrans.position);
        }
        //纠正旋转角度
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
        if (!canTakeDamage) //不可以造成伤害
        {
            attackTimeVal += Time.deltaTime; //计时器累加
            
            if(attackTimeVal>= 0.5 - towerLevel * 0.15f)  //计时器判断（塔等级越高，伤害时间越低） ToDO 优化公式
            {
                //可以造成伤害
                canTakeDamage = true;  
                attackTimeVal = 0;   //重置计时器
                DecreaseHP();  //造成伤害
            }   
        }
    }

    //造成伤害
    private void DecreaseHP()
    {
        if (!canTakeDamage || targetTrans == null) //不可以造成伤害
        {
            return;
        }
        if (targetTrans.gameObject.activeSelf) //目标激活时才造成伤害
        {
            //如果当前目标是物体但不是集火目标的情况下
            if(targetTrans.position == null || (targetTrans.tag == "Item" &&  targetTrans != GameController.Instance.targetTrans))
                return;
            //如果当前目标是怪物或者是集火的物体
            if (targetTrans.tag == "Monster" || (targetTrans.tag == "Item" && targetTrans == GameController.Instance.targetTrans))
            {
                targetTrans.SendMessage("TakeDamage", attackValue); //传递伤害
                CreateEffect();
                canTakeDamage = false;
            }
        }
        
    }

    //重写子弹特效方法
    //现在子弹特效的产生位置是目标的位置 （父类中是子弹本身的位置）
    protected override void CreateEffect()
    {
        if (targetTrans.position == null)
            return;
        GameObject effect = GameController.Instance.GetGameObject("Tower/ID" + towerID.ToString() + "/Effect/" + towerLevel.ToString());
        effect.transform.position = targetTrans.position; //特效产生在目标的位置
    }
}
