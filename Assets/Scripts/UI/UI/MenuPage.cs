using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 菜单页面
/// 玩家点击时显示，此时游戏应该暂停
/// </summary>
public class MenuPage : MonoBehaviour
{
    
    //引用
    private NormalModelPanel normalModelPanel;
    //属性 
    public bool isPreGamePause;  //关闭菜单时游戏应该恢复的状态

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
    }

    //继续游戏
    public void GoOn()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        //恢复游戏状态
        GameController.Instance.isPause = isPreGamePause;
        transform.gameObject.SetActive(false);    //隐藏菜单
    }

    //重新开始
    public void RePlay()
    {
        normalModelPanel.Replay();
    }

    //选择关卡
    public void ChooseOtherLevel()
    {
        normalModelPanel.ChooseOtherLevel();
    }
}
