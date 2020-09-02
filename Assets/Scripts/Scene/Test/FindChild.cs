using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindChild : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.name);
        Transform trans = transform.Find("Img_Curtain").Find("Img_Monster");
        Debug.Log(trans.name);
        Image img = trans.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Pictures/MonsterNest/Monster/Baby/2");
    }

   
}
