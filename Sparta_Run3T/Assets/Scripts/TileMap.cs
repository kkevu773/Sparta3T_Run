using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    public float speed = 6f;          // 이동 속도
    public Camera cam;
    public float leftCullMargin = 2f;

    public Tilemap[] tilemaps;
    public bool destroyEmptyTilemapGO = false;

    int[] nextCullX;

    private bool isScrolling = true;

    /* 난이도, 아이템별 속도 조절용 */
    [SerializeField] private float difficultySpeedMultiplier = 1.0f;
    [SerializeField] private float itemSpeedMultiplier = 1.0f;

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
        /* isScrolling == false => 바로 return */
        if (!isScrolling) return;

        /* 최종 속도 = 기본 속도 * 난이도 배율 * 아이템 배율 */
        float appliedSpeed = speed * difficultySpeedMultiplier * itemSpeedMultiplier;

        transform.Translate(Vector3.left * appliedSpeed * Time.deltaTime, Space.World);

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



    /* 타일맵 스크롤 시작 */
    public void StartScroll()
    {
        isScrolling = true;
    }

    /* 타일맵 스크롤 정지 */
    public void StopScroll()
    {
        isScrolling = false;
    }

    /* 게임 재시작 시 타일맵 리셋 */
    public void ResetTilemap()
    {
        /* 타일맵 위치 초기화 */
        transform.position = Vector3.zero;

        /* Cull 인덱스 초기화 */
        for (int i = 0; i < tilemaps.Length; i++)
        {
            if (tilemaps[i] != null)
            {
                nextCullX[i] = tilemaps[i].cellBounds.xMin;
            }
        }

        /* 스크롤 시작 */
        isScrolling = true;

        Debug.Log("타일맵 리셋 완료!");
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
