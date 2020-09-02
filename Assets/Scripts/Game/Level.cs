using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡对象。负责Round责任链的创建和执行
/// </summary>
public class Level 
{
    public int totalRound;  //总波次数
    public Round[] roundList; //当前关卡每轮数组
    public int currentRound;  //当前波次的索引

    //构造函数，为roundList赋值(设置每一个Round的怪物列表，roundID，和所属关卡，同时设置责任链)
    public Level(int totalRound, List<Round.RoundInfo> roundInfoList)
    {
        currentRound = 0;
        this.totalRound = totalRound;
        roundList = new Round[totalRound];
        for (int i = 0; i < totalRound; i++)
        {
            roundList[i] = new Round(roundInfoList[i].monsterIDList, i, this); //创建Round对象
        }
        //设置责任链
        for (int i = 0; i < totalRound; i++)
        {
            if (i == totalRound - 1)
            {
                //最后一个Round对象不用设置下一个Round
                break;
            }
            roundList[i].SetNextRound(roundList[i+1]); 
        }
    }

    //处理请求，如果是当前关卡，生成怪物，否则传递给下一个round对象
    public void HandleRound()
    {
        if(currentRound >= totalRound)
        {
            //胜利
            currentRound--;  //纠正文本显示
            GameController.Instance.normalModelPanel.ShowGameWinPage(); //显示胜利界面
        }
        else if(currentRound == totalRound - 1)
        {
            //最后一波怪的UI显示和音效
            GameController.Instance.normalModelPanel.ShowFinalWaveUI(); 
        }
        else
        {
            roundList[currentRound].Handle(currentRound);
        }
    }

    //调用最后一回合的Handle方法
    public void HandleLastRound()
    {
        roundList[currentRound].Handle(currentRound);
    }

    public void AddRoundNum()
    {
        currentRound++;
    }
}
