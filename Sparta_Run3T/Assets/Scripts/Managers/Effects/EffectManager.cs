using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    public static EffectManager Instance {  get; private set; }

    [Header("이펙트")]
    public List<EffectPair> effectPairs = new();
    private Dictionary<EffectKey, GameObject> effectDic = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var pair in effectPairs)
        {
            if (pair.prefab != null && !effectDic.ContainsKey(pair.key))
            {
                effectDic.Add(pair.key, pair.prefab);
            }
        }
    }
    public void Play(EffectKey key, Vector3 pos)
    {
        if (key == EffectKey.None)
        {
            return;
        }
        if (!effectDic.TryGetValue(key, out GameObject prefab))
        {
            return;
        }
        GameObject effect = Instantiate(prefab, pos, Quaternion.identity);
    }

    public void PlayUI(EffectKey key, Transform parent)
    {
        if (!effectDic.TryGetValue(key, out GameObject prefab))
        {
            return;
        }
        GameObject uiFx = Instantiate(prefab, parent);
        uiFx.transform.localPosition = Vector3.zero;
    }

    public void PlayLoop(EffectKey key, Vector3 pos, out GameObject instance)
    {
        if (!effectDic.TryGetValue(key, out GameObject prefab))
        {
            instance = null;
            return;
        }
        instance = Instantiate(prefab, pos, Quaternion.identity);
    }

    public void Stop(GameObject instance)
    {
        if(instance != null)
        {
            instance.SetActive(false);
        }
    }
}
