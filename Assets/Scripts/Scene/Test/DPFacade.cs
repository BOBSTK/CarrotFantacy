using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPFacade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Principal principal = new Principal();
        principal.OrderToDoTask();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//校长
public class Principal
{
    private Teacher teacher = new Teacher();
    public void OrderToDoTask()
    {
        teacher.OrderStudentsToSummary();
    }
}

//老师 外观类
public class Teacher
{
    private Monitor monitor = new Monitor();
    private leageSecretary leage = new leageSecretary();
    public void OrderStudentsToSummary()
    {
        monitor.WritSummary();
        leage.WritSummary();
    }
}

public class Monitor
{
    public void WritSummary()
    {
        Debug.Log("班长汇报");
    }
}

public class leageSecretary
{
    public void WritSummary()
    {
        Debug.Log("团支书汇报");
    }
}
