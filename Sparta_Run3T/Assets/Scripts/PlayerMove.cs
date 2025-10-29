using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;        // Player 쪽 Rigidbody2D
    [SerializeField] private BoxCollider2D bodyCol; // Player 쪽 BoxCollider2D
    [SerializeField] private SpriteRenderer sr;     // MainSprite 쪽 SpriteRenderer
    [SerializeField] private Transform groundCheck; // GroundCheck 트랜스폼

    [Header("Sprites")]
    [SerializeField] private Sprite runA;
    [SerializeField] private Sprite runB;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite slideSprite;

    [Header("Move / Jump / Slide")]
    [SerializeField] private float runFrameTime = 0.15f; // 달리기 깜빡임 속도
    [SerializeField] private float jumpForce = 8f;       // 점프 힘
    [SerializeField] private int maxJumps = 2;           // 2면 더블 점프
    [SerializeField] private KeyCode slideKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;


    [Header("Collider Shapes")]
    [SerializeField] private Vector2 standColSize = new Vector2(0.8f, 1.2f);
    [SerializeField] private Vector2 standColOffset = new Vector2(0f, 0f);
    [SerializeField] private Vector2 slideColSize = new Vector2(1.2f, 0.6f);
    [SerializeField] private Vector2 slideColOffset = new Vector2(0f, -0.3f);

    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    // 내부 상태
    private bool isGrounded;
    private bool isSliding;
    private int jumpCount;
    private float runTimer;
    private bool runToggle; // false -> runA, true -> runB

    void Update()
    {
        // 바닥
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // 점프 카운트 리셋
        if (isGrounded && rb.velocity.y <= 0f)
            jumpCount = 0;

        HandleInput();
        HandleAnim();
    }

    private void HandleInput()
    {
        // 점프
        if (Input.GetButtonDown("Jump") && !isSliding)
            TryJump();

        // 슬라이드 시작
        if (isGrounded && Input.GetKeyDown(slideKey))
        {
            isSliding = true;
            ApplySlideCollider(true);
        }

        // 슬라이드 종료
        if (isSliding && Input.GetKeyUp(slideKey))
        {
            isSliding = false;
            ApplySlideCollider(false);
        }
    }

    private void TryJump()
    {
        if (jumpCount >= maxJumps) return;

        // 위로 튈 때 Y속도 초기화해서 항상 같은 힘으로 점프
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpCount++;
    }

    private void HandleAnim()
    {
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

        // 기본 달리기 A/B 토글
        runTimer += Time.deltaTime;
        if (runTimer >= runFrameTime)
        {
            runTimer = 0f;
            runToggle = !runToggle;
        }

        sr.sprite = runToggle ? runA : runB;
    }

    private void ApplySlideCollider(bool slide)
    {
        if (slide)
        {
            bodyCol.size = slideColSize;
            bodyCol.offset = slideColOffset;
        }
        else
        {
            bodyCol.size = standColSize;
            bodyCol.offset = standColOffset;
        }
    }

    private void OnDrawGizmosSelected() // 디버그용
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}

