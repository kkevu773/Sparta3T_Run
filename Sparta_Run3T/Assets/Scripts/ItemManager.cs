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

    /* 난이도, 아이템별 속도 변화 캐싱 */
    [Header("Speed Settings")]
    [SerializeField] private float cachedDifficultySpeed = 1.0f;
    [SerializeField] private float cachedItemSpeed = 1.0f;

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

        /* 새롭게 스폰된 아이템에 현재 속도 배율 적용 */
        ApplySpeedToItem(newItem);
    }



    /* 스폰된 아이템에 현재 속도 배율 적용 */
    private void ApplySpeedToItem(GameObject item)
    {
        /* Hp 회복 아이템 */
        Hp hpItem = item.GetComponent<Hp>();
        if (hpItem != null)
        {
            hpItem.SetDifficultySpeedMultiplier(cachedDifficultySpeed);
            hpItem.SetItemSpeedMultiplier(cachedItemSpeed);
            return;
        }

        /* 속도 증감 아이템 */
        SpeedItem speedItem = item.GetComponent<SpeedItem>();
        if (speedItem != null)
        {
            speedItem.SetDifficultySpeedMultiplier(cachedDifficultySpeed);
            speedItem.SetItemSpeedMultiplier(cachedItemSpeed);
        }
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

    /* 이미 스폰된 아이템들에 난이도별 기본 속도 적용 */
    public void SetAllItemsDifficultySpeed(float multiplier)
    {
        if (itemsParent == null) return;

        /* Hp 회복 아이템 */
        var hpItems = itemsParent.GetComponentsInChildren<Hp>(true);
        foreach (var item in hpItems)
        {
            if (item != null) item.SetDifficultySpeedMultiplier(multiplier);
        }

        /* 속도 증감 아이템 */
        var speedItems = itemsParent.GetComponentsInChildren<SpeedItem>(true);
        foreach (var item in speedItems)
        {
            if (item != null) item.SetDifficultySpeedMultiplier(multiplier);
        }
    }

    /* 이미 스폰된 아이템들에 아이템별 속도 변화 적용 */
    public void SetAllItemsItemSpeed(float multiplier)
    {
        if (itemsParent == null) return;

        /* Hp 회복 아이템 */
        var hpItems = itemsParent.GetComponentsInChildren<Hp>(true);
        foreach (var item in hpItems)
        {
            if (item != null) item.SetItemSpeedMultiplier(multiplier);
        }

        /* 속도 증감 아이템 */
        var speedItems = itemsParent.GetComponentsInChildren<SpeedItem>(true);
        foreach (var item in speedItems)
        {
            if (item != null) item.SetItemSpeedMultiplier(multiplier);
        }
    }
}
