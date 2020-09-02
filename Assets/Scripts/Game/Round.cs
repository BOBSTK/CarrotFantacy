using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  波次信息,负责具体产怪的逻辑
/// </summary>
public class Round 
{
    //当前波次的信息
    [System.Serializable] //序列化结构体
    public struct RoundInfo
    {
        public int[] monsterIDList; //怪物列表
    }

    public RoundInfo roundInfo;
    protected Round nextRound; //下一波
    protected int roundID;     
    protected Level level;     //所属关卡

    public Round(int[] monsterIDList,int roundID,Level level)
    {
        this.level = level;
        roundInfo.monsterIDList = monsterIDList; 
        this.roundID = roundID;
    }

    //设置下一波Round对象 -- 责任链
    public void SetNextRound(Round nextRound)
    {
        this.nextRound = nextRound;
    }

    //
    public void Handle(int roundID)
    {
        if(this.roundID < roundID)
        {
            //沿责任链传递给下一波
            nextRound.Handle(roundID);
        }
        else
        {
            //产生怪物     
            GameController.Instance.monsterIDList = roundInfo.monsterIDList; //将该轮怪物ID列表传给GameController
            GameController.Instance.CreateMonster();
            //GameController.Instance.isCreatingMonster = true;
        }
    }
}
