using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    #region sington
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion sington

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    private string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(name == effectSounds[i].name)
            {
                for(int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중입니다.");
                return;
            }
        }
        Debug.Log(name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for(int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if(playSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }
        Debug.Log("재생 중인 " + name + "사운드가 없습니다.");
    }


}
