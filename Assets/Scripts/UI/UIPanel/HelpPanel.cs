using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpPanel : BasePanel
{
    //组件的引用
    private GameObject helpPage;
    private GameObject monsterPage;
    private GameObject towerPage;

    private SlideScrollView slideScrollView; //TowerPage翻页脚本(单滑)
    private SlideCoverSrcollView slideCoverSrcollView; //HelpPage翻页脚本(多滑)
    private Tween helpPanelTween;

    protected override void Awake()
    {
        base.Awake();
        //初始化成员变量
        helpPage = transform.Find("HelpPage").gameObject;
        monsterPage = transform.Find("MonsterPage").gameObject;
        towerPage = transform.Find("TowerPage").gameObject;

        slideCoverSrcollView = helpPage.transform.Find("Scroll View").GetComponent<SlideCoverSrcollView>();
        slideScrollView = towerPage.transform.Find("Scroll View").GetComponent<SlideScrollView>();
        
        helpPanelTween = transform.DOLocalMoveY(0, 0.3f);
        helpPanelTween.SetAutoKill(false);
        helpPanelTween.Pause();
    }

    //显示页面的方法
    public void ShowHelpPage()
    {
        if (!helpPage.activeSelf) //如果帮助页面处于未激活状态，播放音效
        {
            uiFacade.PlayButtonAudioClip();
            helpPage.SetActive(true);
        }
        
        monsterPage.SetActive(false);
        towerPage.SetActive(false);
    }
    public void ShowMonsterPage()
    {
        uiFacade.PlayButtonAudioClip();
        helpPage.SetActive(false);
        monsterPage.SetActive(true);
        towerPage.SetActive(false);
    }
    public void ShowTowerPage()
    {
        uiFacade.PlayButtonAudioClip();
        helpPage.SetActive(false);
        monsterPage.SetActive(false);
        towerPage.SetActive(true);
    }

    //处理面板的方法
    public override void InitPanel()
    {
        
        transform.SetSiblingIndex(5); //渲染层级
        slideScrollView.Init();
        slideCoverSrcollView.Init();
        ShowHelpPage(); //默认显示HelpPage

        //从其他面板进入帮助面板并返回主界面的方法
        if (transform.localPosition == Vector3.zero)
        { 
            gameObject.SetActive(false);
            helpPanelTween.PlayBackwards();
        }
        transform.localPosition = new Vector3(0, -1440, 0);  //从下边进场
    }

    //进入页面的方法
    public override void EnterPanel()
    {
        gameObject.SetActive(true);
        slideScrollView.Init();
        slideCoverSrcollView.Init();
        MoveToCenter();
    }

    public override void ExitPanel()
    {
        uiFacade.PlayButtonAudioClip();
        if (uiFacade.currentSceneState.GetType() == typeof(GameNormalOptionSceneState))
        {
            //其他场景
            uiFacade.ChangeSceneState(new MainSceneState(uiFacade));
            SceneManager.LoadScene(1); //切换到主界面场景
        }
        else
        {
            //主场景
            helpPanelTween.PlayBackwards();
            uiFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel(); //重新加载MainPanel
        }
        
    }
    public void MoveToCenter()
    {
        helpPanelTween.PlayForward();
    }
}
