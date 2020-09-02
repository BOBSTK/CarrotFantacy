using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 工厂管理，负责管理各种类型的工厂以及对象池
/// </summary>

public class FactoryManager
{
    public Dictionary<FactoryType, IBaseFactory> factoryDict = new Dictionary<FactoryType, IBaseFactory>(); //工厂字典，存所有的对象工厂
    public AudioClipFactory audioClipFactory;
    public SpriteFactory spriteFactory;
    public RuntimeAnimatorControllerFactory runtimeAnimatorControllerFactory;

    public FactoryManager()
    {
        factoryDict.Add(FactoryType.UIFactory, new UIFactory());
        factoryDict.Add(FactoryType.UIPanelFactory, new UIPanelFactory());
        factoryDict.Add(FactoryType.GameFactory, new GameFactory());
        audioClipFactory = new AudioClipFactory();
        spriteFactory = new SpriteFactory();
        runtimeAnimatorControllerFactory = new RuntimeAnimatorControllerFactory();
    }
}
