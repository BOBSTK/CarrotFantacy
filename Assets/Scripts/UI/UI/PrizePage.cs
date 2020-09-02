using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 奖励页面
/// 显示奖励道具
/// </summary>
public class PrizePage : MonoBehaviour
{
    private Image img_Prize;           //奖品图片
    private Image img_Intro;           //奖品说明图片
    private Text  text_PrizeName;      //奖品名
    private Animator animator;         //奖励页面显示动画
    private NormalModelPanel normalModelPanel;
    public bool isPreGamePause;  //游戏状态

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        img_Prize = transform.Find("Img_Prize").GetComponent<Image>();
        img_Intro = transform.Find("Img_Introduction").GetComponent<Image>();
        text_PrizeName = transform.Find("Text_Name").GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    //每次玩家点击礼物盒时随机产生奖励
    private void OnEnable()
    {
        int random = Random.Range(1, 5);  //随机从1-4中产生一个整数
        string prizeName = "";
        //如果玩家还有宠物没有解锁(当前只有三个宠物) toDO 优化数据
        if(random >= 4 && GameManager.Instance.playerManager.monsterPetDataList.Count < 3) //宠物蛋
        {
            //获取一个未解锁的宠物蛋ID 1-3
            int randomEggNum = Random.Range(1,4);
            while (HasThePet(randomEggNum))
            {
                randomEggNum = Random.Range(1, 4);
            }
            //初始化宠物数据
            MonsterPetData monsterPetData = new MonsterPetData
            {
                monsterID = randomEggNum,
                monsterLevel = 1,
                remainCookies = 0,
                remainMilk = 0
            };
            GameManager.Instance.playerManager.monsterPetDataList.Add(monsterPetData);
            prizeName = "宠物蛋";
        }
        else //不是宠物蛋  
        {
            switch (random)
            {
                //toDo 随机数的饼干和牛奶数量
                case 1:  //牛奶
                    prizeName = "牛奶";
                    GameManager.Instance.playerManager.milk += 10;
                    break;
                case 2:  //饼干
                    prizeName = "饼干";
                    GameManager.Instance.playerManager.cookies += 20;
                    break;
                case 3:  //窝
                    prizeName = "窝";
                    GameManager.Instance.playerManager.nest += 1;
                    break;
                case 4:  //已经超过怪物最大数量，改为给玩家饼干
                    prizeName = "饼干";
                    GameManager.Instance.playerManager.cookies += 15;
                    break;
                default:
                    break;
            }
        }
        //显示奖品信息,播放动画
        text_PrizeName.text = prizeName;
        img_Intro.sprite = GameController.Instance.GetSprite("MonsterNest/Prize/Instruction"+random.ToString());
        img_Prize.sprite = GameController.Instance.GetSprite("MonsterNest/Prize/Prize" + random.ToString());
        animator.Play("Enter");
    }

    /*
     *  判断玩家是否解锁对应的宠物
     *  参数：
     *       int monsterID: 要查找的怪物ID
     *  返回：
     *       true - 玩家已解锁怪物  false - 玩家没有解锁怪物
     */
    private bool HasThePet(int monsterID)
    {
        for (int i = 0; i < GameManager.Instance.playerManager.monsterPetDataList.Count; i++)
        {
            if (GameManager.Instance.playerManager.monsterPetDataList[i].monsterID == monsterID)
                return true;
        }
        return false;
    }

    /*
     * 关闭奖励页面
     */
    public void ClosePrizePage()
    {
        transform.gameObject.SetActive(false);
        GameController.Instance.PlayButtonAudioClip();
        GameController.Instance.isPause = isPreGamePause;
    }
}
