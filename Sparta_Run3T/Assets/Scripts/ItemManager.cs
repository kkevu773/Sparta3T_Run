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
}
