using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DOTweenTest : MonoBehaviour
{
    private Image maskImage;
    private Tween maskTween;
    // Start is called before the first frame update
    void Start()
    {
        maskImage = GetComponent<Image>();
        //1 DOTween的静态方法
        //DOTween.To(() =>                         //lambda表达式
        //    maskImage.color,                     //想改变的对象值  
        //    toColor                              //当前值到目标值得插值
        //    => maskImage.color=toColor           //将计算结果赋值给想要改变的对象
        //    , new Color(0, 0, 0, 0), 2f);        //目标值，时间

        //2.DOTween直接作用于transform的方法
        //Tween tween = transform.DOLocalMoveX(300, 1f,true);
        //tween.PlayForward();
        //tween.PlayBackwards();

        //3.循环播放
        //maskTween = transform.DOLocalMoveX(300, 1f, true);//移动
        //maskTween.SetAutoKill(false);//保存动画
        //maskTween.Pause();//暂停

        //4.缓动函数和循环播放
        maskTween = transform.DOLocalMoveX(300, 5f, true);//移动
        maskTween.SetEase(Ease.Linear);
        maskTween.SetLoops(3, LoopType.Yoyo);
        //maskTween.OnComplete(CompleteMethod);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //maskTween.Play();
            maskTween.PlayForward();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            maskTween.PlayBackwards();
        }
    }

    private void CompleteMethod()
    {
        DOTween.To(() =>                         
            maskImage.color,                       
            toColor                              
            => maskImage.color = toColor         
            , new Color(0, 0, 0, 0), 2f);        
    }
}
