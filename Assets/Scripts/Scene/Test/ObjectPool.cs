using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject monster;//获取资源
    private Stack<GameObject> monsterPool; //怪物对象池
    private Stack<GameObject> activeMonsterList;//已经激活的对象，用于测试
    private void Start()
    {
        monsterPool = new Stack<GameObject>();
        activeMonsterList = new Stack<GameObject>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject itemGO = GetMonster();
            itemGO.transform.position = Vector3.one;
            activeMonsterList.Push(itemGO);
        }else if (Input.GetMouseButtonDown(1))
        {
            if (activeMonsterList.Count > 0)
            {
                PushMonster(activeMonsterList.Pop());
            }
        }
    }

    private GameObject GetMonster()
    {
        GameObject monsterGO = null;
        if(monsterPool.Count <= 0)
        {
            monsterGO = Instantiate(monster);
        }
        else
        {
            monsterGO = monsterPool.Pop();
            monsterGO.SetActive(true);
        }
        return monsterGO;
    }

    private void PushMonster(GameObject monsterGO)
    {
        monsterGO.transform.SetParent(transform);
        monsterGO.SetActive(false);
        monsterPool.Push(monsterGO);
    }
}
