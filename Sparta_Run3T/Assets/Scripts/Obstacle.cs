using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float highPosY = 1f;
    public float lowPosY = -1f;

    public float holeSizeMin = 1f;
    public float holeSizeMax = 3f;

    public Transform sawObject;
    public Transform rockObject;
    public Transform block_spikesObject;

    public float widthPadding = 4f;

    public Vector3 SetRandomPlace(Vector3 lastPosition, int obstacleCount)
    {
        float difficultyFactor = 1f - Mathf.Clamp01(obstacleCount * 0.02f);   // 난이도가 올라갈수록 간격을 조금씩 줄이기, 0.02f = 장애물 50개 나오면 간격이 절반쯤 줄어듦 (조절 가능)

        float holeSize = Random.Range(holeSizeMin, holeSizeMax) * difficultyFactor;   // 기본 구멍 크기에서 난이도 적용

        float newX = lastPosition.x + widthPadding + holeSize;   // X 위치는 이전 장애물보다 앞으로 일정 거리만큼 이동

        float newY = Random.Range(lowPosY, highPosY);   // Y 위치는 랜덤

        return new Vector3(newX, newY, 0f);    // 결과 반환
    }
}
