using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;



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

    [Header("설정창")]
    public GameObject panelSet;
    public Button btnSet;
    public Button btnTit;
    public Button btnBack;

    private bool paused = false;
    public bool IsPaused => paused;

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

    private void Start()
    {
        if (btnSet != null) btnSet.onClick.AddListener(ToggleSet);
        if (btnBack != null) btnBack.onClick.AddListener(CloseSet);
        if (btnTit != null) btnTit.onClick.AddListener(GoTitle);
        if (panelSet != null)
        {
            panelSet.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    private void ToggleSet()
    {
        paused = !paused;
        if (panelSet != null)
        {
            panelSet.SetActive(paused);
            Time.timeScale = paused ? 0f : 1f;
        }
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
    private void CloseSet()
    {
        paused = false;
        if (panelSet != null)
        {
            panelSet.SetActive(false);
            Time.timeScale = 1f;
        }
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void GoTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
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
            bestScoreText.text = $"Best: {best}";
        }
    }

    public void UpdateHP(int current, int max)
    {
        if (hpBar == null || max <= 0)
        {
            return;
        }

        hpBar.value = (float)current / max;
    }
}
  