using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 宠物类
/// </summary>
public class MonsterPet : MonoBehaviour
{
    //属性
    public MonsterPetData monsterPetData;

    //引用
    public Sprite[] buttonSprites;  //喂养按钮图片 0-可用milk 1-不可用milk 2-可用cookies 3-不可用cookies
    private GameObject[] monsterLevel; //宠物对应的三个等级的游戏物体
    //Egg
    private GameObject img_Instruction; //没有足够窝的提示
    //Baby
    private GameObject emp_Feed;  
    private Text text_Milk;
    private Text text_Cookies;
    private Button btn_Milk;
    private Button btn_Cookies;
    private Image img_Btn_Milk;
    private Image img_Btn_Cookies;
    //Normal
    private GameObject img_TalkRight;
    private GameObject img_TalkLeft;

    public MonsterNestPanel monsterNestPanel;

    private void Awake()
    {
        //获取资源
        monsterLevel = new GameObject[3];
        monsterLevel[0] = transform.Find("Emp_Egg").gameObject;
        monsterLevel[1] = transform.Find("Emp_Baby").gameObject;
        monsterLevel[2] = transform.Find("Emp_Normal").gameObject;
        //按钮图片
        buttonSprites = new Sprite[4];
        buttonSprites[0] = GameManager.Instance.GetSprite("MonsterNest/MonsterNest_14");
        buttonSprites[1] = GameManager.Instance.GetSprite("MonsterNest/MonsterNest_37");
        buttonSprites[2] = GameManager.Instance.GetSprite("MonsterNest/MonsterNest_19");
        buttonSprites[3] = GameManager.Instance.GetSprite("MonsterNest/MonsterNest_27");

        //Egg
        img_Instruction = monsterLevel[0].transform.Find("Img_Instruction").gameObject;
        img_Instruction.SetActive(false);
        //Baby
        emp_Feed = monsterLevel[1].transform.Find("Emp_Feed").gameObject;
        emp_Feed.SetActive(false);
        btn_Milk = monsterLevel[1].transform.Find("Emp_Feed").Find("Btn_Milk").GetComponent<Button>();
        img_Btn_Milk = monsterLevel[1].transform.Find("Emp_Feed").Find("Btn_Milk").GetComponent<Image>();
        btn_Cookies = monsterLevel[1].transform.Find("Emp_Feed").Find("Btn_Cookie").GetComponent<Button>();
        img_Btn_Cookies = monsterLevel[1].transform.Find("Emp_Feed").Find("Btn_Cookie").GetComponent<Image>();
        text_Milk = monsterLevel[1].transform.Find("Emp_Feed").Find("Btn_Milk").Find("Text").GetComponent<Text>();
        text_Cookies = monsterLevel[1].transform.Find("Emp_Feed").Find("Btn_Cookie").Find("Text").GetComponent<Text>();
        //Normal
        img_TalkRight = transform.Find("Img_TalkRight").gameObject;
        img_TalkLeft = transform.Find("Img_TalkLeft").gameObject;
    }

    private void OnEnable()
    {
        InitMonseterPet();
    }

    //显示当前等级的宠物
    private void ShowMonster()
    {
        for (int i = 0; i < monsterLevel.Length; i++)
        {
            monsterLevel[i].SetActive(false);
            if ((i + 1) == monsterPetData.monsterLevel) //对宠物对应等级的游戏物体进行操作
            {
                monsterLevel[i].SetActive(true);  
                Sprite petSprite = null;
                //根据宠物等级加载显示图片
                switch (monsterPetData.monsterLevel)
                {
                    case 1:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Egg/" + monsterPetData.monsterID.ToString());
                        break;
                    case 2:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Baby/" + monsterPetData.monsterID.ToString());
                        break;
                    case 3:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Normal/" + monsterPetData.monsterID.ToString());
                        break;
                    default:
                        break;
                }
                //宠物每个等级的游戏物体都有这一组件
                Image monsterImage = monsterLevel[i].transform.Find("Img_Pet").GetComponent<Image>();
                monsterImage.sprite = petSprite;
                monsterImage.SetNativeSize();
                //设置图片缩放规模
                float imageScale = 0; 
                if(monsterPetData.monsterLevel == 1) //蛋状态
                {
                    imageScale = 2;
                }
                else
                {
                    imageScale = 1 + (monsterPetData.monsterLevel - 1) * 0.5f;
                }
                monsterImage.transform.localScale = new Vector3(imageScale, imageScale, 1);
            }
        }
    }

    //点击宠物的触发事件方法
    public void ClickPet()
    {
        GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.GetAudioClip("MonsterNest/PetSound"+monsterPetData.monsterLevel.ToString()));
        
        switch (monsterPetData.monsterLevel) //根据宠物的等级有不同的事件
        {
            case 1:  //Egg toDo 增加确认界面
                if(GameManager.Instance.playerManager.nest >= 1) //有窝
                {
                    GameManager.Instance.playerManager.nest--;
                    //升级 更新显示 toDo 增加孵化时间
                    monsterPetData.monsterLevel++;
                    Grow();
                    ShowMonster();
                    monsterNestPanel.UpdateText();
                }
                else  //没有窝
                {
                    img_Instruction.SetActive(true); //显示提示界面
                    Invoke("CloseTalkUI", 2);        //2s后关闭
                }
                break;
            case 2: //Baby
                if (emp_Feed.activeSelf) //如果是显示状态，点击后隐藏喂养界面
                {
                    emp_Feed.SetActive(false);
                }
                else
                {
                    emp_Feed.SetActive(true);
                    //牛奶
                    if(GameManager.Instance.playerManager.milk == 0) //玩家没有牛奶了
                    {
                        img_Btn_Milk.sprite = buttonSprites[1];  //设置按钮图标
                        btn_Milk.interactable = false;           //不可交互
                    }
                    else
                    {
                        img_Btn_Milk.sprite = buttonSprites[0];  //设置按钮图
                        btn_Milk.interactable = true;            //可以交互
                    }

                    if (monsterPetData.remainMilk == 0)  //怪物牛奶喂养达标
                    {
                        btn_Milk.gameObject.SetActive(false);  //不再显示牛奶操作按钮
                    }
                    else
                    {
                        text_Milk.text = monsterPetData.remainMilk.ToString(); //显示剩余牛奶
                        btn_Milk.gameObject.SetActive(true);
                    }

                    //饼干
                    if (GameManager.Instance.playerManager.cookies == 0) //玩家没有饼干了
                    {
                        img_Btn_Cookies.sprite = buttonSprites[3];  //设置按钮图标
                        btn_Cookies.interactable = false;           //不可交互
                    }
                    else
                    {
                        img_Btn_Cookies.sprite = buttonSprites[2];  //设置按钮图
                        btn_Cookies.interactable = true;            //可以交互
                    }

                    if (monsterPetData.remainCookies == 0)  //怪物饼干喂养达标
                    {
                        btn_Cookies.gameObject.SetActive(false);  //不再显示牛奶操作按钮
                    }
                    else
                    {
                        text_Cookies.text = monsterPetData.remainCookies.ToString(); //显示剩余饼干
                        btn_Cookies.gameObject.SetActive(true);
                    }
                }
                break;

            case 3: //Normal
                int random = Random.Range(0, 2); // 0-1 的随机数
                if(random == 1)
                {
                    img_TalkRight.SetActive(true);
                    Invoke("CloseTalkUI", 2);        //2s后关闭
                }
                else
                {
                    img_TalkLeft.SetActive(true);
                    Invoke("CloseTalkUI", 2);        //2s后关闭
                }
                break;
            default:
                break;
        }
    }

    //喂牛奶
    public void FeedMilk()
    {
        //播放喂养动画与音效
        GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.GetAudioClip("MonsterNest/Feed01"));
        //获取心跳动画
        GameObject heartGo = GameManager.Instance.GetGameObject(FactoryType.UIFactory, "Img_Heart");
        heartGo.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGo.transform);  //设置在Canvas下
        if(GameManager.Instance.playerManager.milk >= monsterPetData.remainMilk) //牛奶足够
        {
            //喂养牛奶
            GameManager.Instance.playerManager.milk -= monsterPetData.remainMilk;
            monsterPetData.remainMilk = 0;
            //更新文本
           // Debug.Log("Aftert FeedMilk: " + monsterPetData.remainCookies);
            monsterNestPanel.UpdateText();
        }
        else  //牛奶不够
        {
            monsterPetData.remainMilk -= GameManager.Instance.playerManager.milk;
            GameManager.Instance.playerManager.milk = 0;
            btn_Milk.gameObject.SetActive(false);
            //更新文本
            monsterNestPanel.UpdateText();
        }
        emp_Feed.SetActive(false);
        Invoke("Grow", 0.433f); //在播放完心跳动画后刷新宠物
    }

    //喂饼干
    public void FeedCookie()
    {
        //播放喂养动画与音效
        GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.GetAudioClip("MonsterNest/Feed02"));
        //获取心跳动画
        GameObject heartGo = GameManager.Instance.GetGameObject(FactoryType.UIFactory, "Img_Heart");
        heartGo.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGo.transform);  //设置在Canvas下
        if (GameManager.Instance.playerManager.cookies >= monsterPetData.remainCookies) //饼干足够
        {
            //喂养牛奶
            GameManager.Instance.playerManager.cookies -= monsterPetData.remainCookies;
            monsterPetData.remainCookies = 0;
            //更新文本
            monsterNestPanel.UpdateText();
        }
        else  //饼干不够
        {
            monsterPetData.remainCookies -= GameManager.Instance.playerManager.cookies;
            GameManager.Instance.playerManager.cookies = 0;
            btn_Cookies.gameObject.SetActive(false);
            //更新文本
            monsterNestPanel.UpdateText();
        }
        emp_Feed.SetActive(false);
        Invoke("Grow", 0.433f); //在播放完心跳动画后刷新宠物

    }

    //关闭所有对话框
    private void CloseTalkUI()
    {
        img_Instruction.SetActive(false);
        img_TalkRight.SetActive(false);
        img_TalkLeft.SetActive(false);
    }

    //成长方法
    //包括 Egg -> Baby -> Normal
    private void Grow()
    {
        if(monsterPetData.remainMilk == 0 && monsterPetData.remainCookies == 0) //达到喂养条件
        {
            GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.GetAudioClip("MonsterNest/PetChange"));
            //播放音效
            monsterPetData.monsterLevel++;
            if(monsterPetData.monsterLevel >= 3) //宠物成熟
            {
                //解锁对应的隐藏关卡
                GameManager.Instance.playerManager.normalModelLevelList[monsterPetData.monsterID * 5 - 1].isUnLocked = true;  //解锁对应的隐藏关卡
                GameManager.Instance.playerManager.burriedLevelNum++;   //更新玩家解锁的隐藏关卡数量
                GameManager.Instance.playerManager.unLockedNormalModelLevelList[monsterPetData.monsterID-1]++;  //更新玩家对应大关卡解锁关卡数量
                ShowMonster();
            }
            else
            {
                ShowMonster();
            }
           
        }
        SaveMonsterData(); //保存数据
    }

    //初始化宠物
    public void InitMonseterPet()
    {
        //如果
        if (monsterPetData.remainMilk == 0 && monsterPetData.remainCookies == 0)
        {
            monsterPetData.remainMilk = monsterPetData.monsterID * 60;
            monsterPetData.remainCookies = monsterPetData.monsterID * 30;
        }
     
        ShowMonster();
    }

    //保存宠物数据
    private void SaveMonsterData()
    {
        for (int i = 0; i < GameManager.Instance.playerManager.monsterPetDataList.Count; i++)
        {
            if(GameManager.Instance.playerManager.monsterPetDataList[i].monsterID == monsterPetData.monsterID)
            {
                GameManager.Instance.playerManager.monsterPetDataList[i] = monsterPetData;
            }
        }
    }
}

//宠物数据
public struct MonsterPetData
{
    public int monsterID;
    public int monsterLevel;     //等级
    public int remainCookies;    //升级剩余的饼干数
    public int remainMilk;       //升级剩余的牛奶数
}
