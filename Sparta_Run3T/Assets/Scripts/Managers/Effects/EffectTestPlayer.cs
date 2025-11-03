using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJumpTest : MonoBehaviour
{
    [Header("점프 설정")]
    public float jumpForce = 8f;
    public int maxJumpCount = 2;

    [Header("바닥 감지")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;

    private Rigidbody2D rb;
    private int jumpCount = 0;
    private bool isGrounded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (UIManager.Instance != null && UIManager.Instance.IsPaused)
            return;
        CheckGround();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }
    }

    private void CheckGround()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, ~0);

        bool grounded = false;

        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                grounded = true;
                break;
            }
        }
        if (grounded && !isGrounded)
        {
            jumpCount = 0;
        }

        isGrounded = grounded;
    }

    private void TryJump()
    {
        if (jumpCount >= maxJumpCount)
            return;

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (jumpCount == 0)
        {
            EffectManager.Instance.Play(EffectKey.PLAYER_JUMP, transform.position);
        }
        else if (jumpCount == 1)
        {
            EffectManager.Instance.Play(EffectKey.PLAYER_DOUBLEJUMP, transform.position);
        }

        jumpCount++;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
