using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 建塔按钮脚本 
/// </summary>
public class ButtonTower : MonoBehaviour
{
    public int towerID;   //由GameController在初始化的时候赋值
    public int price;
    private Button button;             
    private Sprite canClickSprite;   // 可以升级塔的图片
    private Sprite cantClicklSprite;  // 不可以升级塔的图片
    private Image iamge;

    private void Start()
    {
        iamge = GetComponent<Image>();
        button = GetComponent<Button>();
        canClickSprite = GameController.Instance.GetSprite("NormalModel/Game/Tower/"+towerID.ToString()+ "/CanClick1");
        cantClicklSprite = GameController.Instance.GetSprite("NormalModel/Game/Tower/" + towerID.ToString() + "/CanClick0");
        UpdateIcon();
        price = GameController.Instance.towerPriceDict[towerID];
        button.onClick.AddListener(BuildTower);
    }

    private void OnEnable()
    {
        if(price == 0) //只有从对象池中取出时才调用此方法
        {
            return;
        }
        UpdateIcon();
    }

    //建塔的方法
    private void BuildTower()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/Tower/TowerBulid");
        //由建造者建造新塔
        GameController.Instance.towerBuilder.towerID = towerID;
        GameController.Instance.towerBuilder.towerLevel = 1;
        GameObject towerGo = GameController.Instance.towerBuilder.GetProduct();//通过建造者获得塔对象
        //设置对象属性
        towerGo.transform.SetParent(GameController.Instance.selectGrid.transform);//将父对象设为当前选中的格子对象
        towerGo.transform.position = GameController.Instance.selectGrid.transform.position;
        //产生特效
        GameObject buildEffect = GameController.Instance.GetGameObject("BuildEffect");
        buildEffect.transform.SetParent(GameController.Instance.transform);
        buildEffect.transform.position = GameController.Instance.selectGrid.transform.position; //在建塔位置产生特效
                                                                                                //处理格子

        //完成创建之后的后续方法
#if Game
        GameController.Instance.selectGrid.AfterBuild();
        GameController.Instance.selectGrid.HideGrid(); //隐藏格子
#endif
        GameController.Instance.selectGrid.hasTower = true;
        GameController.Instance.ChangeCoin(-price); //更新玩家金币
  
        GameController.Instance.selectGrid = null;  //和GameController HandleGrid()方法有关 不置空在建塔后直接点击塔无法正常使用按钮
        GameController.Instance.handleTowerCanvas.SetActive(false); //通过切换显示与隐藏更新处理UI的图片(OnEnable)
        
    }

    //更新图标的方法
    private void UpdateIcon()
    {
        if(GameController.Instance.coin > price)  //能够购买
        {
            iamge.sprite = canClickSprite;
            button.interactable = true;
        }
        else                             //不可以购买
        {
            iamge.sprite = cantClicklSprite;
            button.interactable = false;
        }

    }
}


