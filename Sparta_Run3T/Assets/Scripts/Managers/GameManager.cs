using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Ready,      // 게임 시작 전, 첫 화면
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // GameManager 가 여러 개 생성되는 것을 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGame()
    {
        // TODO : 게임 시작 시, 초기화 로직 작성
    }

    public void StartGame()
    {
        
    }

    public void RestartGame()
    {
        // TODO : ActiveScene 을 재활용하는 방식으로 게임 재시작?
    }

    public void GameOver()
    {
        // TODO : 게임오버 로직 작성
    }
}
