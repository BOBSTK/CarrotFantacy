using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏控制管理，负责控制游戏的整个逻辑
/// </summary>
public class GameController : MonoBehaviour
{
    private static GameController _instance; //单例
    private GameManager gameManager;

    //引用
    public Level level; //负责生成和控制本关的责任链
    public int[] monsterIDList; //当前波次的产怪列表  在每一波的Round对象调用Handle方法时赋值
    public int monsterIDIndex;  //产怪索引
    public Stage currentStage;  //当前关卡的Stage对象
    public NormalModelPanel normalModelPanel; //冒险模式下游戏中的UI面板·
    public MapMaker mapMaker; //生成地图的地图编辑器(游戏中)
    public Transform targetTrans; //集火目标(怪物OR物品)
    public GameObject targetSignal; //集火信号
    public GridPoint selectGrid;   //上一个选中的格子
    


    //游戏资源
    public RuntimeAnimatorController[] controllers; //怪物动画播放控制器

    //用于计数的成员变量
    public int monsterKilledNum;  ///该轮击杀怪物数量(只有在该轮所有怪物都被击杀或接触到萝卜时，才会刷新下一轮怪物)
    public int itemClearedNum;    //销毁道具数量
    public int monsterKilledTotalNum;  //杀怪总数

    //属性
    public int carrotHP = 10;
    public int coin;
    public int gameSpeed;     //游戏进行速度
    public bool isPause;
    public bool isCreatingMonster; //是否继续产怪
    public bool isGameOver;        //游戏是否结束
   
   


    //建造者
    public MonsterBuilder monsterBuilder;
    public TowerBuilder towerBuilder;

    //建塔有关的成员变量
    //ToDo 使用JSON存储
    public Dictionary<int, int> towerPriceDict;   //建塔价格表
    public GameObject towerListGO;  //建塔按钮列表  拖拽赋值
    public GameObject handleTowerCanvas;  //处理塔升级和买卖的画布  拖拽赋值


    public static GameController Instance { get => _instance;  }

    
    
    private void Awake()
    {
        
# if Game
        _instance = this;
        gameManager = GameManager.Instance;

        //For Test
        //currentStage = new Stage(10,new int[] { 1,2,3,4,5},false,0,1,1,true,false);
        currentStage = gameManager.currentStage; //获取关卡信息
        //获取游戏场景面板
        normalModelPanel = gameManager.uiManager.uiFacade.currentScenePanelDict[StringManager.NormalModelPanel] as NormalModelPanel;
        normalModelPanel.EnterPanel();
        //加载地图
        mapMaker = GetComponent<MapMaker>();
        mapMaker.InitMapMaker();
        mapMaker.LoadMap(currentStage.bigLevelID, currentStage.levelID); //根据玩家选择的场景加载地图
     
      
       
        //成员变量赋值
        gameSpeed = 1;
        coin = 1000;

        monsterBuilder = new MonsterBuilder();
        towerBuilder = new TowerBuilder();

        //建塔列表的处理
        for (int i = 0; i < currentStage.towerIDList.Length; i++)
        {
            GameObject item = gameManager.GetGameObject(FactoryType.UIFactory, "Btn_TowerBuild");  //获取建塔按钮预制体
            item.transform.GetComponent<ButtonTower>().towerID = currentStage.towerIDList[i];  //将当前塔的ID赋值给ButtonTower
            item.transform.SetParent(towerListGO.transform); //把父对象设为TowerList
            //调整位置和大小
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
        }

        //塔的价格表 ToDo 优化
        towerPriceDict = new Dictionary<int, int>
        {
            {1,100 },
            {2,120 },
            {3,140 },
            {4,160 },
            {5,160 }
        };
        


        controllers = new RuntimeAnimatorController[12];//每关最多有12种怪物
        //获得当前关卡所有怪物的动画播放器资源
        for (int i = 0; i < controllers.Length; i++)
        {     
            controllers[i] = GetController("Monster/"+mapMaker.bigLevelID.ToString()+"/"+(i+1).ToString());
        }

        level = new Level(mapMaker.roundInfoList.Count, mapMaker.roundInfoList); //创建Level对象
        
        //更新顶部UI的信息
        normalModelPanel.topPage.UpdateCoinText();
        normalModelPanel.topPage.UpdateRoundText();

        //level.HandleRound();
        isPause = true;  //等开场动画完后再产生怪物
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(currentStage.bigLevelID +" "+ currentStage.levelID);
        }
        //Debug.Log("killMonsterNum: " + killMonsterNum + " monsterIDList.length:" + monsterIDList.Length);
#if Game
        if (!isPause && !isGameOver)
        {
            //产怪逻辑
            //监听杀怪总数
                if (monsterKilledNum >= monsterIDList.Length)
            {
                //添加当前回合数的索引(进入下一回合,如果到达最后一回合显示 最后一轮的逻辑，如果超过则游戏胜利)
                if (level.currentRound >= level.totalRound)
                {
                    return;
                }
                AddRoundNum();
            }
            else
            {
                if (!isCreatingMonster)
                {
                    CreateMonster(); //确保只调用一次产怪方法 只有在解除暂停状态时调用一次
                }
            }
        }
        else
        {
            //暂停状态（玩家按下暂停按钮 或者是得到礼物 或者是游戏结束）
            StopCreateMonster(); //停止产怪
            isCreatingMonster = false;
            
        }
#endif
    }




    /// <summary>
    /// 与玩家信息有关的方法
    /// </summary>
    //改变玩家的金币
    public void ChangeCoin(int coinNum)
    {
        coin += coinNum;
        //更新游戏UI显示
        normalModelPanel.UpdatePanel();
    }

    //判断当前关卡道具是否全部清除的方法  toDO 在没消除一个道具时检查，清除所有道具后应该弹窗提示
    public bool IsAllClear()
    {
        //遍历场景中所有的格子
        for (int x = 0; x < MapMaker.xColum ; x++)
        {
            for (int y = 0; y < MapMaker.yRow; y++)
            {
                if (mapMaker.gridPoints[x, y].gridState.hasItem)
                {
                    return false;
                }
            }
        }
        return true;
    }

    //获取萝卜状态
    public int GetCarrotState()
    {
        int carrotState = 0;
        if(carrotHP == 10)
        {
            carrotState = 1;  //金
        }
        else if (carrotHP >= 6)
        {
            carrotState = 2;  //银
        }
        else
        {
            carrotState = 3;  //铜
        }
        return carrotState;
    }


    /// <summary>
    /// 处理集火目标的方法
    /// </summary>
    
    //显示集火信号
    public void ShowSignal()
    {
        PlayEffectMusic("NormalModel/Tower/ShootSelect");
        //将信号位置设在选中目标的上方(半格)
        targetSignal.transform.position = targetTrans.position + new Vector3(0,mapMaker.gridHeight/2,0);
        targetSignal.transform.SetParent(targetTrans); //信号需要跟随怪物移动
        targetSignal.SetActive(true);
    }

    //隐藏集火信号(再次点击集火目标或者目标被消灭时应该隐藏集火信号)
    public void HideSignal()
    {
        targetSignal.gameObject.SetActive(false);
        targetTrans = null;
    }

    /// <summary>
    /// 产怪方法
    /// </summary>
    public void CreateMonster()
    {
        isCreatingMonster = true;
        //延时调用方法产生怪物（由Round对象调用）
        InvokeRepeating("InstantaiteMonster",1.0f/gameSpeed,1.0f/gameSpeed);  //在 1.0f/gameSpeed 秒后调用 methodName 方法，然后每 1.0f/gameSpeed 秒调用一次
    }

    //具体产怪的方法
    private void InstantaiteMonster()
    {
        PlayEffectMusic("NormalModel/Monster/Create");
        if (monsterIDIndex >= monsterIDList.Length)
        {
            //已经产完了本轮所有怪物
            StopCreateMonster();
            return;
        }
        //产生特效
        GameObject effect = GetGameObject("CreateEffect");
        effect.transform.SetParent(transform);
        effect.transform.position = mapMaker.monsterPathPos[0];  //产怪特效在怪物路径起点产生
        //产生怪物
        if (monsterIDIndex < monsterIDList.Length)
        {   
            //通过产怪索引获得怪物ID
            monsterBuilder.monsterID = level.roundList[level.currentRound].roundInfo.monsterIDList[monsterIDIndex];
        }
        GameObject monsterGO = monsterBuilder.GetProduct();
        monsterGO.transform.SetParent(transform);
        monsterGO.transform.position = mapMaker.monsterPathPos[0];
        ++monsterIDIndex;
     
    }

    //停止产怪方法
    public void StopCreateMonster()
    {
        CancelInvoke(); //取消该 MonoBehaviour 上的所有 Invoke 调用
    }

    //进入下一个回合，交给下一个Round对象处理产怪(责任链)
    public void AddRoundNum()
    {
       // Debug.Log("回合" + level.currentRound + "结束");
        //重置回合数据
        monsterIDIndex = 0; //重置产怪索引
        monsterKilledNum = 0;
        level.AddRoundNum(); //进入下一回合（Level）
        level.HandleRound(); //回合处理(责任链)
        //更新回合显示的UI
        normalModelPanel.UpdatePanel();
    }



    /// <summary>
    /// 与游戏处理逻辑有关的方法
    /// </summary>

#if Game
    //开始游戏
    public void StartGame()
    {
        //Debug.Log("Start Game");
        isPause = false;
        level.HandleRound(); //开始产怪
    }

    //格子处理的方法 grid - 当前选中的格子
    public void HandleGrid(GridPoint grid)
    {
        if (grid.gridState.canBuild) //当前格子可以建塔
        {
            if(selectGrid == null)  //没有上一个格子
            {
                selectGrid = grid;
                selectGrid.ShowGrid();
                //播放音效
                PlayEffectMusic("NormalModel/Grid/GridSelect");
            }
            else if(grid == selectGrid) //选中同一个格子
            {
                selectGrid.HideGrid(); //隐藏当前格子
                selectGrid = null;  //没有选中任何(上一个)格子
                //播放音效
                PlayEffectMusic("NormalModel/Grid/GridDeselect");
            }
            else if(grid != selectGrid)  //选中不同的格子
            {
                selectGrid.HideGrid();
                selectGrid =grid;
                selectGrid.ShowGrid();
                //播放音效
                PlayEffectMusic("NormalModel/Grid/GridSelect");
            }
        } 
        else                         //当前格子不可以建塔
        {
            //Debug.Log("不能建塔");
            //grid.HideGrid();         //隐藏当前格子
            PlayEffectMusic("NormalModel/Grid/SelectFault");
            grid.ShowCantBuild();
            if(selectGrid != null)
            {
                selectGrid.HideGrid(); //隐藏上一个塔
            }

        }
    }

    //萝卜血量
    public void DecreaseHP()
    {
        --carrotHP;
        //播放音效
        PlayEffectMusic("NormalModel/Carrot/Crash");
        //更新萝卜UI
        mapMaker.carrot.UpdateCarrotUI();
    }

    //打开奖品页面
    public void ShowPrizePage()
    {
      
        normalModelPanel.ShowPrizePage();
    }
# endif

    /// <summary>
    /// 资源获取的方法
    /// </summary>
    public Sprite GetSprite(string resourceName)
    {
        return gameManager.GetSprite(resourceName);
    }

    public AudioClip GetAudioClip(string resourceName)
    {
        return gameManager.GetAudioClip(resourceName);
    }

    public RuntimeAnimatorController GetController(string resourceName)
    {
        return gameManager.GetController(resourceName);
    }

    public GameObject GetGameObject(string itemName)
    {
        return gameManager.GetGameObject(FactoryType.GameFactory, itemName); //只会去GameFactory取资源
    }

    public void PushGameObjectToFactory(string itemName, GameObject item)
    {
        gameManager.PushGameObjectToFactory(FactoryType.GameFactory,itemName, item);
    }

    /// <summary>
    /// 播放音效的方法
    /// </summary>
    public void PlayEffectMusic(string audioClipPath)
    {
        gameManager.audioSourceManager.PlayEffectMusic(GetAudioClip(audioClipPath));
    }
    public void PlayButtonAudioClip()
    {
        gameManager.audioSourceManager.PlayButtonAudioClip();
    }

}
