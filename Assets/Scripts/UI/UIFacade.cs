using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI中介,上层与管理者交互，下层与UI面板交互
/// </summary>
public class UIFacade 
{
    //管理者
    private UIManager uiManager;
    private GameManager gameManager;
    private AudioSourceManager audioSourceManager;
    public PlayerManager playerManager;

    //UI面板
    public Dictionary<string, IBasePanel> currentScenePanelDict = new Dictionary<string, IBasePanel>();

    //场景状态
    public IBaseSecenState currentSceneState;  //当前场景状态
    private IBaseSecenState lastSceneState;    //前一个场景状态

    //其他成员变量
    private GameObject mask;   //遮罩，用于场景切换
    private Image maskImage;
    public Transform canvasTransform; //获取Canvas(只有一个)
    

    public UIFacade(UIManager uiManager)
    {
        this.uiManager = uiManager;
        gameManager = GameManager.Instance;
        audioSourceManager = gameManager.audioSourceManager;
        playerManager = gameManager.playerManager;
        InitMask();
    }

    //初始化遮罩
    public void InitMask()
    {
        canvasTransform = GameObject.Find("Canvas").transform;  //只有一个Canvas,可以直接通过Find查找
        mask = InitUI("Img_Mask");
        maskImage = mask.GetComponent<Image>();
    } 

    //显示遮罩
    public void ShowMask()
    {
        mask.transform.SetSiblingIndex(10);  //设置渲染顺序
        //遮罩的透明度：0 -> 1
        Tween t = DOTween.To(() =>
                             maskImage.color,
                             toColor
                             => maskImage.color = toColor,
                             new Color(0, 0, 0, 1), 2.0f);
        t.OnComplete(ExitSceneComplete); //回调方法，在动画执行完毕后立即执行ExitSceneComplete
    }

    //离开场景
    private void ExitSceneComplete()
    {
        lastSceneState.ExitScene();
        currentSceneState.EnterScene();
        HideMask();
    }

    //隐藏遮罩
    public void HideMask()
    {
        mask.transform.SetSiblingIndex(10); //新产生的UI会在渲染层级的最下面，需要重新设定渲染层级
        Tween t = DOTween.To(() =>
                            maskImage.color,
                            toColor
                            => maskImage.color = toColor,
                            new Color(0, 0, 0, 0), 2.0f);
    }
    //改变当前场景的状态
    public void ChangeSceneState(IBaseSecenState baseSecenState)
    {
        //离开当前场景 加载新的场景
        lastSceneState = currentSceneState;
        ShowMask();     //显示遮罩，过场  
        currentSceneState = baseSecenState;
    }

    //实例化当前场景所有面板（在UIManager中），并存入字典
    public void InitPanel()
    {
        foreach(var item in uiManager.currentScenePanelDict)
        {
            item.Value.transform.SetParent(canvasTransform);
            item.Value.transform.localPosition = Vector3.zero;
            item.Value.transform.localScale = Vector3.one;
            IBasePanel basePanel = item.Value.GetComponent<IBasePanel>();
            //Debug.Log(item.Key);
            if(basePanel == null)
            {
                Debug.Log("获取面板上IBasePanel脚本失败: "+item.Key);
            }
            basePanel.InitPanel(); //调用Panel初始化方法
            currentScenePanelDict.Add(item.Key, basePanel);
        }
    }

    //实例化UI
    public GameObject InitUI(string uiName)
    {
        GameObject item;
        item = GetGameObject(FactoryType.UIFactory, uiName);
        item.transform.SetParent(canvasTransform);    //将UI组件放到Canvas下
        item.transform.localPosition = Vector3.zero;  //坐标修正
        //Debug.Log(item.name + "的坐标为" + item.transform.localPosition);
        item.transform.localScale = Vector3.one;      //比例修正
        return item;
    }

    //把UIPanel添加到UIManager字典
    public void AddPanelToDict(string uiPanelName)
    {   
        uiManager.currentScenePanelDict.Add(uiPanelName, GetGameObject(FactoryType.UIPanelFactory, uiPanelName));
    }

    //清空UIPanel字典
    public void ClearDict()
    {
        currentScenePanelDict.Clear();
        uiManager.ClearDict();
    }

    //获取资源
    public Sprite GetSprite(string resourceName)
    {
        return gameManager.GetSprite(resourceName);
    }

    public AudioClip GetAudioClip (string resourceName)
    {
        return gameManager.GetAudioClip(resourceName);
    }

    public RuntimeAnimatorController GetController(string resourceName)
    {
        return gameManager.GetController(resourceName);
    }

    //获取游戏物体
    public GameObject GetGameObject(FactoryType type, string itemName)
    {
        return gameManager.GetGameObject(type, itemName);
    }

    //将物体放回对象池
    public void PushGameObjectToFactory(FactoryType type, string itemName, GameObject item)
    {
        gameManager.PushGameObjectToFactory(type, itemName, item);
    }


    /// <summary>
    /// 音乐播放的相关方法
    /// </summary>
    
    //音乐开关
    public void CloseOrOpenBGAudio()
    {
        //Debug.Log("UIFacade CloseOrOpenBGAudio");
        audioSourceManager.CloseOrOpenBGAudio();
    }
    public void CloseOrOpenEffectAudio()
    {
       // Debug.Log("UIFacade CloseOrOpenEffectAudio");
        audioSourceManager.CloseOrOpenEffectAudio();
    }
    //播放按钮音效
    public void PlayButtonAudioClip()
    {
        audioSourceManager.PlayButtonAudioClip();
    }
    //播放翻页音效
    public void PlayPagingAudioClip()
    {
        audioSourceManager.PlayPagingAudioClip();
    }
    
  
}
