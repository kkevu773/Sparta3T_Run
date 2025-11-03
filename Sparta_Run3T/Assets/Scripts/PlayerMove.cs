using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D bodyCol;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private Sprite runA;
    [SerializeField] private Sprite runB;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite slideSprite;

    [SerializeField] private Sprite doubleJumpSprite;

    [SerializeField] private float runFrameTime = 0.15f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private KeyCode slideKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [SerializeField] private Vector2 standColSize = new Vector2(0.8f, 1.2f);
    [SerializeField] private Vector2 standColOffset = new Vector2(0f, 0f);
    [SerializeField] private Vector2 slideColSize = new Vector2(1.2f, 0.6f);
    [SerializeField] private Vector2 slideColOffset = new Vector2(0f, -0.3f);

    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float spinDuration = 0.5f;
    [SerializeField] private Vector3 slideOffset = new Vector3(0f, -0.2f, 0f);

    /* 낙사 판정용 필드 */
    [Header("낙사 판정")]
    [SerializeField] private float deathY = -5.6f; // 씬에 맞춰 조정


    private bool isGrounded;
    private bool isSliding;
    private int jumpCount;
    private float runTimer;
    private bool runToggle;

    private bool spinActive;
    private float spinTime;
    private float spinStartRZ;
    private bool doubleJumpFX;

    private Vector3 DefaultLocalPos;


    void Update()
    {
        // 바닥
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded && rb.velocity.y <= 0f)
        {
            jumpCount = 0;
            spinActive = false;
            doubleJumpFX = false;
            sr.transform.localEulerAngles = new Vector3(0f, 0f, spinStartRZ);
        }

        HandleInput();
        HandleAnim();
        HandleSpin();

        /* 낙사 판정: 플레이 중일 때만 체크하도록 GameManager 상태 확인 */
        if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.Playing)
        {
            if (transform.position.y <= deathY)
            {
                /* PlayerHp가 있으면 KillByFall 호출 (UI/사운드/게임오버 처리 담당) */
                PlayerHp ph = GetComponent<PlayerHp>();
                if (ph != null)
                {
                    ph.KillByFall();
                }
                else
                {
                    /* PlayerHp가 없으면 GameManager에 직접 알림 */
                    GameManager.Instance.GameOver();
                }
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isSliding)
            TryJump();

        // 슬라이드 시작
        if (isGrounded && Input.GetKeyDown(slideKey))
        {
            isSliding = true;
            ApplySlideCollider(true);
        }

        // 종료
        if (isSliding && Input.GetKeyUp(slideKey))
        {
            isSliding = false;
            ApplySlideCollider(false);
        }
    }

    private void Awake()
    {
        DefaultLocalPos = sr.transform.localPosition;
    }
    private void TryJump()
    {
        if (jumpCount >= maxJumps) return;

        bool isSecondJump = (jumpCount == 1);

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(SoundKey.SFX_PLAYER_JUMP);
        }

        if (isSecondJump)
            StartDoubleJumpFX();

        jumpCount++;
    }

    private void StartDoubleJumpFX()
    {
        // 회전 시작
        spinActive = true;
        spinTime = 0f;
        spinStartRZ = sr.transform.localEulerAngles.z;

        // 스프라이트
        doubleJumpFX = true;
    }

    private void HandleAnim()
    {
        if (doubleJumpFX && doubleJumpSprite != null)
        {
            sr.sprite = doubleJumpSprite;
            return;
        }

        // 공중이면 점프 스프라이트
        if (!isGrounded)
        {
            sr.sprite = jumpSprite;
            return;
        }

        // 슬라이드면 슬라이드 스프라이트
        if (isSliding)
        {
            sr.sprite = slideSprite;
            return;
        }

        runTimer += Time.deltaTime;
        if (runTimer >= runFrameTime)
        {
            runTimer = 0f;
            runToggle = !runToggle;
        }

        sr.sprite = runToggle ? runA : runB;
    }

    private void HandleSpin()
    {
        if (!spinActive) return;

        spinTime += Time.deltaTime;

        float t = Mathf.Clamp01(spinTime / spinDuration);
        float angle = Mathf.Lerp(0f, 360f, t);
        sr.transform.localEulerAngles = new Vector3(0f, 0f, spinStartRZ - angle);

        if (spinTime >= spinDuration)
        {
            spinActive = false;
            sr.transform.localEulerAngles = new Vector3(0f, 0f, spinStartRZ);
        }
    }



    private void ApplySlideCollider(bool slide)
    {
        if (slide)
        {
            bodyCol.size = slideColSize;
            bodyCol.offset = slideColOffset;

            sr.transform.localPosition = DefaultLocalPos + slideOffset;
        }
        else
        {
            bodyCol.size = standColSize;
            bodyCol.offset = standColOffset;

            sr.transform.localPosition = DefaultLocalPos;
        }
    }


    /* 게임 재시작 시 플레이어 초기화 */
    public void ResetPlayer()
    {
        /* 체력 리셋 */
        PlayerHp playerHp = GetComponent<PlayerHp>();
        if (playerHp != null)
        {
            playerHp.ResetHP();
        }

        /* 시작 위치로 이동 (예: x = -6f) */
        transform.position = new Vector3(-6f, transform.position.y, 0f);

        /* 물리 초기화 */
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        /* 상태 초기화 */
        jumpCount = 0;
        isSliding = false;
        spinActive = false;
        doubleJumpFX = false;
        runTimer = 0f;
        runToggle = false;

        /* 콜라이더 원래대로 */
        ApplySlideCollider(false);

        /* 회전 초기화 */
        sr.transform.localEulerAngles = Vector3.zero;
        sr.transform.localPosition = DefaultLocalPos;

        /* 스프라이트 초기화 */
        sr.sprite = runA;
    }

    /* 게임오버 시 플레이어 입력 정지 */
    public void StopPlaying()
    {
        enabled = false;  /* Update 비활성화 */
    }

    /* 게임 시작 시 플레이어 입력 활성화 */
    public void StartPlaying()
    {
        enabled = true;  /* Update 활성화 */
    }
}
