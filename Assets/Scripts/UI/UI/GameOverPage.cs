using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏失败页面
/// 在游戏失败时出现
/// 显示关卡信息，可以选择再试一次或者选择关卡
/// </summary>
public class GameOverPage : MonoBehaviour
{
    private Text text_RoundCount;
    private Text text_TotalCount;
    private Text text_Level;
    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        text_RoundCount = transform.Find("Text_Round").GetComponent<Text>();
        text_TotalCount = transform.Find("Text_Total").GetComponent<Text>();
        text_Level = transform.Find("Text_Level").GetComponent<Text>();
    }

    private void OnEnable()
    {
        //更新页面信息
        text_TotalCount.text = normalModelPanel.totalRound.ToString();    //总波数
        text_Level.text = GameController.Instance.currentStage.levelID.ToString(); //关卡
        //normalModelPanel.ShowRoundInfo(text_RoundCount);  //当前波数
        ShowCurrentRound();

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

    private void ShowCurrentRound()
    {
        int roundNum = GameController.Instance.level.currentRound + 1; //从1开始
        string roundStr = "";
        if (roundNum < 10)
        {
            roundStr = "0 " + roundNum.ToString();
        }
        else
        {
            roundStr = (roundNum / 10).ToString() + "   " + (roundNum % 10).ToString();
        }
        text_RoundCount.text = roundStr;
    }
}
