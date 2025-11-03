using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public bool isSpeedUp = true;
    public float moveSpeed = 3f;
    public float effectAmount = 2f;
    public float effectDuration = 5f;

    private void Update()
    {
        transform.Translate(Vector2.left*moveSpeed*Time.deltaTime);

        if (transform.position.x < -10f)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (isSpeedUp)
        {
            GameManager.Instance.OnSpeedUpItemCollected(effectAmount, effectDuration);
            Debug.Log($"속도 증가 효과{effectAmount},{effectDuration}s");
        }
        else
        {
            GameManager.Instance.OnSpeedDownItemCollected(effectAmount, effectDuration);
            Debug.Log($"속도 감소 효과{effectAmount},{effectDuration}s");
        }
            

        Destroy(gameObject);
    }

}
