using System.Collections.Generic;

//玩家管理，负责保持以及加载玩家以及游戏的信息
public class PlayerManager
{
    //统计面板数据
    public int adventureModelNum;   //冒险模式
    public int burriedLevelNum;     //隐藏关卡
    public int bossModelNum;        //Boss模式
    public int coin;                //总金钱
    public int monsterKilledNum;    //打死怪物总数
    public int bossKilledNum;       //打死Boss总数
    public int itemClearedNum;      //摧毁道具总数

    public List<bool> normalModelBigLevelList; //冒险模式大关卡解锁情况 true - 已解锁 false - 未解锁
    public List<int> unLockedNormalModelLevelList;      //每个大关卡已解锁的小关卡的数量
    //public List<int> totalNormalModelLevelNum;     //每个大关卡的小关卡总数
    public List<Stage> normalModelLevelList;   //所有的小关卡 从0-最大关卡数-1


    //怪物窝数据
    public int cookies;
    public int milk;
    public int nest;
    public int diamands;
    public List<MonsterPetData> monsterPetDataList;



    //用于玩家初始Json文件的制作
    //public PlayerManager()
    //{
    //    adventureModelNum = 0;
    //    burriedLevelNum = 0;
    //    bossModelNum = 0;
    //    coin = 0;
    //    monsterKilledNum = 0;
    //    bossKilledNum = 0;
    //    itemClearedNum = 0;
    //    cookies = 100;
    //    milk = 100;
    //    nest = 1;
    //    diamands = 10;
    //    unLockedNormalModelLevelList = new List<int>()
    //    {
    //        1,0,0
    //    };
    //    normalModelBigLevelList = new List<bool>()
    //    {
    //        true,false,false
    //    };
    //    normalModelLevelList = new List<Stage>()
    //    {
    //           new Stage(10,new int[]{ 1},false,0,1,1,true,false),
    //           new Stage(9,new int[]{ 2},false,0,2,1,false,false),
    //           new Stage(8,new int[]{ 1,2},false,0,3,1,false,false),
    //           new Stage(10,new int[]{ 3},false,0,4,1,false,false),
    //           new Stage(9,new int[]{ 1,2,3},false,0,5,1,false,true),
    //           new Stage(8,new int[]{ 2,3},false,0,1,2,false,false),
    //           new Stage(10,new int[]{ 1,3},false,0,2,2,false,false),
    //           new Stage(9,new int[]{ 4},false,0,3,2,false,false),
    //           new Stage(8,new int[]{ 1,4},false,0,4,2,false,false),
    //           new Stage(10,new int[]{ 2,4},false,0,5,2,false,true),
    //           new Stage(9,new int[]{ 3,4},false,0,1,3,false,false),
    //           new Stage(8,new int[]{ 5},false,0,2,3,false,false),
    //           new Stage(7,new int[]{ 4,5},false,0,3,3,false,false),
    //           new Stage(10,new int[]{ 1,3,5},false,0,4,3,false,false),
    //           new Stage(10,new int[]{ 1,4,5},false,0,5,3,false,true)
    //    };
    //    monsterPetDataList = new List<MonsterPetData>()
    //    {
    //        new MonsterPetData()
    //        {
    //            monsterID=1,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        },

    //    };
    //}


    //用于玩家所有关卡都解锁的Json文件的制作
    //public PlayerManager()
    //{
    //    adventureModelNum = 12;
    //    burriedLevelNum = 3;
    //    bossModelNum = 0;
    //    coin = 999;
    //    monsterKilledNum = 999;
    //    bossKilledNum = 0;
    //    itemClearedNum = 999;
    //    cookies = 1000;
    //    milk = 1000;
    //    nest = 10;
    //    diamands = 1000;
    //    unLockedNormalModelLevelList = new List<int>()
    //    {
    //        5,5,5
    //    };
    //    normalModelBigLevelList = new List<bool>()
    //    {
    //        true,true,true
    //    };
    //    normalModelLevelList = new List<Stage>()
    //    {
    //           new Stage(10,new int[]{ 1},false,0,1,1,true,false),
    //           new Stage(9,new int[]{ 2},false,0,2,1,true,false),
    //           new Stage(8,new int[]{ 1,2},false,0,3,1,true,false),
    //           new Stage(10,new int[]{ 3},false,0,4,1,true,false),
    //           new Stage(9,new int[]{ 1,2,3},false,0,5,1,false,true),
    //           new Stage(8,new int[]{ 2,3},false,0,1,2,true,false),
    //           new Stage(10,new int[]{ 1,3},false,0,2,2,true,false),
    //           new Stage(9,new int[]{ 4},false,0,3,2,true,false),
    //           new Stage(8,new int[]{ 1,4},false,0,4,2,true,false),
    //           new Stage(10,new int[]{ 2,4},false,0,5,2,false,true),
    //           new Stage(9,new int[]{ 3,4},false,0,1,3,true,false),
    //           new Stage(8,new int[]{ 5},false,0,2,3,true,false),
    //           new Stage(7,new int[]{ 4,5},false,0,3,3,true,false),
    //           new Stage(10,new int[]{ 1,3,5},false,0,4,3,true,false),
    //           new Stage(10,new int[]{ 1,4,5},false,0,5,3,true,false)

    //    };
    //    monsterPetDataList = new List<MonsterPetData>()
    //    {
    //        new MonsterPetData()
    //        {
    //            monsterID=1,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        },
    //        new MonsterPetData()
    //        {
    //            monsterID=2,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        },
    //        new MonsterPetData()
    //        {
    //            monsterID=3,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        }
    //    };
    //}

    //用于玩家所有关卡都解锁的Json文件的制作
    //public PlayerManager()
    //{
    //    adventureModelNum = 12;
    //    burriedLevelNum = 3;
    //    bossModelNum = 0;
    //    coin = 999;
    //    monsterKilledNum = 999;
    //    bossKilledNum = 0;
    //    itemClearedNum = 999;
    //    cookies = 1000;
    //    milk = 1000;
    //    nest = 10;
    //    diamands = 1000;
    //    unLockedNormalModelLevelList = new List<int>()
    //    {
    //        4,0,0
    //    };
    //    normalModelBigLevelList = new List<bool>()
    //    {
    //        true,false,false
    //    };
    //    normalModelLevelList = new List<Stage>()
    //    {
    //           new Stage(10,new int[]{ 1},false,0,1,1,true,false),
    //           new Stage(9,new int[]{ 2},false,0,2,1,true,false),
    //           new Stage(8,new int[]{ 1,2},false,0,3,1,true,false),
    //           new Stage(10,new int[]{ 3},false,0,4,1,true,false),
    //           new Stage(9,new int[]{ 1,2,3},false,0,5,1,false,true),
    //           new Stage(8,new int[]{ 2,3},false,0,1,2,false,false),
    //           new Stage(10,new int[]{ 1,3},false,0,2,2,false,false),
    //           new Stage(9,new int[]{ 4},false,0,3,2,false,false),
    //           new Stage(8,new int[]{ 1,4},false,0,4,2,false,false),
    //           new Stage(10,new int[]{ 2,4},false,0,5,2,false,true),
    //           new Stage(9,new int[]{ 3,4},false,0,1,3,false,false),
    //           new Stage(8,new int[]{ 5},false,0,2,3,false,false),
    //           new Stage(7,new int[]{ 4,5},false,0,3,3,false,false),
    //           new Stage(10,new int[]{ 1,3,5},false,0,4,3,false,false),
    //           new Stage(10,new int[]{ 1,4,5},false,0,5,3,true,false)

    //    };
    //    monsterPetDataList = new List<MonsterPetData>()
    //    {
    //        new MonsterPetData()
    //        {
    //            monsterID=1,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        },
    //        new MonsterPetData()
    //        {
    //            monsterID=2,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        },
    //        new MonsterPetData()
    //        {
    //            monsterID=3,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        }
    //    };
    //}

    //存储数据
    public void SaveData()
    {
        Memento memento = new Memento();
        memento.SaveByJson();
    }

    //读取数据
    public void ReadData()
    {
        Memento memento = new Memento();
        PlayerManager playerManager = memento.LoadByJson();
        //赋值
        //数据信息
        adventureModelNum = playerManager.adventureModelNum;
        burriedLevelNum = playerManager.burriedLevelNum;
        bossModelNum = playerManager.bossModelNum;
        coin = playerManager.coin;
        monsterKilledNum = playerManager.monsterKilledNum;
        bossKilledNum = playerManager.bossKilledNum;
        itemClearedNum = playerManager.itemClearedNum;
        cookies = playerManager.cookies;
        milk = playerManager.milk;
        nest = playerManager.nest;
        diamands = playerManager.diamands;
        //列表
        normalModelBigLevelList = playerManager.normalModelBigLevelList;
        unLockedNormalModelLevelList = playerManager.unLockedNormalModelLevelList;
        normalModelLevelList = playerManager.normalModelLevelList;
        monsterPetDataList = playerManager.monsterPetDataList;
    }
}
