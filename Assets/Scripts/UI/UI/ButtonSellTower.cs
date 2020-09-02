using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 卖塔按钮
/// </summary>
public class ButtonSellTower : MonoBehaviour
{
    private int price;
    private Text text;
    private Button button;
    private GameController gameController;

    private void OnEnable()
    {
        if (text == null)
            return;
        price = gameController.selectGrid.towerSpecificProperty.sellPrice;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(SellTower);  //注册事件
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //卖塔方法
    private void SellTower()
    {
        gameController.selectGrid.towerSpecificProperty.SellTower();  //销毁塔，处理金币
        gameController.selectGrid.InitGrid();  //重置格子属性
        gameController.selectGrid.handleTowerCanvas.SetActive(false); //隐藏UI
#if Game
        gameController.selectGrid.HideGrid();  //隐藏格子
#endif
        gameController.selectGrid = null;    //处理重复点击同一个格子的问题
    }
}
