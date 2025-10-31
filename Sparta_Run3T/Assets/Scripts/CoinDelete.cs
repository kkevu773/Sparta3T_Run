using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinDelete : MonoBehaviour
{

    [SerializeField] private Tilemap bronzeMap;
    [SerializeField] private Tilemap silverMap;
    [SerializeField] private Tilemap goldMap;

    // 코인 점수
    [SerializeField] private int bronzeValue = 1;
    [SerializeField] private int silverValue = 3;
    [SerializeField] private int goldValue = 5;

    private Collider2D col;

    void Awake() => col = GetComponent<Collider2D>();

    void OnTriggerEnter2D(Collider2D other) { TryCollect(other); }
    void OnTriggerStay2D(Collider2D other) { TryCollect(other); }

    void TryCollect(Collider2D other)
    {
        // 코인 타일맵이 아니면 무시
        if (!other.TryGetComponent<TilemapCollider2D>(out _)) return;
        var map = other.GetComponent<Tilemap>();
        if (map == null) return;

        if (map == bronzeMap) CollectOverlappedCells(map/*, bronzeValue*/);
        else if (map == silverMap) CollectOverlappedCells(map/*, silverValue*/);
        else if (map == goldMap) CollectOverlappedCells(map/*, goldValue*/);
    }

    void CollectOverlappedCells(Tilemap map/*, int value*/)
    {
        var b = col.bounds;
        Vector3Int min = map.WorldToCell(b.min);
        Vector3Int max = map.WorldToCell(b.max);

        for (int x = min.x; x <= max.x; x++)
            for (int y = min.y; y <= max.y; y++)
            {
                var cell = new Vector3Int(x, y, 0);
                if (!map.HasTile(cell)) continue;

                // TODO: 점수/VFX/사운드
                // Score.Add(value);
                // Instantiate(vfx, map.GetCellCenterWorld(cell), Quaternion.identity);

                map.SetTile(cell, null); // 해당 코인 타일 삭제=획득
            }
    }
}
