using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 塔特性脚本：子弹 攻击属性
/// </summary>
public class TowerSpecificProperty : MonoBehaviour
{
    //属性值
    public int towerLevel;
    protected float timeVal;  //攻击计时器
    public float attackCD;    //攻击CD(速度)
    [HideInInspector]
    public int sellPrice;     //售价
    [HideInInspector]
    public int upLevelPrice;  //升级价格
    public int price;         //当前塔的价格

    //资源
    protected GameObject bulletGo;  //子弹空对象

    //引用
    [HideInInspector]
    public Tower tower;            //控制自己的Tower对象
    public Transform targetTrans;  //攻击目标
    public Animator animator;      //塔攻击动画控制器

    //子类可以重写Start方法
    // Start is called before the first frame update
    protected virtual void Start()
    {
        upLevelPrice = (int)(price * 1.5);    
        sellPrice = price / 2;
        animator = transform.Find("tower").GetComponent<Animator>();
        timeVal = attackCD; //建塔后直接进行一次攻击，此时开始攻击计时
    }

    
    // Update is called once per frame
    protected virtual void Update()
    {
        if (GameController.Instance.isPause || targetTrans == null || GameController.Instance.isGameOver)  //游戏暂停或者丢失目标
        {
            return;
        }

        if (!targetTrans.gameObject.activeSelf) //目标失活
        {
            targetTrans = null;
            return;
        }

        if(timeVal >= attackCD / GameController.Instance.gameSpeed) //可以攻击
        {
            timeVal = 0;  //重置计时器
            Attack();     //攻击
        }
        else      //攻击CD中
        {
            timeVal += Time.deltaTime;
        }

        //塔身的旋转   //旋转变换，使向前矢量指向 targetTrans 的当前位置
        if(targetTrans.tag == "Item") //目标是怪物
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3)); //提高物体的高度到和塔持平
            //transform.rotation = Quaternion.LookRotation(targetTrans.position + new Vector3(0, 0, 3) - transform.position);
        }
        else if(targetTrans.tag == "Monster")
        {
            transform.LookAt(targetTrans);
        }
        //纠正旋转角度
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
        }
       

    }

    //初始化特性脚本
    public void Init()
    {   
        //只需要将tower置空
        tower = null;  
    }

    //销售塔 该方法只负责处理游戏的金币，产生特效并销毁塔 后续处理的方法在ButtonSellTower中
    public void SellTower()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/Tower/TowerSell");
        GameController.Instance.ChangeCoin(sellPrice); //增加金币
        GameObject effectGo = GameController.Instance.GetGameObject("BuildEffect"); //产生特效
        effectGo.transform.position = transform.position;
        Destroy();
    }

    //升级塔  该方法只负责处理游戏的金币，产生特效并销毁塔 后续处理的方法在ButtonUpLevel中
    public void UpLevelTower()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/Tower/TowerUpdate");
        GameController.Instance.ChangeCoin(-upLevelPrice); //扣除金币
        GameObject effectGo = GameController.Instance.GetGameObject("UpLevelEffect"); //产生特效
        effectGo.transform.position = transform.position;
        Destroy();    
    }

    //销毁塔的方法
    protected virtual void Destroy()
    {
        tower.DestroyTower();
    }

    //攻击方法
    protected virtual void Attack()
    {
        if(targetTrans == null) //没有攻击目标
        {
            return;
        }
        animator.Play("Attack");//塔播放攻击动画
        GameController.Instance.PlayEffectMusic("NormalModel/Tower/Attack/"+tower.towerID.ToString());
        //产生子弹并发射
        bulletGo = GameController.Instance.GetGameObject("Tower/ID"+tower.towerID.ToString()+ "/Bullet/"+towerLevel.ToString());
        bulletGo.transform.position = transform.position; //在塔的位置生成子弹
        bulletGo.GetComponent<Bullet>().targetTrans = targetTrans; //给子弹指定攻击对象
        //Debug.Log("targetTrans: " + targetTrans.name);

    }
}
