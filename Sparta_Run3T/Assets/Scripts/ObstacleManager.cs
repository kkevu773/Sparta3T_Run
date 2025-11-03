using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Transform obstaclesParent;     // 장애물들을 담을 부모 오브젝트 (없으면 null)
    public float spawnX = 3f;
    public float minY = -1f;
    public float maxY = 2f;
    public float spawnInterval = 0.02f;

    private float timer = 0f;
    private GameObject[] obstaclePrefabs;

    /* 난이도, 아이템별 속도 캐싱 */
    [Header("Speed Settings")]
    [SerializeField] private float cachedDifficultySpeed = 1.0f;
    [SerializeField] private float cachedItemSpeed = 1.0f;

    Obstacle currentObstacle;

    void Awake()
    {
        // Resources,Obstacles 폴더 안에 있는 모든 프리팹 로드
        obstaclePrefabs = Resources.LoadAll<GameObject>("Obstacles");

        if (obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("Resources/Obstacles 폴더에 장애물 프리팹이 없습니다!");
        }
        else
        {
            Debug.Log($"로드된 장애물 프리팹 수: {obstaclePrefabs.Length}");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomObstacle();
            timer = 0f;
        }
    }

    void SpawnRandomObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("장애물 프리팹이 없습니다!");
            return;
        }

        // null 제외
        var validPrefabs = System.Array.FindAll(obstaclePrefabs, p => p != null);
        if (validPrefabs.Length == 0)
        {
            Debug.LogWarning("유효한 장애물이 없습니다!");
            return;
        }

        int randomIndex = Random.Range(0, validPrefabs.Length);
        GameObject prefabToSpawn = validPrefabs[randomIndex];

        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));
        spawnPos.y = randomY;
        GameObject newObstacle = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, obstaclesParent);
        newObstacle.name += "_Spawned"; // Hierarchy에서 구분용

        Debug.Log($"Spawned: {newObstacle.name} at {spawnPos}");

        Obstacle currentObstacle = newObstacle.GetComponent<Obstacle>();

        /* 스폰된 장애물에 현재 속도 배율 적용 */
        if (currentObstacle != null)
        {
            currentObstacle.SetDifficultySpeedMultiplier(cachedDifficultySpeed);
            currentObstacle.SetItemSpeedMultiplier(cachedItemSpeed);
        }
    }



    /* 씬의 모든 장애물 제거 */
    public void ClearAllObstacles()
    {
        if (obstaclesParent != null)
        {
            // 부모 오브젝트의 모든 자식 제거
            foreach (Transform child in obstaclesParent)
            {
                Destroy(child.gameObject);
            }
        }
    }

    /* 장애물 스폰 시작 */
    public void StartSpawning()
    {
        timer = 0f;     /* 타이머 리셋 */
        enabled = true;  /* Update 활성화 */
    }

    /* 장애물 스폰 정지 */
    public void StopSpawning()
    {
        enabled = false;  /* Update 비활성화 */
    }

    /* 난이도에 따른 기본 속도 배율 설정 (게임 시작 시, 한 번만) */
    public void SetDifficultySpeedMultiplier(float multiplier)
    {
        cachedDifficultySpeed = multiplier;
        Debug.Log($"{gameObject.name} 난이도 속도 배율: {multiplier}배속");
    }

    /* 아이템에 의한 일시적 속도 배율 설정 */
    public void SetItemSpeedMultiplier(float multiplier)
    {
        cachedItemSpeed = multiplier;
    }

    /* 난이도에 따른 모든 활성 장애물의 속도 조절 */
    public void SetAllObstaclesDifficultySpeed(float multiplier)
    {
        if (obstaclesParent == null) return;

        var activeObstacles = obstaclesParent.GetComponentsInChildren<Obstacle>();
        foreach (var obs in activeObstacles)
        {
            if (obs != null) obs.SetDifficultySpeedMultiplier(multiplier);
        }
    }

    /* 아이템에 의한 모든 활성 장애물의 속도 조절 */
    public void SetAllObstaclesItemSpeed(float multiplier)
    {
        if (obstaclesParent == null) return;

        var activeObstacles = obstaclesParent.GetComponentsInChildren<Obstacle>();
        foreach (var obs in activeObstacles)
        {
            if (obs != null) obs.SetItemSpeedMultiplier(multiplier);
        }
    }
}
