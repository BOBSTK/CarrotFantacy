using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    
    public float maxDistance;//射线最大检测距离
    void Start()
    {
        maxDistance = 10.0f;
    }
    public void Update()
    {
        //originPos = transform.position ;//射线起始位置
        //direction = transform.forward;//射线方向
        //Vector3 targetPos = originPos + transform.forward * maxDistance;//射线最大检测距离处的点位置，找到起始点与终点，方便画线
        //Ray ray = new Ray(originPos, direction);//创建名为ray的射线
        //RaycastHit hit;//碰撞检测信息存储
        //if (Physics.Raycast(ray, out hit, maxDistance))
        //{//碰撞检测
        //    Debug.DrawLine(originPos, hit.point);//画线显示
        //    Debug.Log(hit.collider.name);//打印检测到的碰撞体名称
        //}
        //else
        //{
        //    Debug.DrawLine(originPos, targetPos);//没检测到碰撞体，则以最大检测距离画线
        //}
        Debug.DrawLine(transform.position, transform.position + transform.forward * maxDistance); //以最大检测距离画线
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, maxDistance);
        

        if (Input.GetMouseButtonDown(0)) //检测塔
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.transform.name == "Tower") ;
                {
                    Debug.Log(hit.transform.name);
                }
            }
            Debug.Log("没有找到塔");
        }
        else if (Input.GetMouseButtonDown(1))  //检测怪物
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.transform.name == "Monster") ;
                {
                    Debug.Log(hit.transform.name);
                    return;
                }
            }
            Debug.Log("没有找到怪物");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(hits.Length);
             for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Debug.Log(hit.transform.name);
                
            }
        }
    }
}
