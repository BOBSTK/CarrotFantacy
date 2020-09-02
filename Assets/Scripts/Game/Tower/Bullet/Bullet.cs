using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹脚本基类
/// </summary>
public class Bullet : MonoBehaviour
{
    //属性
    public float moveSpeed;
    public int attackValue;
    //通过塔的属性来设置子弹效果
    public int towerID;
    public int towerLevel;

    //引用
    [HideInInspector]
    public Transform targetTrans; //攻击目标
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
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
        if(targetTrans == null || !targetTrans.gameObject.activeSelf)
        {
            DestroyBullet();
            return;
        }

        //子弹移动与转向(物品) 
        if(targetTrans.gameObject.tag == "Item")
        {
            //Debug.Log("move to Item:"+targetTrans.name);
           
            //移动
            transform.position = Vector3.Lerp(transform.position, targetTrans.position + new Vector3(0, 0, 3),
                1 / Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3)) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
           
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else if(targetTrans.gameObject.tag == "Monster")
        {
            //移动
            transform.position = Vector3.Lerp(transform.position, targetTrans.position,
                1 / Vector3.Distance(transform.position, targetTrans.position) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            //方向
            transform.LookAt(targetTrans.position );
        }
        //纠正旋转角度
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
    }

    //产生子弹特效的方法
    protected virtual void CreateEffect()
    {
        GameObject effect = GameController.Instance.GetGameObject("Tower/ID" + towerID.ToString()+"/Effect/"+towerLevel.ToString());
        effect.transform.position = transform.position;
    }

    //子弹的销毁
     protected virtual void DestroyBullet()
    {
        targetTrans = null;
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/Bullet/" + towerLevel.ToString(), gameObject);
    }

    //触发检测 (默认只会与目标物体发生触发检测)  //问题 目标是物体 途中遇到怪物 的判定
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
        //Debug.Log(collision.tag);
        if(collision.tag == "Monster" || collision.tag == "Item")
        {
            //Debug.Log("OnTriggerEnter2D " + collision.transform.name);
            if (collision.gameObject.activeSelf)
            {
                //如果目标位置为空,不进行后续操作
                if (targetTrans == null)
                    return;
                //如果触发器是目标对象，执行攻击操作
                if(targetTrans == collision.transform)
                {
                    collision.SendMessage("TakeDamage", attackValue);
                    CreateEffect();
                    DestroyBullet();
                }
            }
        }
    }

    

}
