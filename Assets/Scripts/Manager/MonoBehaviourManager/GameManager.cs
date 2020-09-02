using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏总管理，负责管理所有管理者
public class GameManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public FactoryManager factoryManager;
    public AudioSourceManager audioSourceManager;
    public UIManager uiManager;

    public Stage currentStage; //当前关卡的Stage对象，在GameNormalLevelPanel中加载对应关卡时赋值用于开始游戏场景
    public bool isResetData;     //是否重置游戏

    // Start is called before the first frame update

    private static GameManager _instance;

    public static GameManager Instance 
    {
        get 
        {
            return _instance;
        } 
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); //在加载新的场景时，不会销毁此object
        _instance = this;
        //实例化顺序
        playerManager = new PlayerManager();
        
        //playerManager.SaveData();
        isResetData = false;
        playerManager.ReadData();

        factoryManager = new FactoryManager();
        audioSourceManager = new AudioSourceManager();
        uiManager = new UIManager();  //实例化UIManager和UIFacade
        uiManager.uiFacade.currentSceneState.EnterScene(); //进入第一个场景
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("playBGMusic: " + audioSourceManager.playBGMusic);
        }
    }

    //创建一个对象
    public GameObject CreatItem(GameObject itemGO)
    {
        GameObject go = Instantiate(itemGO); //克隆对象
        return go;
    }

    //获取资源
    //--------
    //获取Sprite资源
    public Sprite GetSprite(string resourceName)
    {
        return factoryManager.spriteFactory.GetSingleResource(resourceName);
    }

    //获取AudioClip资源
    public AudioClip GetAudioClip(string resourceName)
    {
        return factoryManager.audioClipFactory.GetSingleResource(resourceName);
    }

    //获取RuntimeAnimatorController资源
    public RuntimeAnimatorController GetController(string resourceName)
    {
        return factoryManager.runtimeAnimatorControllerFactory.GetSingleResource(resourceName);
    }

    //获取游戏物体 对象池
    public GameObject GetGameObject(FactoryType type, string itemName)
    {
        return factoryManager.factoryDict[type].GetItem(itemName);
    }
    //放回游戏物体 对象池
    public void PushGameObjectToFactory(FactoryType type, string itemName,GameObject item)
    {
        factoryManager.factoryDict[type].PushItem(itemName, item);
    }
}
