using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class SpawnManager : MonoBehaviour
{
    public GameObject goldCoinPrefab;
    public GameObject silverCoinPrefab;

    public Transform coinsParent;       // 생성된 코인을 정리할 부모 오브젝트
    public float minY = -1f;
    public float maxY = 1f;
    public float spawnX = 10f;          // 스폰 X 좌표

    public float spawnInterval = 1.5f;

    private float timer = 0f;

    /* 난이도, 아이템별 속도 캐싱 */
    [Header("Speed Settings")]
    [SerializeField] private float cachedDifficultySpeed = 1.0f;
    [SerializeField] private float cachedItemSpeed = 1.0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRandomCoin();
            timer = 0f;
        }
    }

    /// <summary>
    /// 장애물 위치와 상관없이 랜덤 위치에 코인 스폰
    /// </summary>
    void SpawnRandomCoin()
    {
        if (goldCoinPrefab == null || silverCoinPrefab == null) return;

        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);

        GameObject prefabToSpawn = (Random.Range(0, 2) == 0) ? goldCoinPrefab : silverCoinPrefab;

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, coinsParent);
    }



    /* 코인 스폰 시작 */
    public void StartSpawning()
    {
        timer = 0f;     /* 타이머 리셋 */
        enabled = true;  /* Update 활성화 */
    }

    /* 코인 스폰 정지 */
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
}

