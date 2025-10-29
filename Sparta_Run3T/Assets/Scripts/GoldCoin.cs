using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    public int pointValue = 10;

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
