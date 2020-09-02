using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNormalBigLevelPanel : BasePanel
{
    public Transform bigLevelContentTrans;  //滚动视图的Content
    public int bigLevelPageCount;           //大关卡总数        
    private SlideScrollView slideScrollView;
    private PlayerManager playerManager;
    private Transform[] bigLevelPage;       //每一个大关卡的信息(Content中的对象)
   // private bool hasRigisterEvent;          //是否已经注册 

    protected override void Awake()
    {
        base.Awake();
        playerManager = uiFacade.playerManager;
        bigLevelPage = new Transform[bigLevelPageCount];
        slideScrollView = transform.Find("Scroll View").GetComponent<SlideScrollView>();

        //显示大关卡信息
        for(int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            //ToDo Remove HardCode
            ShowBigLevelState(playerManager.normalModelBigLevelList[i], playerManager.unLockedNormalModelLevelList[i],5,bigLevelPage[i],i+1);
            RigisterEvent(bigLevelPage[i], i + 1); //在Awake方法注册一次
        }
       // hasRigisterEvent = true;
    }

    //每次激活时要更新数据
    private void OnEnable()
    {
        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            //ToDo Remove HardCode
            ShowBigLevelState(playerManager.normalModelBigLevelList[i], playerManager.unLockedNormalModelLevelList[i],5, bigLevelPage[i], i + 1);
        }
    }

    //进入退出面板
    public override void EnterPanel()
    {
        slideScrollView.Init();
        gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        gameObject.SetActive(false);
    }



    //更新大关卡信息
    public void ShowBigLevelState(bool isUnLocked, int unLockedLevelNum, int totalNum, Transform bigLevelButtonTrans, int bigLevelID)
    {
        if (isUnLocked) //解锁状态
        {
            bigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(false); //隐藏锁 
            bigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(true);  //显示书签
            bigLevelButtonTrans.Find("Img_Page").Find("Text_Page").GetComponent<Text>().text
                = unLockedLevelNum.ToString() + "/" + totalNum.ToString();    //设置书签内容
            
            Button bigLevelButton = bigLevelButtonTrans.GetComponent<Button>(); //获取大关卡Button组件
            bigLevelButton.interactable = true;                                //将Button设为可交互状态
            
            //if (!hasRigisterEvent)   //确保只注册一次
            //{
            //    //注册大关卡按钮事件
            //    bigLevelButton.onClick.AddListener(() =>
            //    {
            //        Debug.Log("bigLevelID：" + bigLevelID + "is clicked");
            //        uiFacade.PlayButtonAudioClip();  //点击到大关卡页面的时候播放音效
            //        //离开大关卡页面
            //        uiFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].ExitPanel();
            //        //进入小关卡页面
            //        //获取GameNormalLevelPanel
            //        GameNormalLevelPanel gameNormalLevelPanel = (GameNormalLevelPanel)uiFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel];
            //        gameNormalLevelPanel.ToThisPanel(bigLevelID);  //设置小关卡面板的大关卡值
            //        GameNormalOptionPanel gameNormalOptionPanel = (GameNormalOptionPanel)uiFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel];
            //        gameNormalOptionPanel.isInBigLevelPanel = false; //表示已经不在大关卡页面了
            //    }
            //        );
            //}
           

        }
        else            //未解锁
        {
            bigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(true);  //显示锁 
            bigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(false);  //隐藏书签
            bigLevelButtonTrans.GetComponent<Button>().interactable = false;   //将Button设为不可交互状态
        }
    }

    //注册按钮事件
    public void RigisterEvent(Transform bigLevelButtonTrans, int bigLevelID)
    {
        Button bigLevelButton = bigLevelButtonTrans.GetComponent<Button>(); //获取大关卡Button组件
        //if (!hasRigisterEvent)   //确保只注册一次
        //{
            //注册大关卡按钮事件
            bigLevelButton.onClick.AddListener(() =>
            {
                Debug.Log("bigLevelID：" + bigLevelID + "is clicked");
                uiFacade.PlayButtonAudioClip();  //点击到大关卡页面的时候播放音效
                //离开大关卡页面
                uiFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].ExitPanel();
                //进入小关卡页面
                //获取GameNormalLevelPanel
                GameNormalLevelPanel gameNormalLevelPanel = (GameNormalLevelPanel)uiFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel];
                gameNormalLevelPanel.ToThisPanel(bigLevelID);  //设置小关卡面板的大关卡值
                GameNormalOptionPanel gameNormalOptionPanel = (GameNormalOptionPanel)uiFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel];
                gameNormalOptionPanel.isInBigLevelPanel = false; //表示已经不在大关卡页面了
            }
                );
        //}
    }

    //翻页按钮方法
    public void ToNextPage()
    {
        uiFacade.PlayButtonAudioClip();
        uiFacade.PlayPagingAudioClip();
        slideScrollView.ToNextPage();
    }

    public void ToLastPage()
    {
        uiFacade.PlayButtonAudioClip();
        uiFacade.PlayPagingAudioClip();
        slideScrollView.ToLastPage();
    }
}
