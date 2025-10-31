using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    [SerializeField] int maxHp = 3;
    [SerializeField] int damageOnHit = 1;
    [SerializeField] string Obstacle = "Obstacle";

    [Header("무적 시간 설정")]
    [SerializeField] private float invincibleTime = 1f;  /* 피격 후 무적 시간 */
    private bool isInvincible = false;

    public int MaxHp => maxHp;
    public int Hp { get; private set; }

    void Awake() { 
        Hp = maxHp;

        /* UI 업데이트 (초기 체력) */
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHP(Hp, maxHp);
        }
    }

    public void TakeHit(int dmg)
    {
        /* 무적 상태면 데미지 안 받음 */
        if (isInvincible) return;

        Hp = Mathf.Max(0, Hp - Mathf.Max(0, dmg));

        /* UI 업데이트 */
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHP(Hp, maxHp);
        }

        /* 피격 사운드 재생 */
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(SoundKey.SFX_OBSTACLE_HIT);
        }

        if (Hp <= 0) 
        { /* 죽음 처리 */
            Die();
        }
        else
        {
            /* 무적 시간 시작 */
            StartCoroutine(InvincibleCoroutine());
        }
    }

    void OnTriggerEnter2D(Collider2D other)  //부딪히면 깎기
    {
        if (other.CompareTag(Obstacle)) TakeHit(damageOnHit);
    }



    /* 체력 리셋 (게임 재시작 시) */
    public void ResetHP()
    {
        Hp = maxHp;
        isInvincible = false;

        /* 스프라이트 보이게 (무적 깜빡임 중이었을 수도 있으니) */
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = true;
        }

        /* UI 업데이트 */
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHP(Hp, maxHp);
        }

        Debug.Log("플레이어 체력 리셋!");
    }

    /* 플레이어 사망 처리 */
    private void Die()
    {
        Debug.Log("플레이어 사망!");

        /* GameManager에게 게임오버 알림 */
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        /* 플레이어 입력 비활성화 */
        PlayerMove playerMove = GetComponent<PlayerMove>();
        if (playerMove != null)
        {
            playerMove.StopPlaying();
        }
    }

    /* 피격 후 무적 시간 코루틴 */
    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        /* 깜빡이는 효과 (선택사항) */
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            float blinkTime = 0.1f;
            int blinkCount = Mathf.FloorToInt(invincibleTime / (blinkTime * 2));

            for (int i = 0; i < blinkCount; i++)
            {
                sr.enabled = false;
                yield return new WaitForSeconds(blinkTime);
                sr.enabled = true;
                yield return new WaitForSeconds(blinkTime);
            }
        }
        else
        {
            yield return new WaitForSeconds(invincibleTime);
        }

        isInvincible = false;
    }

    /* 외부 (PlayerMove.cs) 에서 낙사로 즉시 사망시키기 위한 메서드 */
    public void KillByFall()
    {
        /* 이미 죽었거나 무적 같은 상태에서 중복 호출 차단 */
        if (Hp <= 0) return;

        /* 즉시 HP를 0으로 만들고 UI 갱신 */
        Hp = 0;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHP(Hp, maxHp);
        }

        /* 사망 처리 */
        Die();
    }

}
