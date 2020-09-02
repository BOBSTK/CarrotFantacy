using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Monster : MonoBehaviour
{
    //属性值
    public int monsterID;
    public int HP;          //总血量
    public int currentHP;   //当前血量
    public float moveSpeed; //实际速度(游戏速度，减速) 
    public float initMoveSpeed; //初始速度
    public int prize; //奖励金钱

    //引用
    public Animator animator;
    private Slider slider;   //滑动条
    
    private List<Vector3> monsterPoint;

    //用于计数的属性或开关
    private int roadPointIndex = 1;  //下一个路径点的索引(怪物初始在0，走向1)
    private bool isReachCarrot; //到达终点
  

    //资源
    public AudioClip dieAudioClip; //死亡动画
    public RuntimeAnimatorController runtimeAnimatorController; //在运行期间更改Animator Controller

    //Debuff相关的属性和资源
    //便便
    public GameObject Tshit; //减速特效(便便)
    private bool isShitDecrease; //是否被便便减速
    private float shitDecreaseSpeedTimeVal; //减速计时器
    private float shitDecreaseTime;   //减速持续时间


    private void Awake()
    {
        animator = GetComponent<Animator>();
        slider = transform.Find("MonsterCanvas").Find("HPSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false); //只有在怪物受到攻击时才显示血条
        monsterPoint = GameController.Instance.mapMaker.monsterPathPos;
        Tshit = transform.Find("TShit").gameObject;
    }

    private void Update()
    {
        //怪物的移动
        //----------
        if (GameController.Instance.isPause || GameController.Instance.isGameOver)
        {
            //暂停状态或游戏结束时怪物停止
            return;
        }
        if (!isReachCarrot) //没有到达终点
        {
            //怪物移动的方法  (使用路径点间线性插值的方法插值 t 限制在范围 [0, 1] 内  默认速度是每秒走0.1)
            transform.position = Vector3.Lerp(transform.position, monsterPoint[roadPointIndex], 
                //单位化(让怪物匀速运动)
                1/Vector3.Distance(transform.position, monsterPoint[roadPointIndex]) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            if(Vector3.Distance(transform.position, monsterPoint[roadPointIndex]) <= 0.01f) //认为怪物走到了一个检查点
            {
                //怪物到达了下一个路径点
                //怪物转向
                if(roadPointIndex+1 < monsterPoint.Count)
                {
                    //没有到终点
                    float xOffset = monsterPoint[roadPointIndex].x - monsterPoint[roadPointIndex + 1].x;
                    if (xOffset < 0)//向右
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (xOffset > 0) //向左
                    {
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                }
                slider.gameObject.transform.eulerAngles = Vector3.zero; //调整血条的旋转
                ++roadPointIndex;  //设置下一个路径点
                if(roadPointIndex >= monsterPoint.Count)
                {
                    //到达了最后一个路径点(接触到萝卜)
                    isReachCarrot = true;
                }
            }  
        }
        else //到达终点
        {
            //销毁怪物对象
            DestroyMonster();
            //萝卜减血
#if Game
            GameController.Instance.DecreaseHP();
#endif
        }

        if (isShitDecrease) //处于便便减速状态
        {
            shitDecreaseSpeedTimeVal += Time.deltaTime; //计时
        }
        if(shitDecreaseSpeedTimeVal >= shitDecreaseTime) //减速到时
        {
            CancelDecreaseDebuff();
            shitDecreaseSpeedTimeVal = 0; //重置计时器
        }
        
    }


    //销毁怪物的方法
    private void DestroyMonster()
    {
        //如果集火目标被销毁，要调用隐藏集火目标的方法
        if(GameController.Instance.targetTrans == transform)
        {
            GameController.Instance.HideSignal();
        }

        if (!isReachCarrot) //被炮台击杀
        {
            //生成金币以及数目 （UI）
            GameObject coin = GameController.Instance.GetGameObject("CoinCanvas");
            GameObject emp_coin = coin.transform.Find("Emp_Coin").gameObject;
            //Debug.Log("Monster" + monsterID + " : " + prize);

            //coin.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
            coin.transform.SetParent(GameController.Instance.transform);
            coin.transform.position = transform.position; //设置金币UI的位置(怪物的位置)
            emp_coin.GetComponent<CoinMove>().prize = prize;
            if (!emp_coin.activeSelf)
                emp_coin.SetActive(true);
            //增加玩家金币数量
            GameController.Instance.ChangeCoin(prize);
            //奖品的掉落
            int random = Random.Range(0, 100); //0-99的随机数
            if (random < 10) //有百分之十的几率掉落奖品
            {
                GameObject prizeGo = GameController.Instance.GetGameObject("Prize");
                prizeGo.transform.position = transform.position;
                //播放音效
                GameController.Instance.PlayEffectMusic("NormalModel/GiftGot");
            }

        }
        //产生销毁特效
        GameObject destroyEffect = GameController.Instance.GetGameObject("DestoryEffect");
        destroyEffect.transform.SetParent(GameController.Instance.transform);
        destroyEffect.transform.position = transform.position; //设置特效位置(怪物的位置)

        //统计数据
        GameController.Instance.monsterKilledNum++;       
        GameController.Instance.monsterKilledTotalNum++;
        InitMonster(); //初始化怪物对象
        GameController.Instance.PushGameObjectToFactory("MonsterPrefab", gameObject); //放入对象池
    }

    //初始化怪物的方法
    private void InitMonster()
    {
        monsterID = 0;
        HP = 0;
        currentHP = 0;
        moveSpeed = 0;
        roadPointIndex = 1;
        dieAudioClip = null;
        isReachCarrot = false;
        slider.value = 1;
        slider.gameObject.SetActive(false);
        prize = 0;
        transform.eulerAngles = Vector3.zero;
        shitDecreaseSpeedTimeVal = 0;
        shitDecreaseTime = 0;
        CancelDecreaseDebuff();
    }

    //承受伤害的方法
    private void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true); //显示血条
        currentHP -= attackValue;
        if (currentHP <= 0)
        {
            //播放死亡音效
            GameController.Instance.PlayEffectMusic("NormalModel/Monster/"+GameController.Instance.currentStage.bigLevelID.ToString()+"/"+monsterID.ToString());
            DestroyMonster();//消灭怪物
            return;
        }
        slider.value = (float)currentHP / HP; //更新血条
    }

    //减速Debuff的方法
    private void ShitDecrease(BulletProperty bulletProperty)
    {
        if (!isShitDecrease) //如果之前没有被减速
        {
            moveSpeed = moveSpeed - bulletProperty.debuffValue; //减速
            Tshit.SetActive(true);   //减速效果
        }
        shitDecreaseSpeedTimeVal = 0;//每次收到减速攻击时，重置减速计时器
        shitDecreaseTime = bulletProperty.debuffTime;  //更新减速  
        isShitDecrease = true;     //打开减速开关
    }

    //移除怪物减速Debuff
    private void CancelDecreaseDebuff()
    {
        isShitDecrease = false;
        moveSpeed = initMoveSpeed; //恢复初始速度
        Tshit.SetActive(false);
    }

    //怪物对象可能频繁出入对象池，需要在激活时重新赋值
    private void OnEnable()
    {
        monsterPoint = GameController.Instance.mapMaker.monsterPathPos;  //激活时要对怪物路径重新赋值
        // Debug.Log("roadPointIndex" + roadPointIndex + " monsterPoint:" + monsterPoint.Count);
        //初始朝向
        if (roadPointIndex + 1 < monsterPoint.Count)
        {
            //没有到终点
            float xOffset = monsterPoint[0].x - monsterPoint[1].x;
            if (xOffset < 0)//向右
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (xOffset > 0) //向左
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    //设置其他属性 (设置怪物动画)
    public void GetMonsterProperty()
    {
        animator.runtimeAnimatorController = GameController.Instance.controllers[monsterID - 1]; //设置怪物动画
    }

#if Game
    private void OnMouseDown()
    {
        //如果选择的是UI则不发生交互
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Debug.Log("点击到怪物: " + monsterID +"当前位置: "+transform.position+" 正走向路点: "+monsterPoint[roadPointIndex]);
        //Debug.Log("当前路径点:");
        //foreach (var item in monsterPoint)
        //{
        //    Debug.Log(item);
        //}
        if (GameController.Instance.targetTrans == null) //当前没有集火目标
        {
            //设置集火目标
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        else if(GameController.Instance.targetTrans != transform)  //当前有其他的集火目标
        {
            //转换集火目标
            GameController.Instance.HideSignal();  
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        else if(GameController.Instance.targetTrans == transform) //点击已经集火的目标
        {
            //取消集火目标
            GameController.Instance.HideSignal();
        }
    }
#endif

}
