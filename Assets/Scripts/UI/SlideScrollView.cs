using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlideScrollView : MonoBehaviour, IBeginDragHandler,IEndDragHandler

{
    private ScrollRect scrollRect;
    private RectTransform contentTrans;
    
    private float beginMousePositionX;  //鼠标开始位置
    private float endMousePositionX;    //鼠标结束位置

    public int cellLength;    //单元格长度
    public int spacing;       //间隔
    public int leftOffset;    //左偏移
    public float trigger;     //翻页触发值

    private float moveOneItem;    //滑动一个单元格需要的距离
    public int totalItemNum;
    private int currentIndex; //页码

    private Vector3 currentContentLocalPos;  //当前位置局部坐标
    private Vector3 contentInitPos;    //Content初始坐标
    private Vector2 contentTransSize;  //Content初始大小

    public Text pageText;  //显示页码
    public bool needSendMessage; //是否需要传递翻页的消息
  
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentTrans = scrollRect.content;
        currentContentLocalPos = contentTrans.localPosition;
        contentInitPos = contentTrans.localPosition; //记录Content初始坐标
        contentTransSize = contentTrans.sizeDelta;  //记录Content初始大小
        currentIndex = 1;
        moveOneItem = cellLength + spacing;
        if(pageText!=null)
           pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString(); //初始化页码
    }

    /// <summary>
    /// 滑动翻页
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePositionX = Input.mousePosition.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        endMousePositionX = Input.mousePosition.x;
        float xOffset = beginMousePositionX - endMousePositionX;
        float moveDistance = 0; //需要滑动的距离
        //Debug.Log(xOffset);
        if(Mathf.Abs(xOffset) < trigger)   //如果滑动距离太小，回到原页面
        {
            DOTween.To(() => contentTrans.localPosition,
             lerpValue => contentTrans.localPosition = lerpValue,
             currentContentLocalPos,
             0.7f
            ).SetEase(Ease.OutQuint);
            return;
        }
        else
        {
            if (xOffset > 0)  //右滑
            {
                if (currentIndex >= totalItemNum)
                    return;
                if (needSendMessage)
                {
                    UpdatePanel(true);  //传递消息
                }
                moveDistance = -moveOneItem;
                currentIndex++;
            }
            else  //左滑
            {
                if (currentIndex <= 1)
                    return;
                if (needSendMessage)
                {
                    UpdatePanel(false);  //传递消息
                }
                moveDistance = moveOneItem;
                currentIndex--;
            }
        }
        if (pageText != null)
            pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString(); //更新页码

        DOTween.To(() => contentTrans.localPosition,
             lerpValue => contentTrans.localPosition = lerpValue,
             currentContentLocalPos + new Vector3(moveDistance, 0, 0),
             0.7f
            ).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
        //播放翻页音效
        GameManager.Instance.audioSourceManager.PlayPagingAudioClip();
    }

    /// <summary>
    /// 用按钮控制翻书效果
    /// </summary>
    //前往下一页
    public void ToNextPage()
    {
        float moveDistance = 0;
        if(currentIndex >= totalItemNum)//大于最大页数
        {
            return;    
        }
        moveDistance = -moveOneItem; //向右滑动一页
        ++currentIndex;
        if (pageText != null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString(); //更新页码
        }
        if (needSendMessage)
        {
            UpdatePanel(true);  //传递消息
        }
        DOTween.To(() => contentTrans.localPosition,
             lerpValue => contentTrans.localPosition = lerpValue,
             currentContentLocalPos + new Vector3(moveDistance, 0, 0),
             0.7f
            ).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    //前往上一页
    public void ToLastPage()
    {
        float moveDistance = 0;
        if (currentIndex <= 1)//当前在第一页
        {
            return;
        }
        moveDistance = moveOneItem; //向左滑动一页
        --currentIndex;
        if (pageText != null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString(); //更新页码
        }
        if (needSendMessage)
        {
            UpdatePanel(false);  //传递消息
        }
        DOTween.To(() => contentTrans.localPosition,
             lerpValue => contentTrans.localPosition = lerpValue,
             currentContentLocalPos + new Vector3(moveDistance, 0, 0),
             0.7f
            ).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    //切换页面时应该初始化
    public void Init()
    {
        currentIndex = 1;  
        if(contentTrans != null)   //防止报空
        {
            contentTrans.localPosition = contentInitPos;
            currentContentLocalPos = contentInitPos;
            if (pageText != null)
              pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString(); //更新页码
        }
    }

    //设置Content大小
    public void SetContentLength(int itemNum)
    {
        contentTrans.sizeDelta = new Vector2(contentTrans.sizeDelta.x + (cellLength + spacing)*(itemNum-1), contentTrans.sizeDelta.y);
        totalItemNum = itemNum;
    }

    //初始化Content的大小
    public void InitScrollLength()
    {
        contentTrans.sizeDelta = contentTransSize;
    }

    //发送翻页信息的方法
    public void UpdatePanel(bool toNext)
    {
        if (toNext)
        {
            //调用此游戏对象中的每个 MonoBehaviour 上或此行为的每个父级上名为 methodName 的方法
            gameObject.SendMessageUpwards("ToNextLevel");
        }
        else
        {
            gameObject.SendMessageUpwards("ToLastLevel");
        }
    }
 }
