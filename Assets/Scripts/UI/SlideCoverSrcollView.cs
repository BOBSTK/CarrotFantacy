using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SlideCoverSrcollView : MonoBehaviour,IBeginDragHandler,IEndDragHandler

{
    private ScrollRect scrollRect;
    private RectTransform contentRect;
    private float contentLength;
    private float beginMousePositionX;  //鼠标开始位置
    private float endMousePositionX;    //鼠标结束位置
    private float lastProportion;       //上一个位置的比例

    public int cellLength;    //单元格长度
    public int spacing;       //间隔
    public int leftOffset;    //左偏移

    private float upperLimit;    //滑动上限
    private float lowerLimit;    //滑动下限
   // public float trigger;     //翻页触发值

    private float firstItemMove;  //移动第一个单元格的距离 
    private float oneItemMove;    //滑动一个单元格需要的距离
    private float oneItemProportion; //滑动一个单元格所占比例

    public int totalItemNum;  //总页数
    private int currentIndex; //页码

    public Text pageText;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentRect = scrollRect.content;
        contentLength = contentRect.rect.width - 2 * leftOffset - cellLength; //计算比例的实际长度

        firstItemMove = cellLength / 2 + leftOffset;       //滑动第一页玩家需要滑动的距离
        oneItemMove = cellLength + spacing;                //之后的每一个是单元格长度+间隔
        oneItemProportion = oneItemMove / contentLength;   //滑动一页的距离比例

        lowerLimit = firstItemMove / contentLength;
        upperLimit = 1 - lowerLimit;

        currentIndex = 1;
        scrollRect.horizontalNormalizedPosition = 0;//水平滚动位置0到1的值

        if (pageText != null)
            pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString(); //初始化页码
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        beginMousePositionX = Input.mousePosition.x;
       // Debug.Log(beginMousePositionX);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float xOffset = 0;
        endMousePositionX = Input.mousePosition.x;
        xOffset = (beginMousePositionX - endMousePositionX)*2;   //Input.x获得的是视口坐标，UI是世界坐标，存在一个转换，为了简便可以乘2来比较
        if (Mathf.Abs(xOffset) < firstItemMove)   //如果滑动距离太小，回到原页面
        {
            DOTween.To(() => scrollRect.horizontalNormalizedPosition,
               lerpValue => scrollRect.horizontalNormalizedPosition = lerpValue,
               lastProportion,
               0.5f
               ).SetEase(Ease.InOutQuint);
         
        }else
        {
            int moveCount = 0;
            if (xOffset > 0) //右滑
            {
                if (currentIndex >= totalItemNum)
                {
                    return;
                }
                moveCount = (int)((xOffset - firstItemMove) / oneItemMove) + 1;  //移动的页数
                currentIndex += moveCount;
                if (currentIndex > totalItemNum)
                    currentIndex = totalItemNum;

                lastProportion += oneItemProportion * moveCount; //此次需要滑动的比例
                if (lastProportion >= upperLimit)
                {
                    lastProportion = 1;
                }
            }
            else             //左滑
            {
                if (currentIndex <= 1)
                {
                    return;
                }
                moveCount = (int)((xOffset + firstItemMove) / oneItemMove) - 1;  //移动的页数(负数)
                currentIndex += moveCount;
                if (currentIndex <= 1)
                    currentIndex = 1;
                lastProportion += oneItemProportion * moveCount;
                if (lastProportion <= lowerLimit)
                {
                    lastProportion = 0;
                }
            }

            if (pageText != null)
                pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString(); //更新页码

            DOTween.To(() => scrollRect.horizontalNormalizedPosition,
                lerpValue => scrollRect.horizontalNormalizedPosition = lerpValue,
                lastProportion,
                0.5f
                ).SetEase(Ease.InOutQuint);
            //播放翻页音效
            GameManager.Instance.audioSourceManager.PlayPagingAudioClip();
        }
        // Debug.Log("结束拖拽");
    }
    //切换页面时应该从第一页开始
    public void Init()
    {
        currentIndex = 1;
        lastProportion = 0;
        if (scrollRect != null)  //scrollRect不为空说明Awake方法已经调用过了
        {
            scrollRect.horizontalNormalizedPosition = 0;
            pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString(); //更新页码
        }
    }
}
