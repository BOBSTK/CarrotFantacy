using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPFactory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FactoryIPhone8 factoryIPhone8 = new FactoryIPhone8();
        factoryIPhone8.CreateIPhone();
        FactoryIPhoneX factoryIPhoneX = new FactoryIPhoneX();
        factoryIPhoneX.CreateIPhone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//简单工厂模式
//public class IPhone
//{
//    public IPhone()
//    {

//    }
//}

//public class IPhone8 : IPhone
//{
//    public IPhone8()
//    {

//    }
//}

//public class IPhoneX : IPhone
//{
//    public IPhoneX()
//    {

//    }
//}

//public interface IFactory
//{
//    IPhone CreateIPhone();
//}

//public class FactoryIPhone8 : IFactory
//{
//    public IPhone CreateIPhone()
//    {
//        return new IPhone8();
//    }
//}

//public class FactoryIPhoneX : IFactory
//{
//    public IPhone CreateIPhone()
//    {
//        return new IPhoneX();
//    }
//}

//抽象工厂模式

//IPhone
public interface IPhone
{
    
}

public class IPhone8 : IPhone
{
    public IPhone8()
    {

    }
}

public class IPhoneX : IPhone
{
    public IPhoneX()
    {

    }
}

//充电器
public interface IPhoneCharger
{

}

public class IPhone8Charger : IPhoneCharger
{
    public IPhone8Charger()
    {
        //return new IPhone8Charger();
    }
}

public class IPhoneXCharger : IPhoneCharger
{
    public IPhoneXCharger()
    {

    }
}

public interface IFactory
{
    IPhone CreateIPhone();
    IPhoneCharger CreatCharger();
}

public class FactoryIPhone8 : IFactory
{
    public IPhoneCharger CreatCharger()
    {
        return new IPhone8Charger();
    }

    public IPhone CreateIPhone()
    {
        return new IPhone8();
    }
}

public class FactoryIPhoneX : IFactory
{
    public IPhoneCharger CreatCharger()
    {
        return new IPhoneXCharger();
    }

    public IPhone CreateIPhone()
    {
        return new IPhoneX();
    }
}
