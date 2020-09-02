
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 冒险模式小关卡选择面板
/// </summary>
public class GameNormalLevelPanel : BasePanel
{
    private string filePath;         //图片资源加载的根路径
    private string spritePath;       //获取关卡资源路径 (根据每次外部调用ToThisPanel时赋值)
    public int currentBigLevelID;    //所属大关卡ID
    public int currentLevelID;       //当前关卡ID

    private Transform levelContentTrans;   //小关卡滚动视窗Content对象
    
    private GameObject img_LockBtn;        //未解锁关卡遮挡
    private Transform emp_TowerTrans;      //建塔列表
    private Image img_BGLeft, img_BGRight; //大关卡主题图片(左下角，右下角)
    private Image img_Carrot; //萝卜状态图片
    private Image img_Clear;  //道具清理情况
    private Text text_TotalWaves;  //敌人总数(波)

    private PlayerManager playerManager;
    private SlideScrollView slideScrollView;

    private List<GameObject> levelContentImage;  //地图卡片UI列表
    private List<GameObject> towerContentImage;  //建塔列表



    protected override void Awake()
    {
        base.Awake();
        filePath = @"GameOption/Normal/Level/";
        playerManager = uiFacade.playerManager;
        levelContentImage = new List<GameObject>();
        towerContentImage = new List<GameObject>();
        levelContentTrans = transform.Find("Scroll View").Find("Viewport").Find("Content");
        img_LockBtn = transform.Find("Img_Lock").gameObject;
        emp_TowerTrans = transform.Find("EMP_Tower");
        img_BGLeft = transform.Find("Img_BGLeft").GetComponent<Image>();
        img_BGRight = transform.Find("Img_BGRight").GetComponent<Image>();
        text_TotalWaves = transform.Find("Img_Total").Find("Text").GetComponent<Text>();
        slideScrollView = transform.Find("Scroll View").GetComponent<SlideScrollView>();
        currentBigLevelID = 1;
        currentLevelID = 1;
    }

    //更新地图UI(动态UI 地图信息) (进入面板时只调用一次)
    public void UpdateMapUI(string spritePath)
    {
        img_BGLeft.sprite = uiFacade.GetSprite(spritePath + "BG_Left");
        img_BGRight.sprite = uiFacade.GetSprite(spritePath + "BG_Right");
        for(int i = 0; i < 5; i++)
        {
            levelContentImage.Add(InitUI("Img_Level", levelContentTrans));  //实例化一个地图卡片UI,并放入地图卡片UI列表
            //更换关卡图片
            levelContentImage[i].GetComponent<Image>().sprite = uiFacade.GetSprite(spritePath + "Level_" + (i+1).ToString());
            //获取当前关卡的Stage对象
            Stage stage = playerManager.normalModelLevelList[(currentBigLevelID - 1) * 5 + i];
            
            levelContentImage[i].transform.Find("Img_Carrot").gameObject.SetActive(false);
            levelContentImage[i].transform.Find("Img_Clear").gameObject.SetActive(false);

            if (stage.isUnLocked)
            {
                //已解锁
                if (stage.isAllClear)
                {
                    //所有道具都被清理了
                    levelContentImage[i].transform.Find("Img_Clear").gameObject.SetActive(true);
                }
                if(stage.carrotState != 0)
                {
                    //已经通关
                    Image carrotImage = levelContentImage[i].transform.Find("Img_Carrot").GetComponent<Image>();
                    carrotImage.gameObject.SetActive(true);
                    carrotImage.sprite = uiFacade.GetSprite(filePath + "Carrot_" + stage.carrotState.ToString());
                }
                levelContentImage[i].transform.Find("Img_Lock").gameObject.SetActive(false); //隐藏锁
                levelContentImage[i].transform.Find("Img_Curtain").gameObject.SetActive(false); //隐藏幕布
            }
            else
            {
                //未解锁
                if (stage.isRewardLevel)
                {
                    //奖励关卡(有幕布)
                    levelContentImage[i].transform.Find("Img_Lock").gameObject.SetActive(false); //隐藏锁
                    levelContentImage[i].transform.Find("Img_Curtain").gameObject.SetActive(true); //显示幕布
                    //修改隐藏关卡解锁怪物图片                   
                    Image monsterImage = levelContentImage[i].transform.Find("Img_Curtain").Find("Img_Monster").GetComponent<Image>();
                    monsterImage.sprite = uiFacade.GetSprite("MonsterNest/Monster/Baby/"+currentBigLevelID.ToString());
                    //调整图片大小
                    monsterImage.SetNativeSize();
                    monsterImage.transform.localScale = new Vector3(2, 2, 1);
                }
                else
                {
                    //不是奖励关卡
                    levelContentImage[i].transform.Find("Img_Lock").gameObject.SetActive(true); //显示锁
                    levelContentImage[i].transform.Find("Img_Curtain").gameObject.SetActive(false); //隐藏幕布
                }
            }
        }
        //更新滚动视图Content大小的方法
        //ToDo Remove HardCode
        slideScrollView.SetContentLength(5);
    }

    //销毁地图UI
    private void DestroyMapUI()
    {
        if (levelContentImage.Count > 0) 
        {
            for(int i = 0; i < 5; i++)
            {
                //将地图UI放入对象池
                uiFacade.PushGameObjectToFactory(FactoryType.UIFactory, "Img_Level", levelContentImage[i]);
            }
            slideScrollView.InitScrollLength(); //重置Content大小
            levelContentImage.Clear();  //清空字典
        }
    }

    //更新静态UI (关卡信息 敌人总数 建塔列表 开始挡板)
    public void UpdateLevelUI(string spritePath)
    {   
        //销毁之前的静态UI
        if(towerContentImage.Count != 0)
        {
            for(int i = 0; i < towerContentImage.Count; i++)
            {
                towerContentImage[i].GetComponent<Image>().sprite = null;
                uiFacade.PushGameObjectToFactory(FactoryType.UIFactory,"Img_Tower", towerContentImage[i]); 
            }
            towerContentImage.Clear(); //清空字典
        }
        //ToDo Remove Hard Code
        Stage stage = playerManager.normalModelLevelList[(currentBigLevelID-1)*5 + currentLevelID -1];
        if (stage.isUnLocked)
        {
            //解锁
            img_LockBtn.SetActive(false); //隐藏挡板

        }
        else
        {
            //未解锁
            img_LockBtn.SetActive(true); //显示挡板
        }
        text_TotalWaves.text = stage.totalRound.ToString(); //显示敌人总数
        //建塔列表
        for(int i = 0; i < stage.towerIDList.Length; i++)
        {
            towerContentImage.Add(InitUI("Img_Tower", emp_TowerTrans));
            towerContentImage[i].GetComponent<Image>().sprite = uiFacade.GetSprite(filePath + "Tower/Tower_" + stage.towerIDList[i].ToString());
        }

    }

    /// <summary>
    /// 处理面板的方法
    /// </summary>
    /// <param name="currentBigLevelID"></param>
   
    //外部调用的进入当前页面的方法
    public void ToThisPanel(int currentBigLevelID)
    {
        this.currentBigLevelID = currentBigLevelID;
        currentLevelID = 1; //进入面板时从第一关开始
        EnterPanel();
    }

    //初始化
    public override void InitPanel()
    {
        gameObject.SetActive(false);
    }

    //进入面板(由ToThisPanel调用)
    public override void EnterPanel()
    {
        gameObject.SetActive(true);
        spritePath = filePath + currentBigLevelID.ToString() + "/"; //获取关卡资源路径
        //更新动态UI
        DestroyMapUI();
        UpdateMapUI(spritePath);
        //更新静态UI
        UpdateLevelUI(spritePath);
        slideScrollView.Init();
    }
   

    //更新面板 （更新静态UI）
    public override void UpdatePanel()
    {
        UpdateLevelUI(spritePath);
    }


    //退出面板
    public override void ExitPanel()
    {
        gameObject.SetActive(false);
    }

    //进入游戏场景
    public void ToGamePanel()
    {
        uiFacade.PlayButtonAudioClip();
        GameManager.Instance.currentStage = playerManager.normalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID - 1];
        uiFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel(); //显示过场
        uiFacade.ChangeSceneState(new NormalModelSceneState(uiFacade));
    }

    /// <summary>
    /// 帮助更新UI的方法
    /// </summary>

    //实例化UI并设置父对象  
    public GameObject InitUI(string uiName, Transform parentTrans)
    {
        GameObject item = uiFacade.GetGameObject(FactoryType.UIFactory,uiName);
        item.transform.SetParent(parentTrans);
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;
        return item;
    }

    //下一关
    public void ToNextLevel()
    {
        ++currentLevelID;
        UpdatePanel();
    }

    //上一关
    public void ToLastLevel()
    {
        --currentLevelID;
        UpdatePanel();
    }

    //资源加载 （提前加载相应的资源,存储在SpriteFactory的资源字典中）
    private void LoadResource()
    {
        //Sprite资源
        //----------
        uiFacade.GetSprite(filePath+ "AllClear");  
        uiFacade.GetSprite(filePath + "Carrot_1");  //金
        uiFacade.GetSprite(filePath + "Carrot_2");  //银
        uiFacade.GetSprite(filePath + "Carrot_3");  //普通
        //ToDo HardCode
        for(int i = 1; i < 4; i++)
        {
            string spritePath = filePath + i.ToString() + "/";
            uiFacade.GetSprite(spritePath + "BG_Left");
            uiFacade.GetSprite(spritePath + "BG_Right");
            for(int j = 1; j < 6; j++)
            {
                uiFacade.GetSprite(spritePath + "Level_" + j.ToString());  //地图卡片资源
            }
            for(int j=1; j < 13; j++)
            {
                uiFacade.GetSprite(filePath + "Tower/Tower_" + j.ToString()); //塔资源
            }
        }
    }

   
}
