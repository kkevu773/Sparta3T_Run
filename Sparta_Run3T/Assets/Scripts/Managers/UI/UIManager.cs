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

    [Header("슬라이더")]
    public Slider sliderBGM;
    public Slider sliderSFX;

    [Header("결과창")]
    public GameObject panelGameOver;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textBestScore;
    public TextMeshProUGUI textNewRecord;
    public Button btnRetry;
    public Button btnTitle;

    private int currentScore;
    private int bestScore;

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

    private IEnumerator Start()
    {
        yield return null;

        if (btnSet != null)
        {
            btnSet.onClick.AddListener(() =>
            {
                AudioManager.Instance.Play(SoundKey.SFX_UI_UICLICK);
                ToggleSet();
            });
        }
        if (btnBack != null)
        {
            btnBack.onClick.AddListener(() =>
            {
                AudioManager.Instance.Play(SoundKey.SFX_UI_UICLICK);
                CloseSet();
            });
        }
        if (btnTit != null)
        {
            btnTit.onClick.AddListener(() =>
            {
                AudioManager.Instance.Play(SoundKey.SFX_UI_UICLICK);
                GoTitle();
            });
        }
        if (panelSet != null)
        {
            panelSet.SetActive(false);
        }
        if (sliderBGM != null)
        {
            sliderBGM.value = 1f;
            sliderBGM.onValueChanged.AddListener(BGMChange);
        }
        if (sliderSFX != null)
        {
            sliderSFX.value = 1f;
            sliderSFX.onValueChanged.AddListener(SFXChange);
        }
        Time.timeScale = 1f;
    }

    public void ShowGameOver(int score)
    {
        if (panelGameOver == null)
        {
            return;
        }

        panelGameOver.SetActive(true);
        Time.timeScale = 0f;

        currentScore = score;
        textScore.text = $"Score: {score}";

        if (score > bestScore)
        {
            bestScore = score;
            textBestScore.gameObject.SetActive(false);
            textNewRecord.gameObject.SetActive(true);
            textNewRecord.text = $"NEW RECORD: {bestScore}";
        }
        else
        {
            textBestScore.gameObject.SetActive(true);
            textBestScore.text = $"Best: {bestScore}";
            textNewRecord.gameObject.SetActive(false);
        }
    }
    public void RetryGame()
    {
        AudioManager.Instance.Play(SoundKey.SFX_UI_UICLICK);
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void BGMChange(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    private void SFXChange(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void ToggleSet()
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
    public void CloseSet()
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

    public void GoTitle()
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
    public void GoGame()
    {
        SceneManager.LoadScene("ParkScene");
    }
    public void QuitGame()
    {
        AudioManager.Instance.Play(SoundKey.SFX_UI_UICLICK);
        UnityEditor.EditorApplication.isPlaying = false;
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
