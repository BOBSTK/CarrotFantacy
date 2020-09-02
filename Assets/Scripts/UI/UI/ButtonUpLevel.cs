using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 塔升级按钮的脚本
/// </summary>
public class ButtonUpLevel : MonoBehaviour
{
    //属性
    private int price;   //升级所需的金币
    

    //引用
    private Button button;
    private Text text;  //显示升级所需金币的脚本
    private Image image;  //按钮图片
    private Sprite canUpLevelSprite;   
    private Sprite cantUpLevelSprite;
    private Sprite reachHighestLevel;
    private GameController gameController;

    //每次激活的时候更新UI
    private void OnEnable()
    {
        if(text == null) //安全判定，第一次必须先调用Start方法为成员变量赋初始值
        {
            return;
        }
        UpdateUI();
    }

    // Start is called before the first frame update
    void Start()
    {
#if Game
        gameController = GameController.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(UpLevelTower);  //注册升级方法
        //获取资源
        canUpLevelSprite = gameController.GetSprite("NormalModel/Game/Tower/Btn_CanUpLevel");
        cantUpLevelSprite = gameController.GetSprite("NormalModel/Game/Tower/Btn_CantUpLevel");
        reachHighestLevel = gameController.GetSprite("NormalModel/Game/Tower/Btn_ReachHighestLevel");
        text = transform.Find("Text").GetComponent<Text>();
        image = GetComponent<Image>();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //升级塔
    private void UpLevelTower()
    {
        //Debug.Log("UpLevel");
        //赋值建造要产生的塔的属性
        gameController.towerBuilder.towerID = gameController.selectGrid.tower.towerID;
        gameController.towerBuilder.towerLevel = gameController.selectGrid.towerLevel+1;

        //销毁之前的塔
        gameController.selectGrid.towerSpecificProperty.UpLevelTower();
        //产生新塔
        GameObject towerGo = gameController.towerBuilder.GetProduct();
        towerGo.transform.SetParent(gameController.selectGrid.transform);//将父对象设为当前选中的格子对象
        towerGo.transform.position = gameController.selectGrid.transform.position;
#if Game
        gameController.selectGrid.AfterBuild(); //更新格子上塔的相关信息
        gameController.selectGrid.HideGrid();   //隐藏塔处理方法的UI
#endif
        gameController.selectGrid = null;       //处理重复点击同一个格子的问题
        //后续处理
    }
    //更新塔升级按钮的UI
    private void UpdateUI()
    {
        if(gameController.selectGrid.towerLevel >= 3) //塔已经是最高等级(3)了
        {
            image.sprite = reachHighestLevel;  //切换按钮图片
            button.interactable = false;       //按钮设为不可交互
            text.enabled = false;              //隐藏文本
        }
        else
        {
            text.enabled = true;
            price = gameController.selectGrid.towerSpecificProperty.upLevelPrice;
            text.text = price.ToString();
            if (gameController.coin >= price)  //金币足够
            {
                image.sprite = canUpLevelSprite;
                button.interactable = true;
            }
            else                              //金币不够
            {
                image.sprite = cantUpLevelSprite;
                button.interactable = false;
            }
        }
    }
}
