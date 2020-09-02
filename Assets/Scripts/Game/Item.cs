using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 道具脚本
/// </summary>
public class Item : MonoBehaviour
{
    //属性
    public int itemID;
    private int prize;          //奖励金币
    private int HP;
    private int currentHP;
    private float timeVal;      //计时器 一段时间没有受到攻击后隐藏血条
    private bool showHP;        //血条显示开关

    //引用
    public GridPoint gridPoint; //记录物体所属的格子  用于检测玩家是否销毁所有道具 在创建物品时赋值
    private Slider slider;      //血条 塔只有在受到攻击时才显示血条 


    private void OnEnable()
    {
        if (itemID != 0)
        {
#if Game
        InitItem();
#endif
        }
    }

    // Start is called before the first frame update
    void Start()
    {
#if Tool
        //工具状态下将工具身上的BoxCollider组件失活
        GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Mask").GetComponent<BoxCollider>().enabled = false;
#endif
        slider = transform.Find("ItemCanvas").Find("HpSlider").GetComponent<Slider>();

#if Game
        InitItem();
#endif
        //Debug.Log(itemID);
    }

    // Update is called once per frame
    void Update()
    {
        if (showHP) //正在显示血条
        {
            if(timeVal <= 0)  //计时器到时隐藏血条
            {
                slider.gameObject.SetActive(false);
                showHP = false;
                timeVal = 3.0f; //重置计时器
            }
            else
            {
                timeVal -= Time.deltaTime;  
            }
        }
    }

#if Game
    //初始化道具
    private void InitItem()
    {
        prize = 1000 - 100 * itemID;  //ToDO 优化设计 公式 or JSON
        HP = 1500 - 100 * itemID;     //ToDO 优化设计 公式 or JSON
        currentHP = HP;
        timeVal = 3.0f;
        transform.Find("ItemCanvas").Find("HpSlider").gameObject.SetActive(false); //加载时隐藏血条
    }

    //承受伤害的方法
    private void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true); //显示血条
        currentHP -= attackValue;
        if (currentHP <= 0)
        {
            //播放死亡音效
            DestroyItem();//消灭道具
            return;
        }
        slider.value = (float)currentHP / HP; //更新血条
        showHP = true;  //显示血条
        timeVal = 3.0f; //重置计时器
    }

    //销毁道具
    private void DestroyItem()
    {
        if(GameController.Instance.targetTrans == transform) //如果当前目标是集火目标
        {
            GameController.Instance.HideSignal();  
        }

        //金币奖励
        GameObject coin = GameController.Instance.GetGameObject("CoinCanvas");
        coin.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
        coin.transform.SetParent(GameController.Instance.transform);
        coin.transform.position = transform.position; //设置金币UI的位置(道具的位置)    
        GameController.Instance.ChangeCoin(prize);//增加玩家金币数量

        //销毁特效
        GameObject destroyEffect = GameController.Instance.GetGameObject("DestoryEffect");
        destroyEffect.transform.SetParent(GameController.Instance.transform);
        destroyEffect.transform.position = transform.position; //设置特效位置(怪物的位置)


        //统计数据
        GameController.Instance.PushGameObjectToFactory(GameController.Instance.mapMaker.bigLevelID.ToString() + "/Item/Item_"+itemID, gameObject); //放入对象池
        gridPoint.gridState.hasItem = false;
        InitItem();//初始化道具对象

        GameController.Instance.PlayEffectMusic("NormalModel/Item");
    }

    //鼠标交互
    private void OnMouseDown()
    {
        //如果选择的是UI则不发生交互
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (GameController.Instance.targetTrans == null) //当前没有集火目标
        {
            //设置集火目标
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        else if (GameController.Instance.targetTrans != transform)  //当前有其他的集火目标
        {
            //转换集火目标
            GameController.Instance.HideSignal();
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        else if (GameController.Instance.targetTrans == transform) //点击已经集火的目标
        {
            //取消集火目标
            GameController.Instance.HideSignal();
        }
    }










#endif
}
