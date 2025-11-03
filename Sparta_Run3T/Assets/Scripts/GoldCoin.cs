using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    public int pointValue = 10;
    public float moveSpeed = 5f;
    public float destroyX = -10f;

    /* 코인 이동/정지 구분 */
    private bool canMove = true;

    /* 난이도, 아이템별 속도 조절용 */
    [Header("Speed Settings")]
    [SerializeField] private float difficultySpeedMultiplier = 1.0f;
    [SerializeField] private float itemSpeedMultiplier = 1.0f;

    private void Update()
    {
        /* canMove == false => 바로 return */
        if (!canMove) return;

        /* 최종 속도 = 기본 속도 * 난이도 배율 * 아이템 배율 */
        float appliedSpeed = moveSpeed * difficultySpeedMultiplier * itemSpeedMultiplier;

        transform.Translate(Vector3.left * appliedSpeed * Time.deltaTime);

        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
    public void Collect()
    {
        /* ScoreManager.Instance.AddScore(pointValue);   //점수 추가 */

        /* GameManager를 통해 점수 추가 (UI 업데이트까지 함께 됨) */
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(pointValue);
        }

        /* 코인 획득 사운드 */
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayOneShot(SoundKey.SFX_ITEM_COIN);
        }

        Destroy(gameObject);   //코인 제거

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }



    /* 코인 이동 시작 */
    public void StartMoving()
    {
        canMove = true;
    }

    /* 코인 이동 정지 */
    public void StopMoving()
    {
        canMove = false;
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
