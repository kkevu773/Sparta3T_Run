using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int healAmount = 10;

    private void Update()
    {
        transform.Translate(Vector2.left*moveSpeed*Time.deltaTime);

        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 체력 회복 GameManager 를 거쳐서 처리
            if (GameManager.Instance != null)
            {
                //GameManager.Instance.OnHpItemCollected(healAmount);
            }

            // 아이템 획득 효과음
            if (AudioManager.instance != null)
            {
                AudioManager.instance.Play(SoundKey.SFX_ITEM_PICKUP);
            }
            
            Debug.Log("플레이어가 HP 아이템을 획득했습니다!");
            Destroy(gameObject);
        }
    }
}
