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
    public float obstacleSpeed = 5f;

    private float timer = 0f;
    private GameObject[] obstaclePrefabs;

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
}
