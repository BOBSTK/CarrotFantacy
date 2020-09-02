using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadPanel : BasePanel
{   
    
    //只有在跳转到其他面板时显示
    public override void EnterPanel()    
    {   
        gameObject.SetActive(true);
        transform.SetSiblingIndex(8);
    }

    //实例化时不显示
    public override void InitPanel()
    {
        gameObject.SetActive(false);
    }
}
