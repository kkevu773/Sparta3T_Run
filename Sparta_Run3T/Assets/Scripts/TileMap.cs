using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    [Header("Scroll")]
    public float speed = 6f;          // 왼쪽 이동 속도
    public Camera cam;                // 비우면 MainCamera
    public float leftCullMargin = 2f; // 화면 왼쪽 밖 여유

    [Header("Targets")]
    public Tilemap[] tilemaps;        // 비우면 자식에서 자동 수집
    public bool destroyEmptyTilemapGO = false; // 다 지워지면 GO까지 삭제

    int[] nextCullX;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        if (tilemaps == null || tilemaps.Length == 0)
            tilemaps = GetComponentsInChildren<Tilemap>();

        nextCullX = new int[tilemaps.Length];
        for (int i = 0; i < tilemaps.Length; i++)
            nextCullX[i] = tilemaps[i].cellBounds.xMin; // 맨 왼쪽 열부터 시작
    }

    void Update()
    {
        // 1) 전체 스테이지를 왼쪽으로 이동
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        // 2) 화면 왼쪽 경계(조금 더 왼쪽으로) 월드 X
        float leftEdge = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f)).x - leftCullMargin;

        // 3) 경계 밖으로 나간 "열"을 순서대로 제거
        for (int i = 0; i < tilemaps.Length; i++)
        {
            var map = tilemaps[i];
            var b = map.cellBounds; // xMin/xMax, yMin/yMax (yMax는 배타)

            while (nextCullX[i] < b.xMax)
            {
                // 이 열의 오른쪽 경계 월드X
                float colRightWorldX = map.CellToWorld(new Vector3Int(nextCullX[i] + 1, 0, 0)).x;
                if (colRightWorldX >= leftEdge) break;

                ClearColumn(map, nextCullX[i], b.yMin, b.yMax);
                nextCullX[i]++;
            }

            if (destroyEmptyTilemapGO && map.GetUsedTilesCount() == 0)
            {
                Destroy(map.gameObject); // 완전히 비었으면 통째 삭제(옵션)
            }
        }
    }

    // 열 하나를 비움(타일 = null). 콜라이더는 TilemapCollider2D/Composite가 알아서 갱신.
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
