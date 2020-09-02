using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 获得金币的脚本
/// </summary>
public class CoinMove : MonoBehaviour
{
    private Text coinText;    //金币数量文本
    private Image coinImage;  //获得金币的图片
    [HideInInspector]
    public Sprite[] coinSprites;
    [HideInInspector]
    public int prize;         //金币数量

    private void Awake()
    {
      
      coinText = transform.Find("Tex_Coin").GetComponent<Text>();
      coinImage = transform.Find("Img_Coin").GetComponent<Image>();
      
      coinSprites = new Sprite[2];    
      coinSprites[0] = GameController.Instance.GetSprite("NormalModel/Game/Coin"); //金币图片（少量）
      coinSprites[1] = GameController.Instance.GetSprite("NormalModel/Game/ManyCoin");//金币图片（大量）
     // ShowCoin();
    }

    private void OnEnable()
    {
        //Debug.Log("OnEnable coin" + prize);
        ShowCoin(); //每次激活时调用显示金币的方法
        
    }

    //显示金币UI
    private void ShowCoin()
    {
        //Debug.Log("ShowCoin: " + prize);
        
            //Debug.Log("ShowCoin" + prize);
            coinText.text = prize.ToString();
            //图片显示
            if (prize >= 500)
            {
                coinImage.sprite = coinSprites[1];
            }
            else
            {
                coinImage.sprite = coinSprites[0];
            }
            //金币上升动画
            transform.DOLocalMoveY(60, 1.0f);
            //金币图片消失动画
            DOTween.To(() => coinImage.color,
                toColor => coinImage.color = toColor,
                new Color(1, 1, 1, 0), 1.0f);
            //文字消失动画
            Tween tween = DOTween.To(() => coinText.color,
                toColor => coinText.color = toColor,
                new Color(1, 1, 1, 0), 1.0f
                );
            tween.OnComplete(DestroyCoin); //回调函数
        
    }

    //销毁金币UI
    private void DestroyCoin()
    {
        //将金币UI恢复成默认状态
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        coinImage.color = coinText.color = new Color(1, 1, 1); //重置图片颜色初始值
        GameController.Instance.PushGameObjectToFactory("CoinCanvas", transform.parent.gameObject); //放入对象池
    }

}
