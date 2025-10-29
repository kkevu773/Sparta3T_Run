using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    public Transform goldCoinPrefab;
    public Transform silverCoinPrefab;

    public float minY = -1f;
    public float maxY = 1f;
    public float xSpacing = 2f;

    private Vector3 lastSpawnPosition=Vector3.zero;

    public void SpawnCoin(Vector3 obstacleposition)
    {
        float newX = obstacleposition.x + xSpacing;

        float newY = Random.Range(minY, maxY);

        Vector3 spawnPos = new Vector3(newX, newY, 0f);
        int random = Random.Range(0, 2);

        if (random == 0 && goldCoinPrefab != null)
            Instantiate(goldCoinPrefab, spawnPos, Quaternion.identity);
        else if (silverCoinPrefab != null)
            Instantiate(silverCoinPrefab, spawnPos, Quaternion.identity);

        lastSpawnPosition = spawnPos;



    }
}
