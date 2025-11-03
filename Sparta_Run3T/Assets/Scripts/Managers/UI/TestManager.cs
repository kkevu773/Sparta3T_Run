using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private List<EffectKey> testKeys = new();

    void Start()
    {

        UIManager.Instance.UpdateScore(124);
        UIManager.Instance.UpdateBestScore(12512);
        UIManager.Instance.UpdateHP(30, 100);

        foreach (EffectKey key in System.Enum.GetValues(typeof(EffectKey)))
        {
            if (key != EffectKey.None)
                testKeys.Add(key);
        }

        StartCoroutine(TestEffects());
    }

    IEnumerator TestEffects()
    {
        foreach (EffectKey key in testKeys)
        {
            EffectManager.Instance.Play(key, Vector3.zero);
            Debug.Log($"[Effect Test] Played: {key}");
            yield return new WaitForSeconds(1.0f);
        }
    }
}
