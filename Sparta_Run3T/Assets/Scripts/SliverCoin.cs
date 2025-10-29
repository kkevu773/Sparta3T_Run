using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliverCoin : MonoBehaviour
{
    public int pointValue = 5;

    public void Collect()
    {
        ScoreManager.Instance.AddScore(pointValue);

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }
}
