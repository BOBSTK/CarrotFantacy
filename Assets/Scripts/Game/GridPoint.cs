using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
///  格子脚本
///  功能：处理格子的信息，如是否可以建塔，是否有道具，是否是道路
///  存储塔的信息以及玩家与塔交互的方法
/// </summary>
public class GridPoint : MonoBehaviour
{
    //属性
    private SpriteRenderer spriteRenderer;  //用于控制格子的渲染
    public GridState gridState;
    public GridIndex gridIndex;  //作为工具使用时在MapMaker中赋值
    public bool hasTower;  //格子上是否建塔
    //有关塔的操作的两个Button的初始位置
    private Vector3 upLevelInitPos;    
    private Vector3 sellTowerInitPos;


    //资源
    private Sprite gridSprite;        //格子图片资源 
    private Sprite startSprite;       //开始时格子的图片显示(过场，提示哪些地方可以建塔)
    private Sprite cantBuildSprite;   //禁止建塔的图片

    //引用
    private GameObject towerListGo;   //建塔列表
    private Transform upLevelButtonTrans;  //升级塔的按钮
    private Transform sellTowerButtonTrans; //出售塔的按钮
    public GameObject handleTowerCanvas; //有塔后的操作按钮Canvas


    //有塔之后的属性
    public GameObject towerGo;  //塔的对象
    public Tower tower;         
    public TowerSpecificProperty towerSpecificProperty;
    public int towerLevel;
    private GameObject levelUpSignal;  //显示塔可以升级的信号



# if Tool
    private Sprite monsterPathSprite; //怪物路径图片资源
    public GameObject[] itemPrefabs;  //道具数组
    public GameObject currentItem;    //当前格子道具
#endif

    //格子状态
    public struct GridState
    {
        public bool canBuild;      //是否可以建塔
        public bool isMonsterPath; //是否是怪物路径
        public bool hasItem;       //是否有道具
        public int itemID;
    }

    //格子索引
    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;
    }


    private void Awake()
    {
#if Tool
        //资源获取 格子图片 怪物路径图片 物体图片
        gridSprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/Grid");
        monsterPathSprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/1/Monster/6-1");
        UpdateItem();
#endif
        spriteRenderer = GetComponent<SpriteRenderer>();
#if Game
        gridSprite = GameController.Instance.GetSprite("NormalModel/Game/Grid");
        startSprite = GameController.Instance.GetSprite("NormalModel/Game/StartSprite");
        cantBuildSprite = GameController.Instance.GetSprite("NormalModel/Game/cantBuild");
        spriteRenderer.sprite = startSprite;

        towerListGo = GameController.Instance.towerListGO;
        handleTowerCanvas = GameController.Instance.handleTowerCanvas;

        upLevelButtonTrans = handleTowerCanvas.transform.Find("Btn_UpLevel");
        sellTowerButtonTrans = handleTowerCanvas.transform.Find("Btn_Sell");
        //设置两个按钮的初始位置
        upLevelInitPos = upLevelButtonTrans.localPosition;
        sellTowerInitPos = sellTowerButtonTrans.localPosition;

        levelUpSignal = transform.Find("UpLevelSignal").gameObject;
        levelUpSignal.SetActive(false);

        //开场动画 
        Tween tween = DOTween.To(() => spriteRenderer.color,
            toColor => spriteRenderer.color = toColor,
           new Color(1, 1, 1, 0), 1.0f
            );
        //tween.SetAutoKill(false);
        tween.SetEase(Ease.InQuad);
        tween.SetLoops(3, LoopType.Yoyo).OnComplete(ChangeSpriteToGrid); //循环调用三次动画
     
#endif
        
        InitGrid();
    }

    private void Update()
    {
        if (levelUpSignal != null)  //安全判定
        {
            if (hasTower)  //有塔
            {
                if(GameController.Instance.coin >= towerSpecificProperty.upLevelPrice && towerLevel<3)
                {
                    //金币足够升级且塔可以升级
                    levelUpSignal.SetActive(true);
                }
                else
                {
                    //不可以升级
                    levelUpSignal.SetActive(false);
                }
            }
            else  //没有塔
            {
                if (levelUpSignal.activeSelf)  //玩家卖掉了可以升级的塔需要隐藏掉信号
                {
                    levelUpSignal.SetActive(false);
                }
            }
        }
    }


#if Tool
    //切换到新关卡后更新物品信息
    public void UpdateItem()
    {
        itemPrefabs = new GameObject[10];
        string prefabsPath = "Prefabs/Game/" + MapMaker.Instance.bigLevelID.ToString() + "/Item/Item_";
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            itemPrefabs[i] = Resources.Load<GameObject>(prefabsPath + i);
            if (!itemPrefabs[i])
            {
                Debug.Log("加载失败，失败路径：" + prefabsPath);
            }
        }
    }
#endif


    //开场后改回原来的格子图片样式
    private void ChangeSpriteToGrid()
    {
        //将Render重置
        spriteRenderer.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        if (gridState.canBuild)
        {
            //可以建塔
            spriteRenderer.sprite = gridSprite;
        }
        else
        {
            //不可以建塔(怪物路径，地图边缘)
            spriteRenderer.sprite = cantBuildSprite;
        }
    }

//初始化格子状态
public void InitGrid()
    {
        //重置格子状态
        gridState.canBuild = true;
        gridState.isMonsterPath = false;
        gridState.hasItem = false;
        //Debug.Log(spriteRenderer);
        spriteRenderer.enabled = true;
        gridState.itemID = -1;


#if Tool
        //将格子上的物品销毁并重置为格子图片
        spriteRenderer.sprite = gridSprite;
        Destroy(currentItem);
#endif

#if Game 
        //重置格子上塔的相关属性
        towerGo = null;
        towerSpecificProperty = null;
        tower = null;
        hasTower = false;
        towerLevel = 0;
#endif
    }

# if Game
    //游戏中更新格子的方法
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            //可以建塔
            spriteRenderer.enabled = true; //启用格子渲染
            if (gridState.hasItem)
            {
                //有物品
                CreateItem();
            }
        }
        else
        {
            //不可以建塔
            spriteRenderer.enabled = false;
        }
    }

    //创建物品
    private void CreateItem()
    {
        //获取当前格子上的物体对象
        GameObject item = GameController.Instance.GetGameObject( GameController.Instance.mapMaker.bigLevelID.ToString()+"/Item/Item_" + gridState.itemID);
        item.transform.SetParent(GameController.Instance.transform);
        Vector3 createPos = transform.position - new Vector3(0, 0, 3);//将物体渲染得离摄像机更近，挡住格子
        if(gridState.itemID  <= 2)
        {
            //调整物体的位置
            //--------------
            //将物体向右向下各移半格
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth , -GameController.Instance.mapMaker.gridHeight)/2;
        }
        else if (gridState.itemID <= 4)
        {
            //将物体右移半格
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth, 0) / 2; 
        }
        item.transform.position = createPos; //设置物体的位置
        item.GetComponent<Item>().gridPoint = this; //将当前格子赋值给物体的Item脚本
        item.GetComponent<Item>().itemID = gridState.itemID;
    }

    /// <summary>
    /// 有关游戏中格子处理的方法
    /// </summary>

    //建塔后的处理方法
    public void AfterBuild()
    {
        spriteRenderer.enabled = false; //关闭格子的渲染
        //对塔的后续处理
        towerGo = transform.GetChild(1).gameObject;  //获取格子下的塔对象  0号子对象是 UpLevelSignal
        tower = towerGo.GetComponent<Tower>();
        towerSpecificProperty = towerGo.GetComponent<TowerSpecificProperty>();
        towerLevel = towerSpecificProperty.towerLevel;
    }

    private void OnMouseDown()
    {
        //如果选择的是UI则不发生交互
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        GameController.Instance.HandleGrid(this); //如果选择的不是UI，则进入处理格子的方法
    }

    //显示选中的格子
    public void ShowGrid()
    {
        if (!hasTower)   //没有塔
        {
            spriteRenderer.enabled = true; //显示格子
            //显示建塔列表
            towerListGo.transform.position = CorrectTowerListPosition(); //显示在当前格子的位置
            towerListGo.SetActive(true);
        }
        else             //有塔
        {
            //显示塔的操作列表
            handleTowerCanvas.transform.position = transform.position;
            CorrectHandleTowerCanvasPosition();
            handleTowerCanvas.SetActive(true);
            //显示塔的攻击范围
            towerGo.transform.Find("attackRange").gameObject.SetActive(true);
        }
    }

    //隐藏格子
    public void HideGrid()
    {
        if (!hasTower)   //没有塔
        {
            //隐藏建塔列表
            towerListGo.SetActive(false);
        }
        else             //有塔
        {
            handleTowerCanvas.SetActive(false);
            //隐藏塔的范围
            towerGo.transform.Find("attackRange").gameObject.SetActive(false);
        }
        spriteRenderer.enabled = false;
    }

    //纠正建塔列表的位置  //ToDo 优化
    private Vector3 CorrectTowerListPosition()
    {
        Vector3 correctPosition = Vector3.zero;
        //格子 8行12列
        if(gridIndex.xIndex <=3 && gridIndex.xIndex >= 0)  //左边界
        {
            correctPosition += new Vector3(GameController.Instance.mapMaker.gridWidth,0,0); //向右移一个格子的宽度
        }
        else if (gridIndex.xIndex <= 11 && gridIndex.xIndex >= 8)//右边界
        {
            correctPosition -= new Vector3(GameController.Instance.mapMaker.gridWidth, 0, 0); //向左移一个格子的宽度
        }

        if (gridIndex.yIndex <= 3 && gridIndex.yIndex >= 0)//下边界
        {
            correctPosition += new Vector3(0, GameController.Instance.mapMaker.gridHeight, 0); //向上移一个格子的高度
        }
        else if (gridIndex.yIndex <= 7 && gridIndex.yIndex >= 4)//上边界
        {
            correctPosition -= new Vector3(0, GameController.Instance.mapMaker.gridHeight, 0); //向下移一个格子的高度
        }
        
        return correctPosition+transform.position;
    }

    //纠正塔的操作列表画布的方法  //理解
    private void CorrectHandleTowerCanvasPosition()
    {
        upLevelButtonTrans.localPosition = Vector3.zero;
        sellTowerButtonTrans.localPosition = Vector3.zero;

        
        if (gridIndex.yIndex <= 0) //下边界
        {
            if(gridIndex.xIndex == 11)//右边界
            {
                //卖塔按钮向左移3/4格
                sellTowerButtonTrans.position -= new Vector3(GameController.Instance.mapMaker.gridWidth*3/4,0,0);
            }
            else
            {   //卖塔按钮向右移3/4格
                sellTowerButtonTrans.position += new Vector3(GameController.Instance.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            //升级按钮保持初始位置
            upLevelButtonTrans.localPosition = upLevelInitPos;
        }
        else if(gridIndex.yIndex >= 6)//上边界
        {
            if (gridIndex.xIndex == 11)//右边界
            {
                //升级按钮向左移3/4格
                upLevelButtonTrans.position -= new Vector3(GameController.Instance.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            else
            {   //升级按钮向右移3/4格
                upLevelButtonTrans.position += new Vector3(GameController.Instance.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            //卖塔按钮保持初始位置
            sellTowerButtonTrans.localPosition = sellTowerInitPos;
        }
        else
        {
            //保持默认位置
            sellTowerButtonTrans.localPosition = sellTowerInitPos;
            upLevelButtonTrans.localPosition = upLevelInitPos;
        }
        //Debug.Log(sellTowerButtonTrans.localPosition);
        //Debug.Log(sellTowerButtonTrans.position);
        //
        //Debug.Log(upLevelButtonTrans.localPosition);
        //Debug.Log(upLevelButtonTrans.position);
    }

  
    //不可建造的格子显示提示
    //ToDo 优化 重复点击时刷新动画
    public void ShowCantBuild()
    {   
        DOTween.Complete("ShowCantBuild", true);//完成前一个动画
        spriteRenderer.enabled = true;  //显示不能建塔的提示图片
        Tween t = DOTween.To(() => spriteRenderer.color,
            toColor => spriteRenderer.color = toColor,
           new Color(1, 1, 1, 0), 2.0f
            ).SetId("ShowCantBuild");

        t.OnComplete(() =>
        {
            spriteRenderer.enabled = false;  //关闭Render
            spriteRenderer.color = new Color(1, 1, 1, 1); //重置颜色
        }
            );
    }


# endif

#if Tool
    //设置关卡的方法
    private void OnMouseDown()
    {
        //Debug.Log("GridIndex" + gridIndex.xIndex + gridIndex.yIndex);
        
        if (Input.GetKey(KeyCode.P))
        {
            //怪物路径
            gridState.canBuild = false;    //不可以建塔
            spriteRenderer.enabled = true; //开启格子渲染
            gridState.isMonsterPath = !gridState.isMonsterPath; //转变怪物路径属性
            if (gridState.isMonsterPath)
            {
                //是怪物路径
                MapMaker.Instance.monsterPath.Add(gridIndex); //添加到怪物路径列表中
                spriteRenderer.sprite = monsterPathSprite;    //显示怪物图片，表示该格是怪物路径
            }
            else
            {
                //不是怪物路径
                MapMaker.Instance.monsterPath.Remove(gridIndex);  //移除怪物路径
                spriteRenderer.sprite = gridSprite;               //显示格子图片
                gridState.canBuild = true;                        //可以建塔
            }
        }
        else if (Input.GetKey(KeyCode.I))
        {
            //道具
            gridState.itemID++; //连续点击鼠标切换道具
            //防止越界
            if(gridState.itemID == itemPrefabs.Length)
            {
                gridState.itemID = -1;  
                Destroy(currentItem);   //销毁当前道具
                gridState.hasItem = false;
                return;
            }
            if(currentItem == null)
            {
                //没有道具
                CreateItem();
            }
            else
            {
                //有道具
                Destroy(currentItem);  //先销毁再产生
                CreateItem();
            }
            gridState.hasItem = true;
        }
        else if (!gridState.isMonsterPath)   
        {
            //没有按键，且不是怪物路径
            gridState.isMonsterPath = false;
            gridState.canBuild = !gridState.canBuild;  //切换建造属性
            if (gridState.canBuild)
            {
                spriteRenderer.enabled = true;  //可以建塔则开启渲染
            }
            else {
                spriteRenderer.enabled = false; //不可以建塔，关闭渲染
            }
        }
    }

    //产生道具
    private void CreateItem()
    {
        Vector3 createPos = transform.position;
        //调整物体的位置
        //--------------
        //大物体(四格) itemID<=2
        if (gridState.itemID <= 2)
        {
            //将物体向右向下各移半格
            createPos += new Vector3(MapMaker.Instance.gridWidth,-MapMaker.Instance.gridHeight)/2; 
        }else if (gridState.itemID <= 4)  //中等物体(两格) itemID
        {
            //将物体右移半格
            createPos += new Vector3(MapMaker.Instance.gridWidth, 0) / 2;
        }
       

        //根据itemID获取对象并实例化
        GameObject item = Instantiate(itemPrefabs[gridState.itemID], createPos, Quaternion.identity);
        currentItem = item; 
    }

    //private void OnMouseOver()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //      //道具
    //    }
    //    else if (Input.GetMouseButtonDown(1))
    //    {
    //      //怪物路径
    //    }
    //}

    //更新格子的状态
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite; //显示格子
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            if (gridState.isMonsterPath)
            {
                spriteRenderer.sprite = monsterPathSprite; //显示怪物路径
            }
            else
            {
                spriteRenderer.enabled = false; //关闭渲染
            }
           
        }

    }  
    
#endif
}
