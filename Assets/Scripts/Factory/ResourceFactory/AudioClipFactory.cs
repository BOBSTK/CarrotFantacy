using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音频资源工厂
/// </summary>
public class AudioClipFactory : IBaseResourceFactory<AudioClip>
{
    //当前拥有的AudioClip类型的资源
    protected Dictionary<string, AudioClip> resourceDict = new Dictionary<string,AudioClip>();
    protected string loadPath;

    public AudioClipFactory()
    {
        loadPath += "AudioClips/";
    }
    public AudioClip GetSingleResource(string resourceName)
    {
        AudioClip itemAC = null;
        string path = loadPath + resourceName;
        if (resourceDict.ContainsKey(resourceName))
        {
            itemAC = resourceDict[resourceName];
        }
        else
        {
            itemAC = Resources.Load<AudioClip>(path);
            resourceDict.Add(resourceName, itemAC);
        }
        if(itemAC == null)
        {
            Debug.Log(resourceName+" 获取失败");
        }
        return itemAC;
    }
}
