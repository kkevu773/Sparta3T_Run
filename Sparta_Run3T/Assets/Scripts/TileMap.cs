using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    [Header("Scroll")]
    public float speed = 6f;          // 이동 속도
    public Camera cam;
    public float leftCullMargin = 2f;

    [Header("Targets")]
    public Tilemap[] tilemaps;
    public bool destroyEmptyTilemapGO = false;

    int[] nextCullX;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        if (tilemaps == null || tilemaps.Length == 0)
            tilemaps = GetComponentsInChildren<Tilemap>();

        nextCullX = new int[tilemaps.Length];
        for (int i = 0; i < tilemaps.Length; i++)
            nextCullX[i] = tilemaps[i].cellBounds.xMin;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        float leftEdge = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f)).x - leftCullMargin;

        // 순서대로 제거
        for (int i = 0; i < tilemaps.Length; i++)
        {
            var map = tilemaps[i];
            var b = map.cellBounds;

            while (nextCullX[i] < b.xMax)
            {
                float colRightWorldX = map.CellToWorld(new Vector3Int(nextCullX[i] + 1, 0, 0)).x;
                if (colRightWorldX >= leftEdge) break;

                ClearColumn(map, nextCullX[i], b.yMin, b.yMax);
                nextCullX[i]++;
            }

            if (destroyEmptyTilemapGO && map.GetUsedTilesCount() == 0)
            {
                Destroy(map.gameObject);
            }
        }
    }

    static void ClearColumn(Tilemap map, int x, int yMin, int yMax)
    {
        int count = yMax - yMin;
        if (count <= 0) return;

        var positions = new Vector3Int[count];
        var tiles = new TileBase[count]; // 전부 null

        int idx = 0;
        for (int y = yMin; y < yMax; y++)
            positions[idx++] = new Vector3Int(x, y, 0);

        map.SetTiles(positions, tiles);
    }
}
