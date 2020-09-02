using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuilder : IBuilder<Monster>
{
    public int monsterID;        //怪物ID
    private GameObject monsterGo;  //怪物对象
    public void GetData(Monster productClass)
    {
        //ToDo 使用JSON文件存储并给怪物属性赋值
        productClass.monsterID = monsterID;
        productClass.HP = monsterID * 100;
        productClass.currentHP = productClass.HP;
        productClass.initMoveSpeed = monsterID;
        productClass.moveSpeed = monsterID;
        productClass.prize = monsterID * 50;
    }

    public void GetOtherResource(Monster productClass)
    {
        productClass.GetMonsterProperty(); //为怪物的Animator组件赋值设置动画播放
    }

    public GameObject GetProduct()
    {
        GameObject item = GameController.Instance.GetGameObject("MonsterPrefab");
        Monster monster = GetProductClass(item);
        GetData(monster);
        GetOtherResource(monster);
        return item;
    }

    public Monster GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Monster>();
    }
}
