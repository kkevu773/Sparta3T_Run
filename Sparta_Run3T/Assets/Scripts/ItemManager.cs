using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject hpItemPrefab;
    public GameObject speedUpItemPrefab;
    public GameObject speedDownItemPrefab;

    public Transform itemsParent;
    public float spawnX = 10f;
    public float minY = -1f;
    public float maxY = 1f;
    public float spawnInterval = 3f;

    private float timer = 0f;

    /* 난이도, 아이템별 속도 조절용 */
    [Header("Speed Settings")]
    [SerializeField] private float difficultySpeedMultiplier = 1.0f;
    [SerializeField] private float itemSpeedMultiplier = 1.0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomItem();
            timer = 0f;
        }
    }
    void SpawnRandomItem()
    {
        int rand = Random.Range(0, 3);
        GameObject prefabToSpawn = null;

        switch (rand)
        {
            case 0:
                prefabToSpawn = hpItemPrefab;
                break;
            case 1:
                prefabToSpawn = speedUpItemPrefab;
                break;
            case 2:
                prefabToSpawn = speedDownItemPrefab;
                break;
        }
        if (prefabToSpawn == null)
            return;

        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);

        GameObject newItem = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, itemsParent);
        newItem.name += "+Spawned";
    }

    /* 아이템 스폰 시작 */
    public void StartSpawning()
    {
        timer = 0f;     /* 타이머 리셋 */
        enabled = true;  /* Update 활성화 */
    }

    /* 아이템 스폰 정지 */
    public void StopSpawning()
    {
        enabled = false;  /* Update 비활성화 */
    }

    /* 난이도에 따른 기본 속도 배율 설정 (게임 시작 시, 한 번만) */
    public void SetDifficultySpeedMultiplier(float multiplier)
    {
        difficultySpeedMultiplier = multiplier;
        Debug.Log($"{gameObject.name} 난이도 속도 배율: {multiplier}배속");
    }

    /* 아이템에 의한 일시적 속도 배율 설정 */
    public void SetItemSpeedMultiplier(float multiplier)
    {
        itemSpeedMultiplier = multiplier;
    }
}
