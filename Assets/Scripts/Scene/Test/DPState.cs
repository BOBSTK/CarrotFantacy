using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPState : MonoBehaviour
{
    //public enum PersonState
    //{
    //    Eat,
    //    Work,
    //    Sleep
    //}

    //public PersonState personState;

 
    // Start is called before the first frame update
    void Start()
    {
        //personState = PersonState.Work;
        //if(personState == PersonState.Work)
        //{
        //    Debug.Log("正在工作");
        //}
        //else if(personState == PersonState.Eat)
        //{
        //    Debug.Log("正在吃饭");
        //}
        //else if (personState == PersonState.Sleep)
        //{
        //    Debug.Log("正在睡觉");
        //}
        Context context = new Context();
        context.SetState(new Eat(context));
        context.Handle();
        context.SetState(new Sleep(context));
        context.Handle();

    }

}
public interface iState
{
    void Handle();
}

public class Context //状态机，上下文
{
    private iState mState;  //当前状态
    public void SetState(iState state)
    {
        mState = state;
    }

    public void Handle()
    {
        mState.Handle(); //当前状态下要执行的方法
    }
}

public class Eat : iState
{
    private Context mContext;

    public Eat(Context context)
    {
        mContext = context;
    }
    public void Handle()
    {
        Debug.Log("正在吃饭");
    }
  
}

public class Work : iState
{
    private Context mContext;

    public Work(Context context)
    {
        mContext = context;
    }
    public void Handle()
    {
        Debug.Log("正在工作");
    }

}

public class Sleep : iState
{
    private Context mContext;

    public Sleep(Context context)
    {
        mContext = context;
    }
    public void Handle()
    {
        Debug.Log("正在睡觉");
    }

}
