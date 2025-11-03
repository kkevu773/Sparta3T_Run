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

// 게임 난이도
public enum Difficulty
{
    Easy,       // 쉬움 (0.8배속)
    Normal,     // 보통 (1배속)
    Hard        // 어려움 (1.2배속)
}

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private BackgroundManager bgManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PlayerMove player;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private TileMap tileMap;
    [SerializeField] private ItemManager itemManager;

    [Header("Difficulty Info")]
    [SerializeField] private Difficulty currentDifficulty = Difficulty.Normal;
    public Difficulty CurrentDifficulty => currentDifficulty;

    [Header("Speed Info")]
    [SerializeField] private float difficultySpeedFactor = 1f;       // 난이도에 따른 기본 고정 속도 배율
    public float DifficultySpeedFactor => difficultySpeedFactor;

    [SerializeField] private float itemSpeedMultiplier = 1f;         // 아이템에 의한 일시적인 속도 배율
    private Coroutine speedEffectCoroutine;     // 속도 효과 코루틴

    [Header("State Info")]
    [SerializeField] private GameState currentState = GameState.Playing;
    public GameState CurrentState => currentState;

    public static GameManager Instance { get; private set; }

    // PlayerPrefs 호출용 key string
    private const string BEST_SCORE_KEY = "BestScore";

    // 최고 점수 저장용 변수
    private int bestScore = 0;

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

        // 씬 로드 이벤트 리스너 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 이벤트 리스너 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ReconnectManagerReferences(scene.name));
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

                // 난이도 설정 테스트용 (개발용 - 나중에 삭제)
                if (Input.GetKeyDown(KeyCode.Alpha1))   // 숫자 1 키
                {
                    SetDifficulty(Difficulty.Easy);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))   // 숫자 2 키
                {
                    SetDifficulty(Difficulty.Normal);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))   // 숫자 3 키
                {
                    SetDifficulty(Difficulty.Hard);
                }
                break;

            case GameState.Playing:
                // P 키로 점수 획득 테스트 (개발용 - 나중에 삭제)
                if (Input.GetKeyDown(KeyCode.P))
                {
                    AddScore(10);
                }

                // H 키로 회복 아이템 획득 테스트 (개발용 - 나중에 삭제)
                if (Input.GetKeyDown(KeyCode.H))
                {
                    OnHealItemCollected(1);
                }
                // U 키로 속도 증가 아이템 획득 테스트 (개발용 - 나중에 삭제)
                if (Input.GetKeyDown(KeyCode.U))
                {
                    OnSpeedUpItemCollected(1.5f, 5f);
                }
                // D 키로 속도 감소 아이템 획득 테스트 (개발용 - 나중에 삭제)
                if (Input.GetKeyDown(KeyCode.D))
                {
                    OnSpeedDownItemCollected(2f, 5f);
                }
                break;

            case GameState.GameOver:
                // R 키를 눌러 재시작 (개발용 - 나중에 삭제)
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RestartGame();
                }
                break;
        }
    }

    // 게임 시작 전, 난이도 설정
    public void SetDifficulty(Difficulty difficulty)
    {
        if (currentState != GameState.Ready)
        {
            Debug.LogWarning("난이도 설정은 게임 시작 전에만 가능합니다!!!");
            return;
        }

        currentDifficulty = difficulty;

        // 난이도에 따른 속도 배율 설정
        switch (difficulty)
        {
            case Difficulty.Easy:
                difficultySpeedFactor = 0.7f;
                break;
            case Difficulty.Normal:
                difficultySpeedFactor = 1.0f;
                break;
            case Difficulty.Hard:
                difficultySpeedFactor = 1.3f;
                break;
        }

        Debug.Log($"난이도 설정 완료 : {currentDifficulty} ({difficultySpeedFactor}배속)");
    }

    // 게임 초기화 (첫 실행 시)
    public void InitGame()
    {
        Debug.Log("=== 게임 초기화 시작 ===");

        // 게임 상태를 Ready로 설정
        currentState = GameState.Ready;

        // 속도 초기화
        itemSpeedMultiplier = 1.0f;
        if (speedEffectCoroutine != null)
        {
            StopCoroutine(speedEffectCoroutine);
            speedEffectCoroutine = null;
        }

        // 속도 기본값 설정 (Normal)
        if (difficultySpeedFactor == 0f)
        {
            SetDifficulty(Difficulty.Normal);
        }

        // 점수 초기화
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }

        // 최고점 불러오기 및 UI 갱신
        LoadBestScore();

        // UI 초기화 - HUD 표시
        if (uiManager != null)
        {
            uiManager.UpdateScore(0);
            uiManager.UpdateBestScore(bestScore);

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

        // 모든 매니저에 난이도 기반 속도 적용
        ApplyDifficultySpeedToAll(difficultySpeedFactor);

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

        // 속도 증감 효과 중단
        if (speedEffectCoroutine != null)
        {
            StopCoroutine(speedEffectCoroutine);
            speedEffectCoroutine = null;
        }
        itemSpeedMultiplier = 1.0f;

        // 최고점 갱신 로직 (현재 점수 비교 후 필요 시 저장)
        SaveBestScoreIfNeeded();

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
            // GameOver 패널에서도 최고점 표시 갱신
            uiManager.UpdateBestScore(bestScore);

            uiManager.ShowUI(UIKey.UI_GAMEMOVER_PANEL, true);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_RETRY_BUTTON, true);
            uiManager.ShowUI(UIKey.UI_GAMEMOVER_TITLE_BUTTON, true);
            uiManager.ShowGameOver(scoreManager != null ? scoreManager.GetScore() : 0);
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

        Debug.Log("=== 게임 재시작 - 씬 Reload ===");

        // 씬 전체 재로드(모든 타일 복구됨!)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 씬 재로드 후, 모든 매니저 재연결
    private IEnumerator ReconnectManagerReferences(string sceneName)
    {
        // 씬이 완전히 로드될 때까지 한 프레임 대기
        yield return new WaitForEndOfFrame();

        // 모든 매니저들을 찾아서 다시 연결
        bgManager = FindObjectOfType<BackgroundManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        player = FindObjectOfType<PlayerMove>();
        obstacleManager = FindObjectOfType<ObstacleManager>();
        uiManager = FindObjectOfType<UIManager>();
        audioManager = FindObjectOfType<AudioManager>();
        tileMap = FindObjectOfType<TileMap>();
        itemManager = FindObjectOfType<ItemManager>();

        // 게임 초기화 실행
        InitGame();

        // GameScene이고 난이도가 이미 설정되어 있으면 자동으로 게임 시작
        if (sceneName == "GameScene" && difficultySpeedFactor > 0f)
        {
            yield return new WaitForEndOfFrame(); // 한 프레임 더 대기 (매니저 초기화 완료 대기)
            StartGame();
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

        // 아이템 스폰 시작
        if (itemManager != null)
        {
            itemManager.enabled = true;
            itemManager.StartSpawning();
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

        // 아이템 스폰 정지
        if (itemManager != null)
        {
            itemManager.enabled = false;
            itemManager.StopSpawning();
        }

        // 이미 스폰된 장애물 / 코인 이동 정지
        if (obstacleManager != null && obstacleManager.obstaclesParent != null)
        {
            var obstacles = obstacleManager.obstaclesParent.GetComponentsInChildren<Obstacle>(true);
            foreach (var ob in obstacles)
            {
                if (ob != null) ob.StopMoving();
            }
        }
    }

    // PlayerPrefs 관련: 최고점 불러오기
    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
    }

    // PlayerPrefs 관련: 현재 점수와 비교해 필요하면 저장
    private void SaveBestScoreIfNeeded()
    {
        if (scoreManager == null) return;

        int currentScore = scoreManager.GetScore();
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);
            PlayerPrefs.Save();
            Debug.Log($"새 최고점 저장: {bestScore}");
        }
    }

    // HP 회복 아이템 획득 처리
    public void OnHealItemCollected(int healAmount)
    {
        if (currentState != GameState.Playing) return;

        // PlayerHp 컴포넌트 찾아서 체력 회복
        if (player != null)
        {
            PlayerHp playerHp = player.GetComponent<PlayerHp>();
            if (playerHp != null)
            {
                playerHp.Heal(healAmount);
                Debug.Log($"HP 회복 아이템 획득! +{healAmount}");
            }
        }
    }

    // 속도 증가 아이템 획득 처리
    public void OnSpeedUpItemCollected(float multiplier, float duration)
    {
        if (currentState != GameState.Playing) return;

        // 기존 속도 효과 중단
        if (speedEffectCoroutine != null)
        {
            StopCoroutine(speedEffectCoroutine);
        }

        // 새로운 속도 효과 시작
        speedEffectCoroutine = StartCoroutine(ApplySpeedEffect(multiplier, duration, true));
    }

    // 속도 감소 아이템 획득 처리
    public void OnSpeedDownItemCollected(float multiplier, float duration)
    {
        if (currentState != GameState.Playing) return;

        // 기존 속도 효과 중단
        if (speedEffectCoroutine != null)
        {
            StopCoroutine(speedEffectCoroutine);
        }

        // 속도 감소는 역수로 계산 (예: 2f → 0.5f, 속도를 절반으로)
        float speedDownMultiplier = 1f / multiplier;

        // 새로운 속도 효과 시작
        speedEffectCoroutine = StartCoroutine(ApplySpeedEffect(speedDownMultiplier, duration, false));
    }

    // 속도 효과 적용 코루틴
    private IEnumerator ApplySpeedEffect(float multiplier, float duration, bool isSpeedUp)
    {
        // 속도 적용
        itemSpeedMultiplier = multiplier;
        ApplyItemSpeedToAll(multiplier);

        string effectName = isSpeedUp ? "속도 증가" : "속도 감소";
        Debug.Log($"{effectName} 효과 시작! {multiplier}배, {duration}초");

        // 지속 시간 대기
        yield return new WaitForSeconds(duration);

        // 원래 속도로 복귀
        itemSpeedMultiplier = 1f;
        ApplyItemSpeedToAll(1f);

        Debug.Log($"{effectName} 효과 종료!");

        speedEffectCoroutine = null;
    }

    // 모든 매니저에 난이도 속도 배율 적용
    private void ApplyDifficultySpeedToAll(float multiplier)
    {
        // 배경 속도 변경
        if (bgManager != null)
        {
            bgManager.SetDifficultySpeedMultiplier(multiplier);
        }

        // 타일맵 속도 변경
        if (tileMap != null)
        {
            tileMap.SetDifficultySpeedMultiplier(multiplier);
        }

        // 장애물 속도 변경 (매니저 캐시 업데이트 + 이미 스폰된 장애물들도 실시간 적용)
        if (obstacleManager != null)
        {
            obstacleManager.SetDifficultySpeedMultiplier(multiplier);
            obstacleManager.SetAllObstaclesDifficultySpeed(multiplier);
        }

        // 아이템 속도 변경 (매니저 캐시 업데이트 + 이미 스폰된 아이템들도 실시간 적용)
        if (itemManager != null)
        {
            itemManager.SetDifficultySpeedMultiplier(multiplier);
            itemManager.SetAllItemsDifficultySpeed(multiplier);
        }
    }

    // 모든 매니저에 아이템 속도 배율 적용
    private void ApplyItemSpeedToAll(float multiplier)
    {
        // 배경 속도 변경
        if (bgManager != null)
        {
            bgManager.SetItemSpeedMultiplier(multiplier);
        }

        // 타일맵 속도 변경
        if (tileMap != null)
        {
            tileMap.SetItemSpeedMultiplier(multiplier);
        }

        // 장애물 속도 변경 (매니저 캐시 업데이트 + 이미 스폰된 장애물들도 실시간 적용)
        if (obstacleManager != null)
        {
            obstacleManager.SetItemSpeedMultiplier(multiplier);
            obstacleManager.SetAllObstaclesItemSpeed(multiplier);
        }

        // 아이템 속도 변경 (매니저 캐시 업데이트 + 이미 스폰된 아이템들도 실시간 적용)
        if (itemManager != null)
        {
            itemManager.SetItemSpeedMultiplier(multiplier);
            itemManager.SetAllItemsItemSpeed(multiplier);
        }
    }
    public void GameClear()
    {
        if (currentState != GameState.Playing)
        {
            return;
        }

        Debug.Log("게임 클리어");
        currentState = GameState.GameOver;

        StopAllSpawners();
        if (bgManager != null)
        {
            bgManager.StopScroll();
        }
        if (tileMap != null)
        {
            tileMap.StopScroll();
        }
        if (player != null)
        {
            player.StopPlaying();
        }

        SaveBestScoreIfNeeded();
        if (uiManager != null)
        {
            uiManager.ShowGameOver(scoreManager != null ? scoreManager.GetScore() : 0);
        }

        AudioManager.Instance.Play(SoundKey.SFX_UI_GAMEOVER);
    }
}