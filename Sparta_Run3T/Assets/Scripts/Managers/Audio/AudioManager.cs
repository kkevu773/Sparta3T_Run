using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct SoundPair
{
    public SoundKey key;
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("사운드 키 목록")]
    public SoundPair[] soundPairs;

    private Dictionary<SoundKey, AudioSource> soundDic = new();
    private AudioSource currentBGM;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (var pair in soundPairs)
        {
            if (pair.source != null && !soundDic.ContainsKey(pair.key))
            {
                soundDic.Add(pair.key, pair.source);
            }
        }
    }

    public void Play(SoundKey key)
    {
        if (key == SoundKey.None)
        {
            return;
        }

        if (soundDic.TryGetValue(key, out AudioSource src))
        {
            src.PlayOneShot(src.clip, src.volume);
        }
    }

    public void PlayOneShot(SoundKey key)
    {
        if(soundDic.TryGetValue(key, out AudioSource src))
        {
            float volume = src.volume;
            src.PlayOneShot(src.clip, volume);
        }
    }

    public void PlayBGM(SoundKey key)
    {
        StopAllBGM();

        if (soundDic.TryGetValue(key, out AudioSource src))
        {
            src.loop = true;
            src.Play();
            currentBGM = src;
        }

    }

    public void StopAllBGM()
    {
        foreach (var pair in soundDic)
        {
            if (pair.Key.ToString().StartsWith("BGM") && pair.Value.isPlaying)
            {
                pair.Value.Stop();
            }
        }
    }

    public void Stop(SoundKey key)
    {
        if (soundDic.TryGetValue(key, out AudioSource src) && src.isPlaying)
        {
            src.Stop();
        }
    }

    public void SetVolumeAll(float volume)
    {
        volume = Mathf.Clamp01(volume);
        foreach (var pair in soundDic)
        {
            pair.Value.volume = volume;
        }
    }
    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        foreach (var pair in soundDic)
        {
            if (pair.Key.ToString().StartsWith("BGM"))
            {
                pair.Value.volume = volume;
            }
        }
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        foreach (var pair in soundDic)
        {
            string keyName = pair.Key.ToString();

            if (!keyName.StartsWith("BGM"))
            {
                pair.Value.volume = volume;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
