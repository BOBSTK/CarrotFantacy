using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏胜利页面脚本
/// 显示关卡信息 通关波次数 萝卜状态 关卡信息
/// </summary>
public class GameWinPage : MonoBehaviour
{
    private Text text_RoundCount;
    private Text text_TotalCount;
    private Text text_Level;
    private Image img_Carrot; //萝卜状态图片
    private NormalModelPanel normalModelPanel;
    public Sprite[] carrotSprites; //1.金 2.银 3.铜

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        text_RoundCount = transform.Find("Text_Round").GetComponent<Text>();
        text_TotalCount = transform.Find("Text_Total").GetComponent<Text>();
        text_Level = transform.Find("Text_Level").GetComponent<Text>();
        img_Carrot = transform.Find("Img_Carrot").GetComponent<Image>();
        carrotSprites = new Sprite[4];
        for (int i = 1; i <= 3; i++)
        {
            carrotSprites[i] = GameController.Instance.GetSprite("GameOption/Normal/Level/Carrot_" + i.ToString());
        }
    }

    private void OnEnable()
    {
        //更新页面信息
        text_TotalCount.text = normalModelPanel.totalRound.ToString();    //总波数
        text_Level.text = GameController.Instance.currentStage.levelID.ToString(); //关卡
        //normalModelPanel.ShowRoundInfo(text_RoundCount);  //当前波数
        ShowCurrentRound();

        //显示萝卜徽章
        img_Carrot.sprite = carrotSprites[GameController.Instance.GetCarrotState()];
    }

    //继续游戏（进入下一关）
    public void GoOn()
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
