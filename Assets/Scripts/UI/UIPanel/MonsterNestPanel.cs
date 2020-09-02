using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 怪物窝脚本
/// </summary>
public class MonsterNestPanel : BasePanel
{
    //引用
    private GameObject shopPanel;
    private Text text_Cookies;
    private Text text_Milk;
    private Text text_Nest;
    private Text text_Diamands;
    private List<GameObject> monsterPetList;  //管理玩家的宠物信息
    private Transform Emp_MonsterGroupTrans;  //用于排列宠物

    protected override void Awake()
    {
        base.Awake();
        //获取资源
        shopPanel = transform.Find("ShopPage").gameObject;
        text_Cookies = transform.Find("Img_TopPage").Find("Text_Cookies").GetComponent<Text>();
        text_Milk = transform.Find("Img_TopPage").Find("Text_Milk").GetComponent<Text>();
        text_Nest = transform.Find("Img_TopPage").Find("Text_Nest").GetComponent<Text>();
        text_Diamands = transform.Find("ShopPage").Find("Img_Diamonds").Find("Text_Diamonds").GetComponent<Text>();
        Emp_MonsterGroupTrans = transform.Find("Emp_MonsterGroup");
        //加载宠物资源
        for (int i = 1; i < 4; i++)
        {
            uiFacade.GetSprite("MonsterNest/Monster/Egg/" + i.ToString());      //egg
            uiFacade.GetSprite("MonsterNest/Monster/Baby/" + i.ToString());     //baby
            uiFacade.GetSprite("MonsterNest/Monster/Normal/" + i.ToString());   //normal
        }
        monsterPetList = new List<GameObject>();
    }

    public override void InitPanel()
    {
        base.InitPanel();
        //清空之前的数据
        for (int i = 0; i < monsterPetList.Count; i++)
        {
            uiFacade.PushGameObjectToFactory(FactoryType.UIFactory, "Emp_Monster", monsterPetList[i]);
        }
        monsterPetList.Clear();
        for (int i = 0; i < uiFacade.playerManager.monsterPetDataList.Count; i++)
        {
            if (uiFacade.playerManager.monsterPetDataList[i].monsterID != 0)
            {
                //初始化宠物
                GameObject monsterPet = uiFacade.GetGameObject(FactoryType.UIFactory, "Emp_Monster");
                monsterPet.GetComponent<MonsterPet>().monsterPetData = uiFacade.playerManager.monsterPetDataList[i];
                monsterPet.GetComponent<MonsterPet>().monsterNestPanel = this;
                monsterPet.GetComponent<MonsterPet>().InitMonseterPet();
                monsterPet.transform.SetParent(Emp_MonsterGroupTrans);
                monsterPet.transform.localScale = Vector3.one;
                monsterPetList.Add(monsterPet);
            }
        }
        UpdateText(); //更新文本


    }

    //顶部UI
    //------
    //打开商店页面
    public void ShowShopPanel()
    {
        uiFacade.PlayButtonAudioClip();
        shopPanel.SetActive(true);
    }
    //关闭商店页面
    public void CloseShopPanel()
    {
        uiFacade.PlayButtonAudioClip();
        shopPanel.SetActive(false);
    }
    //返回主界面
    public void ReturnToMain()
    {
        uiFacade.PlayButtonAudioClip();
        uiFacade.ChangeSceneState(new MainSceneState(uiFacade));
    }

    //更新文本
    public void UpdateText()
    {
        text_Cookies.text = GameManager.Instance.playerManager.cookies.ToString();
        text_Milk.text = GameManager.Instance.playerManager.milk.ToString();
        text_Nest.text = GameManager.Instance.playerManager.nest.ToString();
        text_Diamands.text = GameManager.Instance.playerManager.diamands.ToString();
    }

    //商店页面
    //--------
    //购买窝
    public void BuyNest()
    {
        UpdateText();
        if (GameManager.Instance.playerManager.diamands >= 60)
        {
            GameManager.Instance.playerManager.diamands -= 60;
            GameManager.Instance.playerManager.nest++;
        }
        
    }

    //购买牛奶
    public void BuyMilk()
    {
        UpdateText();
        if (GameManager.Instance.playerManager.diamands >= 1)
        {
            GameManager.Instance.playerManager.diamands -= 1;
            GameManager.Instance.playerManager.milk += 10;
        }
        
    }

    //购买饼干
    public void BuyCookie()
    {
        UpdateText();
        if (GameManager.Instance.playerManager.diamands >= 10)
        {
            GameManager.Instance.playerManager.diamands -= 10;
            GameManager.Instance.playerManager.cookies += 15;
        }
      
    }

    //设置父对象为游戏中唯一的Canvas
    public void SetCanvasTrans(Transform uiTrans)
    {
        uiTrans.SetParent(uiFacade.canvasTransform);
    }
}
