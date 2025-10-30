using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliverCoin : MonoBehaviour
{
    public int pointValue = 5;
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
