using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 顶部UI脚本  应该在游戏场景中一直显示
/// </summary>
public class TopPage : MonoBehaviour
{
    //属性
    private bool isNormalSpeed;   //游戏速度开关
    private bool isPause;         //暂停开关

    //引用
    private Text text_Coin;
    private Text text_CurrentRound;
    private Text text_TotalRound;
    //只需要拿到Button的Image组件
    private Image img_Btn_GameSpeed;
    private Image img_Btn_Pause;

    //根据游戏是否暂停显示不同的信息
    private GameObject emp_Pause;
    private GameObject emp_Round;

    //按钮图片切换资源
    public Sprite[] btn_GameSpeedSprites;   //0 - 正常  1 - 2倍速
    public Sprite[] btn_PauseSprites;       //0 - 正常  1 - 暂停 

    private NormalModelPanel normalModelPanel;

    private void OnEnable()
    {
        //更新文本
        text_Coin.text = GameController.Instance.coin.ToString();
        text_TotalRound.text = normalModelPanel.totalRound.ToString();

        img_Btn_GameSpeed.sprite = btn_GameSpeedSprites[0];
        img_Btn_Pause.sprite = btn_PauseSprites[0];

        isNormalSpeed = true;
        isPause = false;

        emp_Pause.SetActive(false);
        emp_Round.SetActive(true);
    }

    private void Awake()
    {
        //获取资源
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        text_Coin = transform.Find("Text_Coin").GetComponent<Text>();
        text_CurrentRound = transform.Find("EMP_Round").Find("Text_Current").GetComponent<Text>();
        text_TotalRound = transform.Find("EMP_Round").Find("Text_Total").GetComponent<Text>();
        img_Btn_GameSpeed = transform.Find("Btn_GameSpeed").GetComponent<Image>();
        img_Btn_Pause = transform.Find("Btn_Pause").GetComponent<Image>();
        emp_Pause = transform.Find("EMP_Pause").gameObject;
        emp_Round = transform.Find("EMP_Round").gameObject;

    }

    //更新金币
    public void UpdateCoinText()
    {
        text_Coin.text = GameController.Instance.coin.ToString();
    }

    //更新波次数
    public void UpdateRoundText()
    {
        normalModelPanel.ShowRoundInfo(text_CurrentRound);
    }

    //改变游戏速度
    public void ChangeGameSpeed()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isNormalSpeed = !isNormalSpeed; //切换速度状态
        if (isNormalSpeed) //1倍速
        {
            GameController.Instance.gameSpeed = 1;
            img_Btn_GameSpeed.sprite = btn_GameSpeedSprites[0];
        }
        else
        {
            GameController.Instance.gameSpeed = 2;
            img_Btn_GameSpeed.sprite = btn_GameSpeedSprites[1];
        }
    }

    //游戏暂停
    public void PauseGame()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isPause = !isPause;   //切换游戏暂停状态
        if (isPause)  //暂停
        {
            GameController.Instance.isPause = true;
            img_Btn_Pause.sprite = btn_PauseSprites[1];
            emp_Pause.SetActive(true);
            emp_Round.SetActive(false);
        }
        else
        {
            GameController.Instance.isPause = false;
            img_Btn_Pause.sprite = btn_PauseSprites[0];
            emp_Pause.SetActive(false);
            emp_Round.SetActive(true);
        }
    }

    //显示游戏菜单
    public void ShowMenuPage()
    {
        normalModelPanel.ShowMenuPage();
    }
    

}
