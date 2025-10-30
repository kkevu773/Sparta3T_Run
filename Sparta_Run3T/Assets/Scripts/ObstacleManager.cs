using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Obstacle obstaclePrefab;
    public int obstacleCount = 0;

    public float spawnInterval = 2f;
    public Vector3 startSpawnPos = new Vector3(10f, 0f, 0f);

    private float timer = 0f;
    private Vector3 lastSpawnPos;

    private void Start()
    {
        lastSpawnPos = startSpawnPos;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }
    private void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogWarning("연결되지않음");
            return;
        }
        Vector3 spawnPos = obstaclePrefab.SetRandomPlace(lastSpawnPos, obstacleCount);

        GameObject newObstacle = obstaclePrefab.SpawnRandomObstacle(spawnPos);

        if (newObstacle != null)
        {
            lastSpawnPos = spawnPos;
            obstacleCount++;
        }
    }
}
