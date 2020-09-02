using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 萝卜脚本
/// </summary>
public class Carrot : MonoBehaviour
{
    //属性
    private float timeVal;          //idle动画计时器 

    //引用
    private Sprite[] sprites;
    private Animator animator;
    private SpriteRenderer sr;
    private Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        sprites = new Sprite[7];
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = GameController.Instance.GetSprite("NormalModel/Game/Carrot/"+i.ToString());
        }
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hpText = transform.Find("HpCanvas").Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.carrotHP < 10)
            animator.enabled = false;
        if(timeVal >= 20)
        {
            //Debug.Log("play idle");
            animator.Play("Idle");
            timeVal = 0;
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        if(GameController.Instance.carrotHP >= 10)
        {
            //满血时点击萝卜动画
            animator.Play("Touch");
            //音效处理
            int random = Random.Range(1, 4);
            GameController.Instance.PlayEffectMusic("NormalModel/Carrot/"+random.ToString());
        }
    }

    public void UpdateCarrotUI()
    {
        int hp = GameController.Instance.carrotHP;
        hpText.text = hp.ToString();
        if(hp >= 7 && hp <10)
        {
            sr.sprite = sprites[6];
        }
        else if(hp >0 && hp < 7)
        {
            sr.sprite = sprites[hp - 1];
        }
        else  //游戏结束
        {
            GameController.Instance.isGameOver = true;
            GameController.Instance.normalModelPanel.ShowGameOverPage();   
        }
    }
}
