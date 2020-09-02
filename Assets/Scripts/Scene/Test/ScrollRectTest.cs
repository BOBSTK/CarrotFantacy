using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectTest : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    
    private ScrollRect scrollRect;
    private RectTransform rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        //关于RectTransform的探究
        rectTransform = scrollRect.content;

        //当前UI的世界坐标
        Debug.Log("当前UI的世界坐标"+rectTransform.position);
        //当前UI的世界坐标
        Debug.Log("当前UI的局部坐标" + rectTransform.localPosition);
        //获取当前UI的宽度(从左到右
        Debug.Log("当前UI的宽度" + rectTransform.rect.width);
        //获取当前UI的高度(从左到右
        Debug.Log("当前UI的高度" + rectTransform.rect.height);

        Debug.Log(rectTransform.sizeDelta);
        Debug.Log(rectTransform.sizeDelta.x);
        //Debug.Log(rectTransform.sizeDelta.y);
        Debug.Log(rectTransform.rect.xMax);



        //宽度应该是想要增加的值
        //rectTransform.sizeDelta = new Vector2(226, 300);

        //水平滚动位置0到1的值
        scrollRect.horizontalNormalizedPosition = 1;
        scrollRect.onValueChanged.AddListener(PrintValue);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PrintValue(Vector2 vector)
    {
        Debug.Log("Listen" + vector);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("开始滑动");
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        Debug.Log("滑动中");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        Debug.Log("结束滑动");
    }
}
