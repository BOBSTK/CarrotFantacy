using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if Tool  //MapTool 只在工具模式下才存在
[CustomEditor(typeof(MapMaker))]  //将编辑器附加到自定义组件（这里会加入MapMaker组件中）
public class MapTool : Editor  //为自定义对象创建自定义检视面板和编辑器
{
    private MapMaker mapMaker;

    //关卡文件列表
    private List<FileInfo> fileList = new List<FileInfo>();  //关卡的Json文件列表
    private string[] fileNameList;  //关卡文件名字列表

    //当前编辑的关卡索引
    private int selectIndex = -1;

    //绘制操作面板
    public override void OnInspectorGUI()  
    {
        base.OnInspectorGUI();
        //程序运行时才绘制
        if (Application.isPlaying)
        {
            mapMaker = MapMaker.Instance;
            //第一行
            EditorGUILayout.BeginHorizontal();
            //获取操作文件名
            fileNameList = GetNames(fileList);
            int currentIndex = EditorGUILayout.Popup(selectIndex, fileNameList); //下拉列表 返回用户选择的索引
            if (currentIndex != selectIndex) //选择对象改变
            {
                selectIndex = currentIndex;
                //实例化地图的方法
                mapMaker.InitMap();
                //加载当前选择的level文件
                mapMaker.LoadLevelInfo(mapMaker.LoadLevelInfoFile(fileNameList[selectIndex]));
            }
            if (GUILayout.Button("读取关卡列表")) //当用户单击按钮时返回true
            {
                LoadLevelFiles();
            }
            EditorGUILayout.EndHorizontal();

            //第二行
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("回复地图编辑器默认状态"))
            {
                mapMaker.RecoverTowerPoint();
            }

            if(GUILayout.Button("清除怪物路径"))
            {
                mapMaker.ClearMonsterPath();
            }

            if(GUILayout.Button("更新物品"))
            {
                //Debug.Log(mapMaker.bigLevelID + " " + mapMaker.levelID);
                // Debug.Log(mapMaker.grid.GetComponent<GridPoint>()) ;
                //mapMaker.grid.GetComponent<GridPoint>().UpdateItem(); //更新物品信息
                // mapMaker.InitMapMaker(); //初始化MapMaker
                mapMaker.UpdateGrid();
            }
            EditorGUILayout.EndHorizontal();

            //第三行
            if(GUILayout.Button("保存当前关卡数据文件"))
            {
                mapMaker.SaveLevelFileByJson();
            }
        }
    }

    //加载关卡数据文件
    private void LoadLevelFiles()
    {
        ClearList();
        fileList = GetLevelFiles();
        //fileNameList = GetNames(fileList);
    }

    //清除文件列表
    private void ClearList()
    {
        fileList.Clear();  //清空文件列表
        selectIndex = -1;  //重置Index
    }

    //读取关卡列表
    private List<FileInfo> GetLevelFiles()
    {
        /*
         * searchPattern可以是文字和通配符的组合，但不支持正则表达式
         * * （星号）	此位置中的零个或多个字符。
         * ? （问号）	此位置中的零个或一个字符。
         */
        string[] files = Directory.GetFiles(Application.streamingAssetsPath+"/JSON/Level/","*.json");  //找到所有以.json结尾的文件
        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo fileInfo= new FileInfo(files[i]);
            list.Add(fileInfo);
        }
        return list;
    }

    //获取关卡文件的名字
    private string[] GetNames(List<FileInfo> files)
    {
        List<string> names = new List<string>();
        foreach (var file in files)
        {
            names.Add(file.Name);
        }
        return names.ToArray();  //将List转成Array
    }
}

#endif
