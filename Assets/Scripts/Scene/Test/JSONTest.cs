using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONTest : MonoBehaviour
{
    private AppOfTrigger appOfTrigger;

    private void Start()
    {
        //简单Json信息的存取
        //------------------
        //appOfTrigger = new AppOfTrigger
        //{
        //    appNum = 3,
        //    phoneState = true,
        //    appList = new List<string>()
        //    {
        //        "抖音","BiliBili","刺激战场"
        //    }
        //};
        //SaveByJson();
        //appOfTrigger = LoadJson();
        //Debug.Log(appOfTrigger.appNum);
        //Debug.Log(appOfTrigger.phoneState);
        //foreach(var item in appOfTrigger.appList)
        //{
        //    Debug.Log(item);
        //}

        //复杂Json信息的存取
        //------------------
        //appOfTrigger = new AppOfTrigger
        //{
        //    appNum = 3,
        //    phoneState = true,
        //    appList = new List<AppProperty>()
        //};
        //AppProperty appProperty = new AppProperty
        //{
        //    appName = "抖音",
        //    triggerID = "Trigger",
        //    triggerFavour = true,
        //    useTimeList = new List<int> { 6, 7, 8 }
        //};
        //appOfTrigger.appList.Add(appProperty);
        //SaveByJson();
        appOfTrigger = LoadJson();
        Debug.Log(appOfTrigger.appNum);
        Debug.Log(appOfTrigger.phoneState);
        foreach (var item in appOfTrigger.appList)
        {
            Debug.Log(item);
            Debug.Log(item.appName);
            Debug.Log(item.triggerFavour);
            Debug.Log(item.triggerID);
            foreach (var time in item.useTimeList)
            {
                Debug.Log(time);
            }
        }
       

    }

    //存储Json信息文件
    private void SaveByJson()
    {
        
        string filePath = Application.dataPath + "/Resources/JSON"+"/AppOfTrigger.json"; //文件路径

        //将信息类对象转化为json格式字符串
        string saveJsonStr = JsonMapper.ToJson(appOfTrigger);
        //创建一个文件流将字符串写入文件
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }

    //读取Json文件的方法
    private AppOfTrigger LoadJson()
    {
        AppOfTrigger app = new AppOfTrigger();
        string filePath = Application.dataPath + "/Resources/JSON" + "/AppOfTrigger.json";
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            app = JsonMapper.ToObject<AppOfTrigger>(jsonStr);
        }
        if (app == null)
            Debug.Log(filePath + " Json读取失败");
        return app;
    }
}
