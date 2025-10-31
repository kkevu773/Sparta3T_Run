using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready,      // 게임 시작 전
    Playing,    // 게임 플레이 중
    GameOver    // 게임 오버
}

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private BackgroundManager bgManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PlayerMove player;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private TileMap tileMap;

    public static GameManager Instance { get; private set; }

    public GameState currentState = GameState.Ready;
    public GameState CurrentState => currentState;

    private void Awake()
    {
        // GameManager가 여러 개 생성되는 것을 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitGame();
    }

    private void Update()
    {
        HandleInput();
    }

    // 게임 상태에 따른 입력 처리
    private void HandleInput()
    {
        switch (currentState)
        {
            case GameState.Ready:
                // 스페이스바를 눌러 게임 시작
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartGame();
                }
                break;

            case GameState.Playing:
                // P 키로 점수 테스트 (개발용 - 나중에 삭제)
                if (Input.GetKeyDown(KeyCode.P))
                {
                    AddScore(10);
                }
                break;

            case GameState.GameOver:
                // R 키를 눌러 재시작
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RestartGame();
                }
                break;
        }
    }

    // 게임 초기화 (첫 실행 시)
    public void InitGame()
    {
        Debug.Log("=== 게임 초기화 시작 ===");

        // 게임 상태를 Ready로 설정
        currentState = GameState.Ready;

        // 점수 초기화
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }

        // UI 초기화 - HUD 표시
        if (uiManager != null)
        {
            uiManager.UpdateScore(0);
            uiManager.ShowUI(UIKey.UI_HUD_SCORE_TEXT, true);
            uiManager.ShowUI(UIKey.UI_HUD_BESTSCORE_TEXT, true);
            uiManager.ShowUI(UIKey.UI_HUD_HP_BAR, true);

            // GameOver UI 숨기기
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_PANEL, false);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_RETRY_BUTTON, false);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_TITLE_BUTTON, false);

            // Title UI 숨기기 (혹시 켜져있다면)
            uiManager.ShowUI(UIKey.UI_TITLE_PANEL, false);
        }

        // BGM 재생
        if (audioManager != null)
        {
            audioManager.PlayBGM(SoundKey.BGM_DEFAULT);
        }

        // 모든 스폰 매니저 비활성화 (Ready 상태에서는 스폰 안 함)
        StopAllSpawners();

        // 배경 스크롤 정지 (Ready 상태에서는 배경도 멈춤)
        if (bgManager != null)
        {
            bgManager.StopScroll();
        }

        // 타일맵 스크롤 정지
        if (tileMap != null)
        {
            tileMap.StopScroll();
        }

        // 플레이어 정지 (Ready 상태에서는 입력 받지 않음)
        if (player != null)
        {
            player.StopPlaying();
        }

        Debug.Log("=== 게임 초기화 완료 - Ready 상태 (스페이스바를 눌러 시작) ===");
    }

    // 게임 시작 (Ready -> Playing)
    public void StartGame()
    {
        if (currentState != GameState.Ready) return;

        Debug.Log("=== 게임 시작! ===");

        // 게임 상태를 Playing으로 변경
        currentState = GameState.Playing;

        // 모든 스폰 매니저 활성화
        StartAllSpawners();

        // 배경 스크롤 시작
        if (bgManager != null)
        {
            bgManager.StartScroll();
        }

        // 타일맵 스크롤 시작
        if (tileMap != null)
        {
            tileMap.StartScroll();
        }

        // 플레이어 활성화
        if (player != null)
        {
            player.StartPlaying();
        }

        // 게임 시작 효과음 (선택사항)
        audioManager?.Play(SoundKey.SFX_UI_UICLICK);
    }

    // 점수 추가 (코인 획득 시 호출)
    public void AddScore(int value)
    {
        if (currentState != GameState.Playing) return;

        if (scoreManager != null)
        {
            scoreManager.AddScore(value);

            // 디버깅용 코드
            int currentScore = scoreManager.GetScore();
            Debug.Log($"현재 점수: {currentScore}");
            Debug.Log($"UIManager null 여부: {uiManager == null}");

            if (uiManager != null)
            {
                uiManager.UpdateScore(scoreManager.GetScore());
            }
        }
    }

    // 게임 오버 (플레이어 사망 시 호출)
    public void GameOver()
    {
        if (currentState != GameState.Playing) return;

        Debug.Log("=== 게임 오버! ===");

        // 게임 상태를 GameOver로 변경
        currentState = GameState.GameOver;

        // 모든 스폰 정지
        StopAllSpawners();

        // 배경 스크롤 정지
        if (bgManager != null)
        {
            bgManager.StopScroll();
        }

        // 타일맵 스크롤 정지
        if (tileMap != null)
        {
            tileMap.StopScroll();
        }

        // 플레이어 정지
        if (player != null)
        {
            player.StopPlaying();
        }

        // GameOver UI 표시
        if (uiManager != null)
        {
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_PANEL, true);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_RETRY_BUTTON, true);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_TITLE_BUTTON, true);
        }

        // 게임오버 사운드 재생
        if (audioManager != null)
        {
            audioManager.Play(SoundKey.SFX_UI_GAMEOVER);
        }
    }

    /// 게임 재시작 (GameOver -> Ready -> Playing)
    public void RestartGame()
    {
        if (currentState != GameState.GameOver) return;

        Debug.Log("=== 게임 재시작 ===");

        // 1. 씬 내의 모든 게임 오브젝트 정리
        ClearGameObjects();

        // 2. 매니저들 리셋
        ResetAllManagers();

        // 3. 게임 재초기화 (Ready 상태로)
        InitGame();

        // 4. 바로 게임 시작하려면 이 줄 주석 해제
        //StartGame();
    }

    // 씬 내의 모든 게임 오브젝트 정리
    private void ClearGameObjects()
    {
        // 장애물 전부 제거
        if (obstacleManager != null)
        {
            obstacleManager.ClearAllObstacles();
        }

        // 코인 전부 제거
        if (spawnManager != null)
        {
            spawnManager.ClearAllCoins();
        }
    }

    // 모든 매니저 리셋
    private void ResetAllManagers()
    {
        // 플레이어 리셋
        if (player != null)
        {
            player.ResetPlayer();
        }

        // 배경 리셋
        if (bgManager != null)
        {
            bgManager.ResetBackground();
        }

        // 타일맵 리셋
        if (tileMap != null)
        {
            tileMap.ResetTilemap();
        }

        // 점수 리셋
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }

        // UI 리셋
        if (uiManager != null)
        {
            uiManager.UpdateScore(0);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_PANEL, false);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_RETRY_BUTTON, false);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_TITLE_BUTTON, false);
        }
    }

    // 모든 스폰 매니저 시작
    private void StartAllSpawners()
    {
        // 장애물 스폰 시작
        if (obstacleManager != null)
        {
            obstacleManager.enabled = true;

            // 타이머 리셋용
            obstacleManager.StartSpawning();
        }

        // 코인 스폰 시작
        if (spawnManager != null)
        {
            spawnManager.enabled = true;

            // 타이머 리셋용
            spawnManager.StartSpawning();
        }
    }

    // 모든 스폰 매니저 정지
    private void StopAllSpawners()
    {
        // 장애물 스폰 정지
        if (obstacleManager != null)
        {
            obstacleManager.enabled = false;

            obstacleManager.StopSpawning();
        }

        // 코인 스폰 정지
        if (spawnManager != null)
        {
            spawnManager.enabled = false;

            spawnManager.StopSpawning();
        }
    }

    // 타이틀 화면으로 돌아가기 (나중에 구현)
    public void GoToTitle()
    {
        Debug.Log("타이틀 화면으로 이동 (미구현)");

        // 모든 스폰 정지
        StopAllSpawners();

        // UI 전환
        if (uiManager != null)
        {
            // 모든 UI 숨기기
            uiManager.ShowUI(UIKey.UI_HUD_SCORE_TEXT, false);
            uiManager.ShowUI(UIKey.UI_HUD_BESTSCORE_TEXT, false);
            uiManager.ShowUI(UIKey.UI_HUD_HP_BAR, false);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_PANEL, false);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_RETRY_BUTTON, false);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_TITLE_BUTTON, false);

            // 타이틀 UI 보이기
            uiManager.ShowUI(UIKey.UI_TITLE_PANEL, true);
        }

        // BGM 변경 (타이틀 BGM이 있다면)
        // audioManager?.PlayBGM(SoundKey.BGM_TITLE);

        currentState = GameState.Ready;
    }
}