using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : BasePanel
{
    //只在当前场景中使用的资源，不使用对象池
    private Animator carrotAnimator;
    private Transform monsterTrans;   //怪物图片的Transform组件，用于DotWeen
    private Transform cloudTrans;
    private Tween[] mainPanelTween;  //0-上移 1-下移
    private Tween ExitTween;        //离开页面运行的动画

    
    protected override void Awake()
    {
        base.Awake();
        //设置渲染层级
        transform.SetSiblingIndex(8);
        
        carrotAnimator = transform.Find("Emp_Carrot").GetComponent<Animator>();
        //Debug.Log(carrotAnimator);
        carrotAnimator.Play("CarrotGrow");
        monsterTrans = transform.Find("Img_Monster");
        cloudTrans = transform.Find("Img_Cloud");

        mainPanelTween = new Tween[2];
        //左右移动动画
        mainPanelTween[0] = transform.DOLocalMoveY(1440,0.3f);//上移
        mainPanelTween[0].SetAutoKill(false);//保存动画
        mainPanelTween[0].Pause();//暂停，开始时不动
        mainPanelTween[1] = transform.DOLocalMoveY(-1440, 0.3f);//下移
        mainPanelTween[1].SetAutoKill(false);//保存动画
        mainPanelTween[1].Pause();//暂停，开始时不动

        PlayUITween();
    }

    //进入面板的方法(从其他页面返回主页面)
    public override void EnterPanel()
    {
        transform.SetSiblingIndex(8); //在进入页面时设置渲染等级，防止页面切换时发生错误遮盖
        carrotAnimator.Play("CarrotGrow");
        if(ExitTween != null)  //如果之前播过该动画，倒播
        {
            ExitTween.PlayBackwards();
        }
        cloudTrans.gameObject.SetActive(true); //激活云朵
    }

    //退出面板的方法
    public override void ExitPanel()
    {
        ExitTween.PlayForward();
        cloudTrans.gameObject.SetActive(false); //隐藏云朵
    }

    //UI动画播放
    private void PlayUITween()
    {
        monsterTrans.DOLocalMoveY(520, 1.5f).SetLoops(-1,LoopType.Yoyo); //让动画无限循环播放
        cloudTrans.DOLocalMoveX(1300,12.0f).SetLoops(-1, LoopType.Restart);
    }


    public void MoveUp()
    {
        uiFacade.PlayButtonAudioClip();
        ExitTween = mainPanelTween[0]; //将主界面上移
        uiFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }

    public void MoveDown()
    {
        uiFacade.PlayButtonAudioClip();
        ExitTween = mainPanelTween[1]; //将主界面下移
        uiFacade.currentScenePanelDict[StringManager.SetPanel].EnterPanel();
    }

    //场景状态切换的方法
    public void ToNormaModelScene()
    {
        uiFacade.PlayButtonAudioClip();
        uiFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel(); //切换场景时的过场动画
        uiFacade.ChangeSceneState(new GameNormalOptionSceneState(uiFacade)); //切换到冒险模式场景
    }

    public void ToBossModelScene()
    {
        uiFacade.PlayButtonAudioClip();
        uiFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel(); //切换场景时的过场动画
        uiFacade.ChangeSceneState(new GameBossOptionSceneState(uiFacade)); //切换到Boss模式场景
    }

    public void ToMonsterNest()
    {
        uiFacade.PlayButtonAudioClip();
        uiFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel(); //切换场景时的过场动画
        uiFacade.ChangeSceneState(new MonsterNestSceneState(uiFacade)); //切换到怪物窝场景
    }

    public void ExitGame()
    {
        uiFacade.PlayButtonAudioClip();
        GameManager.Instance.playerManager.SaveData(); //保存数据
        Application.Quit(); //退出游戏
    }

}
