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
            Debug.Log("플레이어가 HP 아이템을 획득했습니다!");
            Destroy(gameObject);
        }
    }
}
