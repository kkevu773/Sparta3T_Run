using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    public int pointValue = 10;
    public float moveSpeed = 5f;
    public float destroyX = -10f;

    private void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
    public void Collect()
    {
        ScoreManager.Instance.AddScore(pointValue);   //점수 추가

        Destroy(gameObject);   //코인 제거

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }
}
