using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 水晶塔 魔法球
/// </summary>
public class Crystal : TowerSpecificProperty
{
    //属性
    private float distance;      //塔和目标之间的距离
    private float bulletWidth;   //子弹宽度
    private float bulletLength;  //子弹长度

    //引用
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        //取到子弹对象
        bulletGo = GameController.Instance.GetGameObject("Tower/ID"+tower.towerID.ToString()+"/Bullet/"+towerLevel.ToString());
        bulletGo.SetActive(false); //只有在攻击时显示子弹
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GameController.Instance.GetAudioClip("NormalModel/Tower/Attack/" + tower.towerID.ToString());
    }

    private void OnEnable()
    {
        if(animator == null)
        {
            return;
        }
        //取到子弹对象
        bulletGo = GameController.Instance.GetGameObject("Tower/ID" + tower.towerID.ToString() + "/Bullet/" + towerLevel.ToString());
        bulletGo.SetActive(false); //只有在攻击时显示子弹
    }

    protected override void Update()
    {
        if (GameController.Instance.isPause || targetTrans == null || GameController.Instance.isGameOver)  //游戏暂停或者丢失目标
        {
            if(targetTrans == null)
            {
                bulletGo.SetActive(false); //如果物体离开攻击范围，需要隐藏子弹
            }
            return;
        }

        if (!targetTrans.gameObject.activeSelf) //目标失活
        {
            targetTrans = null;
            return;
        }
        Attack();
    }

    
    protected override void Attack()
    {
        if (targetTrans == null) //没有攻击目标
        {
            return;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
       
        animator.Play("Attack");//塔播放攻击动画

        //攻击方法
        if(targetTrans.gameObject.tag == "Item") //目标是道具
        {
            //水晶塔和道具间的距离
            distance = Vector3.Distance(transform.position,targetTrans.position + new Vector3(0,0,3));
        }
        else if(targetTrans.gameObject.tag == "Monster")//目标是怪物
        {
            //水晶塔和怪物间的距离
            distance = Vector3.Distance(transform.position, targetTrans.position);
        }
        //子弹属性 (为什么，优化)
        //-----------------------
        //子弹宽度 （0.5 - 1.0）
        bulletWidth = Mathf.Max(3 / distance,0.5f);
        bulletWidth = Mathf.Min(bulletWidth, 1.0f);

        //子弹长度
        bulletLength = distance / 2;

        //设置子弹的位置和属性
        bulletGo.transform.position = new Vector3(targetTrans.position.x + transform.position.x, targetTrans.position.y + transform.position.y,0) / 2;
        bulletGo.transform.localScale = new Vector3(1, bulletWidth, bulletLength);
        bulletGo.SetActive(true);  //显示子弹
        bulletGo.GetComponent<Bullet>().targetTrans = targetTrans; //设置子弹的目标
    }

    //销毁魔法球的方法
    protected override void Destroy()
    {
        //销毁子弹
        bulletGo.SetActive(false); //隐藏子弹
        //将子弹对象放入对象池
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + tower.towerID.ToString() + "/Bullet/" + towerLevel.ToString(), bulletGo);
        bulletGo = null;
        
        base.Destroy(); //销毁塔
    }
}
