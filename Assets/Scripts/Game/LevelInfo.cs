using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo 
{
    public int bigLevelID;
    public int levelID;

    //Json无法解析二维数组，需要用List存
    //public GridPoint.GridState[,] gridPoints; //只需要存储格子状态信息 
    public List<GridPoint.GridState> gridPoints;

    public List<GridPoint.GridIndex> monsterPathList;  //存储怪物路径的格子索引

    public List<Round.RoundInfo> roundInfoList; //存储波次信息

}
