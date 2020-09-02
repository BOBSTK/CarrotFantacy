using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 游戏场景UI面板
/// </summary>
public class NormalModelPanel : BasePanel
{
    //控制的页面
    private GameObject topPageGo;           //顶部UI
    private GameObject menuPageGo;          //菜单
    private GameObject gameOverPageGo;      //游戏失败页面
    private GameObject gameWinPageGo;       //游戏胜利页面
    private GameObject prizePageGo;         //奖品页面
    private GameObject img_StartGame;       //游戏开始页面
    private GameObject img_FinalWave;       //最后一波提示页面

    //引用
    public TopPage topPage;

    //属性
    public int totalRound;  //总波数

    protected override void Awake()
    {
        base.Awake();
        //获取资源
        //gameController = GameController.Instance;
        transform.SetSiblingIndex(1);
        topPageGo = transform.Find("TopPage").gameObject;
        menuPageGo = transform.Find("MenuPage").gameObject;
        gameOverPageGo = transform.Find("GameOverPage").gameObject;
        gameWinPageGo = transform.Find("GameWinPage").gameObject;
        prizePageGo = transform.Find("PrizePage").gameObject;
        img_StartGame = transform.Find("StartUI").gameObject;
        img_FinalWave = transform.Find("Img_FinalWave").gameObject;

        topPage = topPageGo.GetComponent<TopPage>();
    }

    private void OnEnable()
    {
        InvokeRepeating("PlayAudio", 0.5f, 1.0f); //每隔1s播放一次音效 延时0.5s是因为GameController加载晚于NormalModelPanel
        Invoke("StartGame",3.5f);  //延时3.5s调用，开始游戏
    }

    //播放开场动画倒计时声音
    private void PlayAudio()
    {
        img_StartGame.SetActive(true); //显示游戏开场动画
        GameController.Instance.PlayEffectMusic("NormalModel/CountDown");
    }

    //开始游戏
    private void StartGame()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/GO");
#if Game
        GameController.Instance.StartGame();     //开始游戏
#endif
        img_StartGame.SetActive(false); //隐藏开场动画
        CancelInvoke();
    }

    /// <summary>
    /// 面板处理相关的方法
    /// </summary>
    
    //进入面板的方法
    public override void EnterPanel()
    {
        totalRound = GameController.Instance.currentStage.totalRound;
        topPageGo.SetActive(true); //先获得波次信息，再显示顶部UI
    }

    //更新面板信息
    public override void UpdatePanel()
    {
        topPage.UpdateRoundText(); //更新回合信息
        topPage.UpdateCoinText();  //更新金币信息
    }


    /// <summary>
    /// 页面显示隐藏的方法
    /// </summary>

    //菜单页面
    public void ShowMenuPage()
    {
        GameController.Instance.PlayButtonAudioClip();
        //存储当前游戏状态（是否暂停）
        menuPageGo.GetComponent<MenuPage>().isPreGamePause = GameController.Instance.isPause;
        //暂停游戏
        GameController.Instance.isPause = true;
        menuPageGo.SetActive(true);
    }

    //public void CloseMenuPage()
    //{ 
    //    //GameController.Instance.isPause = false;
    //    menuPageGo.SetActive(false);
    //}

    //奖励页面
    public void ShowPrizePage()
    {
        //Debug.Log("当前游戏状态" + GameController.Instance.isPause);
       
        prizePageGo.GetComponent<PrizePage>().isPreGamePause = GameController.Instance.isPause;
        GameController.Instance.isPause = true;
        prizePageGo.SetActive(true);
    }

    //public void ClosePrizePage()
    //{
    //    //GameController.Instance.isPause = false;
    //    prizePageGo.SetActive(false);
    //}

    //游戏胜利页面
    public void ShowGameWinPage()
    {
        //toDo 优化计算公式 
        //根据GameController的currentStage对象的属性来找到playerManager中对应关卡的Stage对象
        Stage stage = GameManager.Instance.playerManager.normalModelLevelList[GameController.Instance.currentStage.levelID-1+(GameController.Instance.currentStage.bigLevelID-1)*5 ];

        //道具徽章更新
        if (GameController.Instance.IsAllClear()) //道具全部被清除
        {
            stage.isAllClear = true;
        }
        //萝卜徽章更新
        int carrotState = GameController.Instance.GetCarrotState(); //获得萝卜状态
        if (carrotState != 0 && stage.carrotState!=0) //如果萝卜状态!=0且不是第一次通关（stage中萝卜状态不为0）
        {
            if(stage.carrotState > carrotState) //如果当前关卡的萝卜状态更好才更新
            {
                stage.carrotState = carrotState;
            }
        }
        else if(stage.carrotState == 0)   //第一次通过关卡，直接更新stage中的carrotState ,同时要解锁新关卡
        {
          
            stage.carrotState = carrotState;
        }

        //解锁新关卡
        //5的倍数是隐藏关卡 toDo 优化
        if (GameController.Instance.currentStage.levelID % 5 != 0)  //隐藏关卡不会解锁任何新关卡
        {
            int currentLevelIndex = GameController.Instance.currentStage.levelID - 1 + (GameController.Instance.currentStage.bigLevelID - 1) * 5;  //当前关卡在normalModelLevelList中的索引
            Stage nextStage = null;     //下一个未解锁的普通关卡
            Stage nextPlayStage = null; //下一个可玩关卡的Stage对象
            //找到该关卡后的第一个非隐藏关卡
            for (int i = currentLevelIndex + 1 ; i < GameManager.Instance.playerManager.normalModelLevelList.Count; i++)
            {
                if (!GameManager.Instance.playerManager.normalModelLevelList[i].isRewardLevel)
                {
                    nextStage = GameManager.Instance.playerManager.normalModelLevelList[i];
                    break;
                }
            }
          

            if (nextStage != null && !nextStage.isUnLocked) //找到了下一关且未解锁
            {
                //解锁关卡
                GameManager.Instance.playerManager.normalModelLevelList[nextStage.levelID -1 + (nextStage.bigLevelID - 1) * 5].isUnLocked = true;
                GameManager.Instance.playerManager.unLockedNormalModelLevelList[nextStage.bigLevelID - 1]++; //更新PlayerManager中当前大关卡已解锁关卡的数据
                GameManager.Instance.playerManager.normalModelBigLevelList[nextStage.bigLevelID - 1] = true; //解锁大关卡
            }

            //找到该关卡后的第一个可玩关卡
            for (int i = currentLevelIndex + 1; i < GameManager.Instance.playerManager.normalModelLevelList.Count; i++)
            {
                if (GameManager.Instance.playerManager.normalModelLevelList[i].isUnLocked)
                {
                    nextPlayStage = GameManager.Instance.playerManager.normalModelLevelList[i];
                    break;
                }
            }
            if(nextPlayStage!= null)
            {
                GameManager.Instance.currentStage = nextPlayStage;
            }
        }

        //解锁下一个关卡
        //--------------
        //不是最后一关且不是隐藏关卡才能解锁下一关
       
        //如果当前关卡的索引是normalModelLevelList.count -1 时说明是最后一关
        //if (GameController.Instance.currentStage.levelID % 5 != 0 && (GameController.Instance.currentStage.levelID - 1 + (GameController.Instance.currentStage.bigLevelID - 1) * 5)<GameManager.Instance.playerManager.normalModelLevelList.Count) 
        //{
        //    //解锁下一关
        //    GameManager.Instance.playerManager.normalModelLevelList[GameController.Instance.currentStage.levelID  + (GameController.Instance.currentStage.bigLevelID - 1) * 5].isUnLocked = true;
        //    GameManager.Instance.playerManager.unLockedNormalModelLevelList[GameController.Instance.currentStage.bigLevelID - 1]++; //更新PlayerManager中当前大关卡已解锁关卡的数据
        //    //解锁大关卡
        //    //如果当前通过关卡是该大关卡中的最后一关（隐藏关卡除外）且不是最后一个大关卡
        //    //if (GameController.Instance.currentStage.levelID % 5 == 4 && GameController.Instance.currentStage.bigLevelID != 3)
        //    //{
        //    //    //解锁大关卡
        //    //    GameManager.Instance.playerManager.normalModelBigLevelList[GameController.Instance.currentStage.bigLevelID] = true; //解锁大关卡
        //    //    GameManager.Instance.playerManager.unLockedNormalModelLevelList[GameController.Instance.currentStage.bigLevelID] = 1; //将大关卡的解锁关卡数量设为1
        //    //    GameManager.Instance.playerManager.
        //    //}
        //}

        UpdatePlayerManagerData();
        gameWinPageGo.SetActive(true);
        GameController.Instance.isGameOver = true;
        GameManager.Instance.playerManager.adventureModelNum++; 
        GameController.Instance.PlayEffectMusic("NormalModel/Perfect");
    }

    //游戏失败页面
    public void ShowGameOverPage()
    {
        UpdatePlayerManagerData();      //更新数据
        //GameController.Instance.isGameOver = true;
        gameOverPageGo.SetActive(true); //显示失败页面
        GameController.Instance.PlayEffectMusic("NormalModel/Lose");
    }

    /// <summary>
    /// UI显示相关方法
    /// </summary>

    //更新回合显示文本
    public void ShowRoundInfo(Text roundText)
    {
        int roundNum = GameController.Instance.level.currentRound+1; //从1开始
        string roundStr = "";
        if (roundNum < 10)
        {
            roundStr = "0 " + roundNum.ToString();
        }
        else
        {
            roundStr = (roundNum / 10).ToString() + " " + (roundNum % 10).ToString();
        }
        roundText.text = roundStr;
    }

    //最后一波提示页面
    public void ShowFinalWaveUI()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/Finalwave");
        img_FinalWave.SetActive(true);
        Invoke("CloseFinalWaveUI", 1.0f);
    }

    public void CloseFinalWaveUI()
    {
        img_FinalWave.SetActive(false);
        GameController.Instance.level.HandleLastRound(); //加载最后一波怪物
    }

    /// <summary>
    /// 关卡处理相关的方法
    /// </summary>
     
    //更新玩家数据
    private void UpdatePlayerManagerData()
    {
        //更新基础数据
        GameManager.Instance.playerManager.coin += GameController.Instance.coin;
        GameManager.Instance.playerManager.monsterKilledNum += GameController.Instance.monsterKilledTotalNum;
        GameManager.Instance.playerManager.itemClearedNum += GameController.Instance.itemClearedNum;
    }

    //重玩
    public void Replay()
    {
        GameController.Instance.PlayButtonAudioClip();
        GameController.Instance.isGameOver = true;  //结束当前游戏 （用于清理当前游戏场景的数据）
        UpdatePlayerManagerData(); //更新数据
        //重新加载游戏场景
        uiFacade.ChangeSceneState(new NormalModelSceneState(uiFacade));
       // GameController.Instance.isGameOver = false;
        Invoke("RestGame", 2.0f);  //遮罩时间
    }
    
    //重置游戏
    private void RestGame()
    {
        GameController.Instance.isGameOver = false;
        SceneManager.LoadScene(3); //加载游戏场景
        ResetUI();
        gameObject.SetActive(true); //这里是为了调用OnEnable方法
    }

    //重置页面UI显示状态
    public void ResetUI()
    {
        gameOverPageGo.SetActive(false);
        gameWinPageGo.SetActive(false);
        menuPageGo.SetActive(false);
        gameObject.SetActive(false); //自身也要隐藏
    }

    //选择其他关卡
    public void ChooseOtherLevel()
    {
        GameController.Instance.PlayButtonAudioClip();
        GameController.Instance.isGameOver = true;
        UpdatePlayerManagerData();
        Invoke("ToOtherScene", 2.0f);
        uiFacade.ChangeSceneState(new GameNormalOptionSceneState(uiFacade));
    }

    //加载冒险模式关卡选择场景
    public void ToOtherScene()
    {
        GameController.Instance.isGameOver = false;
        ResetUI();
        SceneManager.LoadScene(2); //返回冒险模式关卡选择场景
    }

  
}
