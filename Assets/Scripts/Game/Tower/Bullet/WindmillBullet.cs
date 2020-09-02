using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 风车子弹
/// </summary>
public class WindmillBullet : Bullet
{
    //属性
    private bool hasTarget;  //是否有目标
    private float timeVal;   //计时器，到时便销毁
    private string targetTag;//目标标签
    
    private void OnEnable()
    {
        targetTag = "";
        hasTarget = false;
        timeVal = 0;
    }

    
    private void InitTarget()
    {
        targetTag = targetTrans.tag;
        if (targetTrans.gameObject.tag == "Item") //目标是物体
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3)); //看向物体
        }
        else if(targetTrans.gameObject.tag == "Monster")//目标是怪物
        {
            transform.LookAt(targetTrans.position); //看向怪物
        }
        //纠正旋转角度
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
    }

    private void Update()
    {
        if (GameController.Instance.isGameOver || timeVal >= 3.0f) //游戏结束或者子弹存在时间超过3.0s
        {
            DestroyBullet();
        }
        if (GameController.Instance.isPause) //暂停
        {
            return;
        }
        if (timeVal < 3.0)
            timeVal += Time.deltaTime;
        if (hasTarget) //有目标
        {
            //根据 translation 的方向和距离移动变换  根据世界坐标移动
            //子弹沿着射出方向向前移动
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }
        else  //没有目标
        {
            //如果子弹有目标对象且是激活状态
            if (targetTrans != null && targetTrans.gameObject.activeSelf)  
            {
                //设置目标
                hasTarget = true;
                InitTarget();
            }
        }
    }

    /// <summary>
    /// 重写子弹触发检测方法
    /// - 攻击目标是怪物时，子弹会穿过怪物并飞出地图边界，会对沿途所有对象(怪物和道具)造成伤害
    /// -攻击目标是道具时，子弹会在接触目标道具后销毁，并对沿途所有对象(怪物和道具) 造成伤害
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(targetTag == "Monster") //目标对象是怪物
        {
            //对沿途所有对象(怪物和道具)造成伤害
            if (collision.tag == "Monster" || collision.tag == "Item")
            {
                if (collision.gameObject.activeSelf)
                {
                    collision.SendMessage("TakeDamage", attackValue);
                    CreateEffect();
                }
            }
        }
        else if(targetTag == "Item")//目标对象是道具
        {
            //在接触到道具时销毁，并对沿途所有对象(怪物和道具) 造成伤害
            if (collision.gameObject.activeSelf)
            {
                //如果目标为空或目标未激活
                if (targetTrans == null || !targetTrans.gameObject.activeSelf)
                    DestroyBullet();
                //如果触发器是怪物 或者是目标道具外的其他道具， 子弹穿过并造车伤害
                if(collision.tag == "Monster" || (collision.tag == "Item" && collision.transform != targetTrans))
                {
                    collision.SendMessage("TakeDamage", attackValue);
                    CreateEffect();
                }
                //如果触发器是目标道具，造车伤害并销毁子弹
                else if (collision.tag == "Item" && collision.transform == targetTrans)
                {
                    collision.SendMessage("TakeDamage", attackValue);
                    CreateEffect();
                    DestroyBullet();
                }
            }
        }


    }

  
}
