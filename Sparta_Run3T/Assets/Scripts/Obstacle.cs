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
        float difficultyFactor = 1f - Mathf.Clamp01(obstacleCount * 0.02f);   // ���̵��� �ö󰥼��� ������ ���ݾ� ���̱�, 0.02f = ��ֹ� 50�� ������ ������ ������ �پ�� (���� ����)

        float holeSize = Random.Range(holeSizeMin, holeSizeMax) * difficultyFactor;   // �⺻ ���� ũ�⿡�� ���̵� ����

        float newX = lastPosition.x + widthPadding + holeSize;   // X ��ġ�� ���� ��ֹ����� ������ ���� �Ÿ���ŭ �̵�

        float newY = Random.Range(lowPosY, highPosY);   // Y ��ġ�� ����

        return new Vector3(newX, newY, 0f);    // ��� ��ȯ
    }
}
