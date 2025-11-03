using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    public Transform obstaclesParent;     // 장애물들을 담을 부모 오브젝트 (없으면 null)
    public float spawnX = 3f;
    public float minY = -1f;
    public float maxY = 2f;
    public float spawnInterval = 0.02f;

    private float timer = 0f;
    private GameObject[] obstaclePrefabs;

    /* 난이도, 아이템별 속도 변화 캐싱 */
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
        spawnPos.y = Mathf.Round(randomY);
        GameObject newObstacle = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, obstaclesParent);
        newObstacle.name += "_Spawned"; // Hierarchy에서 구분용

        Debug.Log($"Spawned: {newObstacle.name} at {spawnPos}");

        /* 새롭게 스폰된 장애물에 현재 속도 배율 적용 */
        ApplySpeedToObstacle(newObstacle);
    }



    /* 스폰된 장애물에 현재 속도 배율 적용 */
    private void ApplySpeedToObstacle(GameObject obstacle)
    {
        Obstacle obstacleComponent = obstacle.GetComponent<Obstacle>();
        if (obstacleComponent != null)
        {
            obstacleComponent.SetDifficultySpeedMultiplier(cachedDifficultySpeed);
            obstacleComponent.SetItemSpeedMultiplier(cachedItemSpeed);
        }
    }

    /* 씬의 모든 장애물 제거 */
    public void ClearAllObstacles()
    {
        if (obstaclesParent != null)
        {
            /* 부모 오브젝트(Obstacles) 의 모든 자식(각각의 장애물들) 제거 */
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

    /* 난이도에 따른 기본 속도 배율 캐싱 from GameManager */
    public void SetDifficultySpeedMultiplier(float multiplier)
    {
        cachedDifficultySpeed = multiplier;
    }

    /* 아이템에 의한 일시적 속도 배율 캐싱 from GameManager */
    public void SetItemSpeedMultiplier(float multiplier)
    {
        cachedItemSpeed = multiplier;
    }

    /* 이미 스폰된 장애물들에 난이도별 기본 속도 적용 */
    public void SetAllObstaclesDifficultySpeed(float multiplier)
    {
        ApplySpeedToAllObstacles(multiplier, isDifficultySpeed: true);
    }

    /* 이미 스폰된 장애물들에 아이템별 속도 변화 적용 */
    public void SetAllObstaclesItemSpeed(float multiplier)
    {
        ApplySpeedToAllObstacles(multiplier, isDifficultySpeed: false);
    }

    /* 스폰된 장애물(Obstacle.cs) 에 난이도, 아이템별 속도 값 적용 */
    private void ApplySpeedToAllObstacles(float multiplier, bool isDifficultySpeed)
    {
        if (obstaclesParent == null) return;

        var activeObstacles = obstaclesParent.GetComponentsInChildren<Obstacle>();
        foreach (var obs in activeObstacles)
        {
            if (obs != null)
            {
                if (isDifficultySpeed)
                    obs.SetDifficultySpeedMultiplier(multiplier);
                else
                    obs.SetItemSpeedMultiplier(multiplier);
            }
        }
    }
}