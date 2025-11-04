using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundInstace : MonoBehaviour
{
    [Header("설정")]
    public float delay = 0f;

    [Range(0f, 1f)] public float volume = 1f;
    public float pitch = 1f;
    public bool loop = false;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        ApplySet();
    }

    private void ApplySet()
    {
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
    }

    public void PlayWithSettings()
    {
        ApplySet();

        if (delay > 0f)
        {
            source.PlayDelayed(delay);
        }
        else
        {
            source.Play();
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
