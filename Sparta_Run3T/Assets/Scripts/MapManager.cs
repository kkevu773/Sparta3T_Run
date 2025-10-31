using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] mapTypes;
    [SerializeField][Range(1f, 40f)] private float speed = 6f;
    [SerializeField] private int activeCount = 3;

    private List<GameObject> activeMaps = new List<GameObject>();
    private float mapWidth = 0f;
    private Camera cam;
    public BoundsInt cellBounds;

    private void Start()
    {
        cam = Camera.main;
        //맵을 랜덤으로 생성시킨다 3개
        InitMaps();
    }

    private void Update()
    {
        MoveMaps();
        LoopMaps();
    }
    //업데이트에서 왼쪽으로 가게 만든다
    //카메라 밖을 나가면 제일 오른쪽으로 간다
    private void InitMaps()
    {
        if (mapTypes.Length == 0) return;
        Debug.Log(" 맵만듬");
        Tilemap renderer = mapTypes[0].GetComponentInChildren<Tilemap>();
        Debug.Log(renderer.localBounds);
        //mapWidth = renderer.bounds.size.x + 18;
        // Debug.Log(mapWidth + " / " + renderer.bounds.size.x);
        for (int i = 0; i < activeCount; i++)
        {
            int rand = Random.Range(0,mapTypes.Length);
            Vector3 spawnPos = new Vector3(i * mapWidth, 0, 0);
            Debug.Log(i * mapWidth + " / " + mapWidth);
            GameObject map = Instantiate(mapTypes[rand], spawnPos , Quaternion.identity, transform);
            //activeMaps.Add(map);
        }
    }

    private void MoveMaps()
    {
        foreach(var map in activeMaps)
        {
            map.transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    private void LoopMaps() //카메라 왼쪽을 구한다 나가면 사라진다 
    {
        if (activeMaps.Count == 0) return;

        GameObject first = activeMaps[0];
        GameObject last = activeMaps[activeMaps.Count - 1];

        float cameraLeftEdge = cam.transform.position.x - (cam.orthographicSize * cam.aspect);
        float mapRightX = first.transform.position.x + mapWidth / 2f;

        if(mapRightX < cameraLeftEdge)
        {
            int rand = Random.Range(0, mapTypes.Length);
            GameObject newmap = Instantiate(mapTypes[rand], transform);

            newmap.transform.position = new Vector3(
                last.transform.position.x + mapWidth,
                first.transform.position.y,
                first.transform.position.z
                );

            activeMaps.RemoveAt(0);
            Destroy(first);
            activeMaps.Add(newmap);
        }

    }
}
  