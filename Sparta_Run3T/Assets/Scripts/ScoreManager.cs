using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int score = 0;

    /* 점수 변경 이벤트 */
    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"현재 점수:{score}");

        /* 점수가 변경되면 이벤트 발생! */
        OnScoreChanged?.Invoke(score);
    }

    public int GetScore()
    {
        return score;
    }
    public void ResetScore()
    {
        score = 0;
        Debug.Log("점수 초기화");

        /* 점수가 초기화되어도 이벤트 발생! */
        OnScoreChanged?.Invoke(score);
    }
}
