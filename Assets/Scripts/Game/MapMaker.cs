using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// 地图编辑器工具，游戏中负责产生地图
/// </summary>
public class MapMaker : MonoBehaviour
{
    //使用宏定义标记方法 (MapMaker需要作为工具类(单例)和游戏中出现)
    //工具
#if Tool
    public bool drawLine; //是否画线
    public GameObject grid; //格子，之后应当实例化
   // public GridPoint gridPoint; //格子脚本

     private static MapMaker _instance;  

    public static MapMaker Instance { get => _instance;  }
#endif
    //地图相关属性
    //------------
    //地图
    private float mapWidth;
    private float mapHeight;
    //格子
    [HideInInspector]
    public float gridWidth;
    [HideInInspector]
    public float gridHeight;
    //全部的格子对象
    public GridPoint[,] gridPoints;

    public const int yRow = 8;    //行
    public const int xColum = 12; //列

    //怪物路径
    [HideInInspector]
    public List<GridPoint.GridIndex> monsterPath;
    //怪物路径点的具体位置
    [HideInInspector]
    public List<Vector3> monsterPathPos;

    private SpriteRenderer bgSR;
    private SpriteRenderer roadSR;

    
    public List<Round.RoundInfo> roundInfoList;//每一波的怪物ID列表

    //当前关卡索引
    //[HideInInspector]
    public int bigLevelID;
    //[HideInInspector]
    public int levelID;

    [HideInInspector]
    public Carrot carrot; //萝卜对象

   

    private void Awake()
    {
#if Tool
        _instance = this;
        InitMapMaker();
#endif

    }

    //初始化地图编辑工具
    public void InitMapMaker()
    {
        CalculateSize();
        gridPoints = new GridPoint[xColum,yRow];
        monsterPath = new List<GridPoint.GridIndex>();
        for (int x = 0; x < xColum; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
#if Tool
                //工具类中获得gridItem的方法
                GameObject gridItem = Instantiate(grid,transform.position,transform.rotation); //生成格子对象
#endif

#if Game
                //通过工厂获得gridItem的方法
                GameObject gridItem = GameController.Instance.GetGameObject("Grid");
#endif

                gridItem.transform.position = CorrectPosition(x*gridWidth,y*gridHeight);  //设置格子的位置
                gridItem.transform.SetParent(transform);
                gridPoints[x, y] = gridItem.GetComponent<GridPoint>();
                //设置gridIndex
                gridPoints[x, y].gridIndex.xIndex = x;
                gridPoints[x, y].gridIndex.yIndex = y;

            }
        }
        bgSR = transform.Find("BG").GetComponent<SpriteRenderer>();
        roadSR = transform.Find("Road").GetComponent<SpriteRenderer>();
    }

#if Tool
    public void UpdateGrid()
    { 
        for (int x = 0; x < xColum; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                if (gridPoints[x, y].gridState.hasItem)
                {
                    Destroy(gridPoints[x, y].currentItem);
                }
                Destroy(gridPoints[x, y].gameObject);
                //工具类中获得gridItem的方法
                GameObject gridItem = Instantiate(grid, transform.position, transform.rotation); //生成格子对象
                gridItem.transform.position = CorrectPosition(x * gridWidth, y * gridHeight);  //设置格子的位置
                gridItem.transform.SetParent(transform);
                gridPoints[x, y] = gridItem.GetComponent<GridPoint>();
                //设置gridIndex
                gridPoints[x, y].gridIndex.xIndex = x;
                gridPoints[x, y].gridIndex.yIndex = y;
            }
        }         
    }
#endif

#if Game
     //加载地图(游戏运行中)
     public void LoadMap(int bigLevelID, int levelID)
    {
        this.bigLevelID = bigLevelID;
        this.levelID = levelID;
        //加载当前关卡的level文件
        LoadLevelInfo(LoadLevelInfoFile("Level" + bigLevelID.ToString() + "_" + levelID.ToString() + ".json"));
       // mapMaker.LoadLevelInfo(mapMaker.LoadLevelInfoFile(fileNameList[selectIndex]));
        monsterPathPos = new List<Vector3>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            //将怪物路径(GridPoint.GridIndex)对应的位置信息(Vector3)存入monsterPathPos
            monsterPathPos.Add(gridPoints[monsterPath[i].xIndex, monsterPath[i].yIndex].transform.position);
        }
        //起始点与终止点
        GameObject startPoint = GameController.Instance.GetGameObject("StartPoint");
        startPoint.transform.position = monsterPathPos[0];  //将起始点的位置设在怪物路径的第一个点
        startPoint.transform.SetParent(transform);

        GameObject endPoint = GameController.Instance.GetGameObject("Carrot");  //获取终点(萝卜)
        endPoint.transform.position = monsterPathPos[monsterPathPos.Count - 1]; //将萝卜的位置设在怪物路径的最后一个点
        endPoint.transform.SetParent(transform);
        carrot = endPoint.GetComponent<Carrot>(); //获取Carrot脚本
    }
#endif

    //纠正格子位置
    public Vector3 CorrectPosition(float x, float y)
    {
        return new Vector3(x - mapWidth / 2 + gridWidth/2, y - mapHeight / 2 + gridHeight/2);
    }
     
    //计算地图格子宽高
    private void CalculateSize()
    {
        //将屏幕左下(0,0)和右上(1,1)由视口坐标转为世界坐标
        Vector3 leftDown = Camera.main.ViewportToWorldPoint(new Vector3(0, 0)); 
        Vector3 rightUp = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
        mapWidth = rightUp.x - leftDown.x;
        mapHeight = rightUp.y - leftDown.y;
        gridWidth = mapWidth / xColum;
        gridHeight = mapHeight / yRow;
    }

#if Tool
    //画格子辅助设计 编辑器扩展方法 
    private void OnDrawGizmos()
    {
        if (drawLine)
        {
            CalculateSize();
            Gizmos.color = Color.green;
            //画行
            for(int y=0; y <= yRow; y++)
            {
                Vector3 startPos = new Vector3(-mapWidth/2,-mapHeight/2 + y*gridHeight);
                Vector3 endPos = new Vector3(mapWidth / 2, -mapHeight / 2 + y * gridHeight);
                Gizmos.DrawLine(startPos,endPos);
            }
            //画列
            for(int x = 0; x <= xColum; x++)
            {
                Vector3 startPos = new Vector3(-mapWidth / 2 + x*gridWidth, mapHeight/2);
                Vector3 endPos = new Vector3(-mapWidth / 2 + x * gridWidth, -mapHeight / 2);
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }
#endif

    /// <summary>
    /// 地图编辑的方法
    /// </summary>
    
    //清除怪物路径
    public void ClearMonsterPath()
    {
        //Debug.Log("ClearMonsterPath: "+ monsterPath.Count);
        //遍历当前的怪物路径列表，将所有怪物路径点初始化
        for (int i = 0; i < monsterPath.Count; i++)
        {
            gridPoints[monsterPath[i].xIndex, monsterPath[i].yIndex].InitGrid(); 
        }
        monsterPath.Clear();

    }

    //恢复地图编辑默认状态
    public void RecoverTowerPoint() 
    {
        ClearMonsterPath(); //清除怪物路径
        //重置格子状态
        for (int x = 0; x < xColum; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].InitGrid();
            }
        }

        //清除当前关卡信息
        roundInfoList.Clear();
        bigLevelID = 0;
        levelID = 0;
    }

    //初始化地图
    public void InitMap()
    {
        bigLevelID = 0;
        levelID = 0;
        RecoverTowerPoint();
        roundInfoList.Clear();
        bgSR.sprite = null;
        roadSR.sprite = null;
    }

#if Tool
//保存关卡的方法(用于地图编辑)
    //生成LevelInfo对象
    private LevelInfo CreateLevelInfo()
    {
        LevelInfo levelInfo = new LevelInfo
        {
            bigLevelID = this.bigLevelID,
            levelID = this.levelID
        };

        //记录格子的状态
        levelInfo.gridPoints = new List<GridPoint.GridState>();
        for (int x = 0; x < xColum; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                levelInfo.gridPoints.Add(gridPoints[x, y].gridState); 
            }
        }

        //记录怪物路径
        levelInfo.monsterPathList = new List<GridPoint.GridIndex>();
        for(int i = 0; i < monsterPath.Count; i++)
        {
            levelInfo.monsterPathList.Add(monsterPath[i]);
        }

        //记录波次信息
        levelInfo.roundInfoList = new List<Round.RoundInfo>();
        for(int i=0; i<roundInfoList.Count;i++)
        {
            levelInfo.roundInfoList.Add(roundInfoList[i]);
        }
        Debug.Log(bigLevelID+"-"+ levelID+ " LevelInfo保存成功");
        return levelInfo;
    }

    //保存当前关卡的Json数据文件
    public void SaveLevelFileByJson()
    {
        LevelInfo levelInfo = CreateLevelInfo();
        string filePath = Application.streamingAssetsPath + "/JSON/Level/" + "Level" + bigLevelID.ToString() + "_" + levelID.ToString() + ".json";
        string saveJsonStr = JsonMapper.ToJson(levelInfo);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
        Debug.Log(bigLevelID + "-" + levelID + " json文件保存成功");
    }
#endif

    //读取关卡文件
    public LevelInfo LoadLevelInfoFile(string fileName)
    {
        LevelInfo levelInfo = new LevelInfo();
        string filePath = Application.streamingAssetsPath + "/JSON/Level/" + fileName;
        if (System.IO.File.Exists(filePath))
        {
           // Debug.Log("文件打开！");
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            //Debug.Log("成功加载Json文件" + filePath);
            //Debug.Log(jsonStr);
            levelInfo = JsonMapper.ToObject<LevelInfo>(jsonStr);
            return levelInfo;
        }
        Debug.Log("文件加载失败，加载路径是: "+filePath);
        return null;
    }

    //加载关卡文件,生成地图
    public void LoadLevelInfo(LevelInfo levelInfo)
    {
        bigLevelID = levelInfo.bigLevelID;
        levelID = levelInfo.levelID;
        //Debug.Log(levelInfo.gridPoints.Length);
        //Debug.Log(gridPoints.Length);
        //更新格子状态
        for (int x = 0; x < xColum; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].gridState  = levelInfo.gridPoints[y+x*yRow];
                //更新格子的状态
       
               gridPoints[x, y].UpdateGrid(); 

            }
        }

        //加载怪物路径
        ClearMonsterPath();
        monsterPath = new List<GridPoint.GridIndex>();
        for (int i = 0; i < levelInfo.monsterPathList.Count; i++)
        {
            monsterPath.Add(levelInfo.monsterPathList[i]);
        }

        //加载波次信息
        roundInfoList = new List<Round.RoundInfo>();
        for (int i = 0; i < levelInfo.roundInfoList.Count; i++)
        {
            roundInfoList.Add(levelInfo.roundInfoList[i]);
        }
        //ToDo 可以加一个信息存储背景图片
        
        bgSR.sprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/"+bigLevelID.ToString()+"/BG"+(levelID/3).ToString());
        
        roadSR.sprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/" + bigLevelID.ToString() + "/Road" + levelID);
       
    }


}
