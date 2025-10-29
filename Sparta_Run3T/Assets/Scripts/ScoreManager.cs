using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int score = 0;

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
    }

    public int GetScore()
    {
        return score;
    }
    public void ResetScore()
    {
        score = 0;
        Debug.Log("점수 초기화");
    }
}
