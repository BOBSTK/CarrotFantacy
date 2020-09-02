using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentLength : MonoBehaviour
{
    private float contentLength;
    private float sizex;
    private RectTransform contentTrans;
    private ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.name);
        scrollRect = transform.GetComponent<ScrollRect>();
        contentTrans = scrollRect.content;
        contentLength = contentTrans.rect.width;
        Debug.Log(contentLength);
        sizex = contentTrans.sizeDelta.x;
        Debug.Log(sizex);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            contentLength = contentTrans.rect.width;
            Debug.Log(contentLength);
            sizex = contentTrans.sizeDelta.x;
            Debug.Log(sizex);
        }
    }
}
