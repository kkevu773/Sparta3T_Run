using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Header("이동 속도 및 삭제 지점")]
    public float moveSpeed = 5f;     // 왼쪽으로 이동 속도
    public float destroyX = -10f;    // 이보다 왼쪽이면 삭제

    void Update()
    {
        // 왼쪽으로 이동
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 화면 왼쪽 밖으로 나가면 삭제
        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
}

