using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppProperty 
{
    public string appName;
    public string triggerID;
    public bool triggerFavour;
    public List<int> useTimeList;

    public void TestMethod()
    {
        Debug.Log("测试方法");
    }


    private void TestMethod2()
    {
        Debug.Log("测试2");
    }
}
