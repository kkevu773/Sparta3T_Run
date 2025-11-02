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

    private void Update()
    {
        /* canMove == false => 바로 return */
        if (!canMove) return;

        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

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
}
