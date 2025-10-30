using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Pair 리스트")]
    public List<UIPair> uiPairs = new();
    private Dictionary<UIKey, GameObject> uiDic = new();

    [Header("HUD 요소")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public Slider hpBar;

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

        foreach (var pair in uiPairs)
        {
            if (pair.uiObject == null)
            {
                continue;
            }
            if (!uiDic.ContainsKey(pair.key))
            {
                uiDic.Add(pair.key, pair.uiObject);
            }
        }
    }

    public void ShowUI(UIKey key, bool show)
    {
        if (key == UIKey.None)
        {
            return;
        }
        if (uiDic.TryGetValue(key, out var go) && go != null)
        {
            go.SetActive(show);
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
    public void UpdateBestScore(int best)
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = $"Best Score: {best}";
        }
    }

    public void UpdateHP(int current, int max)
    {
        if (hpBar != null && max > 0)
        {
            hpBar.value = (float)current / max;
        }
    }
}
  