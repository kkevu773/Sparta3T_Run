using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int healAmount = 10;

    /* 난이도, 아이템별 속도 조절용 */
    [Header("Speed Settings")]
    [SerializeField] private float difficultySpeedMultiplier = 1.0f;
    [SerializeField] private float itemSpeedMultiplier = 1.0f;

    private void Update()
    {
        /* 최종 속도 = 기본 속도 * 난이도 배율 * 아이템 배율 */
        float appliedSpeed = moveSpeed * difficultySpeedMultiplier * itemSpeedMultiplier;
        transform.Translate(Vector2.left * appliedSpeed * Time.deltaTime);

        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            /* 체력 회복 GameManager 를 거쳐서 처리 */
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnHealItemCollected(healAmount);
            }

            /* 아이템 획득 효과음 */
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play(SoundKey.SFX_ITEM_HEAL);
            }
            
            //Debug.Log("플레이어가 HP 아이템을 획득했습니다!");
            Destroy(gameObject);
        }
    }

    /* 난이도에 따른 기본 속도 배율 설정 from ItemManager */
    public void SetDifficultySpeedMultiplier(float multiplier)
    {
        difficultySpeedMultiplier = multiplier;
    }

    /* 아이템에 의한 일시적 속도 배율 설정 from ItemManager */
    public void SetItemSpeedMultiplier(float multiplier)
    {
        itemSpeedMultiplier = multiplier;
    }
}
