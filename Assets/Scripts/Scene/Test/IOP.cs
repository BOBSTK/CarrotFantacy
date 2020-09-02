using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOP : MonoBehaviour
{

    private void Start()
    {
        //IHero myHero = new Gangplank();
        BaseHero myHero = new Graves();
        myHero.SkillE();
        myHero.SkillE();
        myHero.SkillQ();
        myHero.SkillE();
        myHero.SkillR();
        myHero.hp = 10;
        
    }
}

public interface IHero
{
    void SkillQ();
    void SkillW();
    void SkillE();
    void SkillR();
}

public class BaseHero : IHero
{
    public int hp;
    public int mp;
    public virtual void SkillE()
    {
        Debug.Log("E技能");
    }

    public virtual void SkillQ()
    {
        Debug.Log("Q技能");
    }

    public virtual void SkillR()
    {
        Debug.Log("R技能");
    }

    public virtual void SkillW()
    {
        Debug.Log("W技能");
    }
}
public class Gangplank : BaseHero
{
    public void SkillE()
    {
        Debug.Log("火药桶");
    }

    public void SkillQ()
    {
        Debug.Log("枪火谈判");
    }

    public void SkillR()
    {
        Debug.Log("加农炮幕");
    }

    public void SkillW()
    {
        Debug.Log("坏血病疗法");
    }
}

public class Graves : BaseHero
{
    public override void SkillE()
    {
        base.SkillE();
        Debug.Log("猛龙过江");
    }

    public override void SkillQ()
    {
        base.SkillQ();
        Debug.Log("人在江湖");
    }

    public override void SkillR()
    {
        base.SkillR();
        Debug.Log("龙争虎斗");
    }

    public override void SkillW()
    {
        base.SkillW();
        Debug.Log("只手遮天");
    }
}