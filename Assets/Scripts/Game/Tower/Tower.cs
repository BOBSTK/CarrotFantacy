using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 塔共性脚本  
/// </summary>
public class Tower : MonoBehaviour
{
    public int towerID;  //只存储ID，level存储在TowerSpecificProperty中
    private CircleCollider2D circleCollider2D;  //攻击检测范围
    [HideInInspector]
    private TowerSpecificProperty towerSpecificProperty; //塔的特性脚本
    private SpriteRenderer attackRangeSr; //攻击范围渲染

    private bool isTarget; //是集火目标
    public bool hasTarget; //有目标

    // Start is called before the first frame update
    void Start()
    {
        Init();
       //Debug.Log("circleCollider2D.radius"+circleCollider2D.radius);
       //Debug.Log("attackRangeSr.radius" + attackRangeSr.size);
    }

    private void OnEnable()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.isPause || GameController.Instance.isGameOver)
        {
            return;
        }
        //如果已经找到集火目标，需要监听集火目标的状态
        if (isTarget)  
        {
            //集火目标改变，重置塔的攻击状态
            if (towerSpecificProperty.targetTrans != GameController.Instance.targetTrans)
            {  
                towerSpecificProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }

        //如果有攻击目标，需要监听目标的状态(死亡)
        if (hasTarget)
        {
            //攻击目标死亡(被放入对象池)
            if (!towerSpecificProperty.targetTrans.gameObject.activeSelf)
            {
                //重置塔的攻击状态
                towerSpecificProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
    }

    //初始化方法
    protected void Init()
    {
        //获取相关引用
        circleCollider2D = GetComponent<CircleCollider2D>();
        towerSpecificProperty = GetComponent<TowerSpecificProperty>();
        towerSpecificProperty.tower = this;
        attackRangeSr = transform.Find("attackRange").GetComponent<SpriteRenderer>();
       
        attackRangeSr.gameObject.SetActive(false);  //隐藏塔的攻击范围

        //改变塔的攻击范围  ToDo 优化 
        float range = 2.0f;
        circleCollider2D.radius = 1.1f * towerSpecificProperty.towerLevel * range;   //For Test
        attackRangeSr.transform.localScale = new Vector3(towerSpecificProperty.towerLevel * range, towerSpecificProperty.towerLevel * range, 1);//ForTest
        
        isTarget = false;
        hasTarget = false;
    }

    //摧毁塔的方法
    public void DestroyTower()
    {
        towerSpecificProperty.Init(); //初始化塔的特性脚本
        Init();
        GameController.Instance.PushGameObjectToFactory("Tower/ID"+ towerID.ToString() + "/TowerSet/"+towerSpecificProperty.towerLevel.ToString(), gameObject);
    }

    
    



    //得到特异性方法
    public void GetTowerProperty()
    {

    }

    /// <summary>
    /// 塔检测攻击目标的方法
    /// </summary>
    /// <param name="collision"></param>
    
    //怪物进入
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerEnter2D");
        FindTarget(collision);
       
    }

    //滞留怪物
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerStay2D");
        FindTarget(collision);
        
    }

    //怪物离开
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerExit2D");
        if(towerSpecificProperty.targetTrans == collision.transform) //离开的怪物是正在攻击的目标
        {
            towerSpecificProperty.targetTrans = null; //攻击目标置空
            hasTarget = false;                        //丢失目标
            isTarget = false;                         //丢失集火目标
        }
    }

    //寻找目标的方法 问题：塔在消灭一个指定
    private void FindTarget(Collider2D collision)
    {
       // Debug.Log("Finding Target");
        if (collision.tag != "Monster" && collision.tag != "Item" && isTarget)
        {
            //如果检测到的触发器不属于怪物或物品，或者已经找到集火目标时，不走此方法
            return;
        }
        Transform targetTrans = GameController.Instance.targetTrans; //找到集火目标
        if (targetTrans != null)  //有集火目标
        {
            if (!isTarget)  //没有找到集火目标
            {
                //是物品且是集火目标
                if (collision.tag == "Item" && collision.transform == targetTrans)
                {
                    towerSpecificProperty.targetTrans = collision.transform;//设为攻击目标
                    isTarget = true;
                    hasTarget = true;
                }
                //是怪物
                else if (collision.tag == "Monster")
                {
                    //是集火目标
                    if (collision.transform == targetTrans)
                    {
                        towerSpecificProperty.targetTrans = collision.transform;
                        isTarget = true;
                        hasTarget = true;
                    }
                    //不是集火目标且当前没有攻击目标
                    else if (collision.transform != targetTrans && !hasTarget)
                    {
                        towerSpecificProperty.targetTrans = collision.transform;
                        hasTarget = true;
                    }
                }
            }
        }
        else   //没有集火目标
        {
            if (!hasTarget && collision.tag == "Monster")  //没有攻击目标且进入的是怪物
            {
                towerSpecificProperty.targetTrans = collision.transform;  //设为攻击目标
                hasTarget = true;
            }
        }
    }

    private void OnMouseDown()
    {
        //如果选择的是UI则不发生交互
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Debug.Log("点击到塔的攻击触发器了");
       
    }
}
