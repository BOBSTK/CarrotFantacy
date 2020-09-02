using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 便便塔的子弹
/// 减速
/// </summary>

//便便子弹特殊属性
struct BulletProperty
{
    public  float debuffTime;    //Debuff时间
    public  float debuffValue;   //Debuff值 (减速数值)
}
 

public class TShitBullet : Bullet
{
    private BulletProperty bulletProperty;
    // Start is called before the first frame update
    void Start()
    {
        bulletProperty = new BulletProperty
        {
            debuffTime = towerLevel * 0.4f,
            debuffValue = towerLevel * 0.3f
        };
    }

   


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!collision.gameObject.activeSelf)
            return;
        //如果触发器是怪物且是攻击目标，触发怪物的减速效果
        if (collision.tag == "Monster" && collision.transform == targetTrans)
            collision.SendMessage("ShitDecrease", bulletProperty);
        base.OnTriggerEnter2D(collision);
    }
}
