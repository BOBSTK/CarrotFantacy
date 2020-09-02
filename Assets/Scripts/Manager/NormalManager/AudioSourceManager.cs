using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责控制音乐和游戏中各种音效的播放
/// </summary>
public class AudioSourceManager
{
    private AudioSource[] audioSources;  //0.播放背景音乐 1.播放音效
    private bool playEffectMusic = true;
    public bool playBGMusic = true;

    public AudioSourceManager()
    {
        audioSources = GameManager.Instance.GetComponents<AudioSource>(); //获取GameManger上的音效组件

    }

    //播放背景音乐
    public void PlayBGMusic(AudioClip audioClip)
    {
        //Debug.Log("PlayBGMusic");
        //如果没有背景音乐或传入的背景音乐资源与正在播放的音频不同时播放背景音乐
        if (!audioSources[0].isPlaying || audioSources[0].clip != audioClip) 
        {
            audioSources[0].clip = audioClip;
            audioSources[0].Play();
        }
    }

    //播放音效
    public void PlayEffectMusic(AudioClip audioClip)
    {
        //Debug.Log(playEffectMusic);
        if (playEffectMusic)
        {
            audioSources[1].PlayOneShot(audioClip); //可以在一个 AudioSource 上播放多个声音
        }
    }

    //关闭背景音乐
    public void CloseBGMusic()
    {
        audioSources[0].Stop();
    }

    //打开背景音乐
    public void OpenBGMusic()
    {
        if (playBGMusic) {
            audioSources[0].Play();//播放新的音频剪辑
        }
    }

    public void CloseOrOpenBGAudio()
    {
      //  Debug.Log("CloseOrOpenBGAudio");
        playBGMusic = !playBGMusic;
        if(playBGMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }

    public void CloseOrOpenEffectAudio()
    {
        playEffectMusic = !playEffectMusic; //PlayEffectMusic()根据该bool值决定是否播放音效
    }

    
    /// <summary>
    /// UI音效
    /// </summary>
     
    //Button音效
    public void PlayButtonAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResource("Main/Button"));
    }
    //翻页音效
    public void PlayPagingAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResource("Main/Paging"));
    }

}
