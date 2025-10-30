using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready,      // 게임 시작 전, 첫 화면
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private BackgroundManager bgManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PlayerMove player;
    
    public static GameManager Instance { get; private set; }

    private GameState currentState = GameState.Playing;

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
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGame()
    {
        // TODO : 게임 시작 시, 초기화 로직 작성

        // 점수 초기화
        scoreManager?.ResetScore();

        // TODO : 첫 화면 구현 시, 주석 해제
        //currentState = GameState.Ready;
    }

    public void StartGame()
    {
        // TODO : 게임 플레이를 시작하는 로직 작성
        currentState = GameState.Playing;

        //spawnManager?.SpawnCoin(spawnManager.LastSpawnPosition);
    }    

    public void GameOver()
    {
        // TODO : 게임오버 로직 작성
        currentState = GameState.GameOver;
        Debug.Log("게임 오버!");
    }

    public void RestartGame()
    {
        // TODO : ActiveScene 을 재활용하는 방식으로 게임 재시작?
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
