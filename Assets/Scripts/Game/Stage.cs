using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏关卡信息
/// </summary>
public class Stage 
{
    public int[] towerIDList;       //本关卡可以建的塔种类
    public bool isAllClear;           //是否清空此关卡道具
    public int carrotState;         //萝卜状态
    public int levelID;             //小关卡ID
    public int bigLevelID;          //大关卡ID
    public bool isUnLocked;           //此关卡是否解锁
    public bool isRewardLevel;      //是否为奖励关卡
    public int totalRound;          //一共有几波怪

    //构造函数
    //public Stage(int totalRound, int[] towerIDList,
    //    bool isAllClear, int carrotState, int levelID, int bigLevelID,
    //    bool isUnLocked, bool isRewardLevel
    //    )
    //{
    //    this.totalRound = totalRound;
    //    this.towerIDList = towerIDList;
    //    this.isAllClear = isAllClear;
    //    this.carrotState = carrotState;
    //    this.levelID = levelID;
    //    this.bigLevelID = bigLevelID;
    //    this.isUnLocked = isUnLocked;
    //    this.isRewardLevel = isRewardLevel;
    //}
}
