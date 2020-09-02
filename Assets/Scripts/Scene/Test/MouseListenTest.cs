using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseListenTest : MonoBehaviour
{

    //OnMouse类型的API只作用于左键
    private void OnMouseDown()
    {
        //Debug.Log("游戏物体：鼠标按下");
        //if (Input.GetKey(KeyCode.P))
        //{
        //    Debug.Log("创建怪物路径");
        //}
        //else if (Input.GetKey(KeyCode.I))
        //{
        //    Debug.Log("创建道具");
        //}
        Debug.Log(transform.name);
    }

}



//private void OnMouseOver()
//{
//    if (Input.GetMouseButtonDown(0))
//    {
//        Debug.Log("创建道具");
//    }
//    else if (Input.GetMouseButtonDown(1))
//    {
//        Debug.Log("创建怪物路径");
//    }
//}

