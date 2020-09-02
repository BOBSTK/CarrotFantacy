using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : BasePanel
{
    //组件的引用
    private GameObject optionPage;    
    private GameObject statisticsPage;
    private GameObject producerPage;
    private GameObject resetPanel;   //重置游戏的提醒页面

    private Tween setPanelTween;     

    private bool playBGAudio = true;      //背景音乐
    private bool playEffectAudio = true;  //音效
    public Sprite[] btnSprite;           //按钮切换的Sprite 0-音效开 1-音效关 2-背景音乐开 3-背景音乐关
    private Image Img_Btn_EffectAudio;    //音效开关图片
    private Image Img_Btn_BGAudio;        //背景音乐开关图片
    public Text[] statisticTexts;         //数据统计页面的文本



    protected override void Awake()
    {
        base.Awake();
        //设置面板进场动画
        setPanelTween = transform.DOLocalMoveY(0, 0.3f);
        setPanelTween.SetAutoKill(false);
        setPanelTween.Pause();
        //初始化成员变量     
        optionPage = transform.Find("OptionPage").gameObject;
        producerPage = transform.Find("ProducerPage").gameObject;
        statisticsPage = transform.Find("StatisticsPage").gameObject;        

        resetPanel = transform.Find("ResetPanel").gameObject;
        Img_Btn_EffectAudio = optionPage.transform.Find("Btn_EffectAudio").GetComponent<Image>();
        Img_Btn_BGAudio = optionPage.transform.Find("Btn_BGAudio").GetComponent<Image>();
    }

    public override void InitPanel()
    {
        transform.localPosition = new Vector3(0, 1440, 0);  //从上边进场
        transform.SetSiblingIndex(2); //渲染层级

    }

    //显示页面的方法
    public void ShowOptionPage()
    {
        if (!optionPage.activeSelf) //如果设置页面处于未激活状态，播放音效
        {
            uiFacade.PlayButtonAudioClip();
            optionPage.SetActive(true);
        }
        statisticsPage.SetActive(false);
        producerPage.SetActive(false);
    }
    public void ShowStatisticsPage()
    {
        uiFacade.PlayButtonAudioClip();
        UpdateStatistics(); //更新玩家数据
        optionPage.SetActive(false);
        statisticsPage.SetActive(true);
        producerPage.SetActive(false);
    }
    public void ShowProducerPage()
    {
        uiFacade.PlayButtonAudioClip();
        optionPage.SetActive(false);
        statisticsPage.SetActive(false);
        producerPage.SetActive(true);
    }

    //进入页面的方法
    public override void EnterPanel()
    {
        ShowOptionPage();
        MoveToCenter();
    }

    //退出页面的方法
    public override void ExitPanel()
    {
        uiFacade.PlayButtonAudioClip();
        setPanelTween.PlayBackwards();
        uiFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel(); //重新加载MainPanel
        //InitPanel();  //在切换场景状态时会自动调用这个方法
    }

    public void MoveToCenter()
    {
        setPanelTween.PlayForward();
    }

    //背景音乐处理
    public void CloseOrOpenBGAudio()
    {
        uiFacade.PlayButtonAudioClip();
        playBGAudio = !playBGAudio;
        uiFacade.CloseOrOpenBGAudio();//背景音乐切换

        if (playBGAudio)
        {
            Img_Btn_BGAudio.sprite = btnSprite[2];
        }
        else
        {
            Img_Btn_BGAudio.sprite = btnSprite[3];
        }
    }

    //音效处理
    public void CloseOrOpenEffectAudio()
    {
        uiFacade.PlayButtonAudioClip();
        playEffectAudio = !playEffectAudio;
        uiFacade.CloseOrOpenEffectAudio();//音效切换

        if (playEffectAudio)
        {
            Img_Btn_EffectAudio.sprite = btnSprite[0];
        }
        else
        {
            Img_Btn_EffectAudio.sprite = btnSprite[1];
        }
    }

    //更新玩家数据
    public void UpdateStatistics()
    {
        //Todo 显示玩家的数据
        PlayerManager playerManager = uiFacade.playerManager; //需要优化
        statisticTexts[0].text = playerManager.adventureModelNum.ToString();
        statisticTexts[1].text = playerManager.burriedLevelNum.ToString();
        statisticTexts[2].text = playerManager.bossModelNum.ToString();
        statisticTexts[3].text = playerManager.coin.ToString();
        statisticTexts[4].text = playerManager.monsterKilledNum.ToString();
        statisticTexts[5].text = playerManager.bossKilledNum.ToString();
        statisticTexts[6].text = playerManager.itemClearedNum.ToString();
    }

    //重置游戏
    public void ResetGame()
    {
        uiFacade.PlayButtonAudioClip();
        //重置玩家数据
        GameManager.Instance.isResetData = true;
        GameManager.Instance.playerManager.ReadData();
        ShowStatisticsPage();
        CloseResetPanel();
    }

    public void ShowResetPanel()
    {
        resetPanel.SetActive(true);
    }

    public void CloseResetPanel()
    {
        uiFacade.PlayButtonAudioClip();
        resetPanel.SetActive(false);
    }


}
