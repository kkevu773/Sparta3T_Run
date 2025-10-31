using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroyX = -10;

    public float highPosY = 1f;
    public float lowPosY = -1f;

    public float holeSizeMin = 1f;
    public float holeSizeMax = 3f;
    public float widthPadding = 4f;

    public GameObject[] obstaclePrefabs;

    /* 장애물 이동/정지 구분 */
    private bool canMove = true;

    private void Update()
    {
        /* canMove == false => 바로 return */
        if (!canMove) return;

        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
    public Vector3 SetRandomPlace(Vector3 lastPosition,int obstacleCount)
    {
        float difficultyFactor = 1f - Mathf.Clamp01(obstacleCount * 0.02f);
        float holeSize = Random.Range(holeSizeMin, holeSizeMax) * difficultyFactor;
        float newX = lastPosition.x + widthPadding + holeSize;
        float newY = Random.Range(lowPosY, highPosY);
        return new Vector3(newX, newY, 0f);
    }
    public GameObject SpawnRandomObstacle(Vector3 spawnPosition)
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("배열 빔");
            return null;
        }
        int randomIndex=Random.Range(0,obstaclePrefabs.Length);

        GameObject prefabToSpawn = obstaclePrefabs[randomIndex];
        if (prefabToSpawn != null)
        {
            GameObject newObstacle = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            return newObstacle;
        }
        else
        {
            Debug.LogWarning("선택된 장애물 prefab이 null입니다!");
            return null;
        }
    }



    /* 장애물 이동 정지 */
    public void StopMoving()
    {
        canMove = false;
    }

    /* 장애물 이동 시작 */
    public void StartMoving()
    {
        canMove = true;
    }
}
