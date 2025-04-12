using UnityEngine;
using UnityEngine.UI;

public class PlayerMovmentScript : MonoBehaviour
{
    // Gravity parameters
    public float fallGravityMult = 2.0f;
    public float maxFallSpeed = -10.0f;
    public float gravityScale = 1.0f;

    // Speed parameters
    public float speed = 5f;

    // Jump parameters
    public float jumpForce = 10f;
    public float jumpSpeed = 10f;
    public float maxJumpForce = 15f;

    // Wall sliding parameters
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 10f;
    public float wallJumpTime = 0.2f;

    // Wall jumping parameters
    public bool isWallJumping;
    public float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // Dodge parameters
    public float dodgeCooldownTime = 0.2f;
    public float dodgeSpeed = 15f;
    public float dodgeDuration = 0.2f;
    public float invincibilityDuration = 0.3f;
    private bool isDodging = false;
    private bool isInvincible = false;
    private float dodgeTimeCounter = 0f;
    private float invincibilityTimeCounter = 0f;

    // Other variables
    private float lastAttackTime = 0f;
    public float direction = 0f;
    private bool isWallSliding = false;
    private bool isTouchingWall = false;
    private Rigidbody2D rb;
    public HealthAndMana playerHealthAndMana;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealthAndMana = GetComponent<HealthAndMana>();
    }

    void Update()
    {
        if (playerHealthAndMana.IsDead())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        HandleWallSliding();
        HandleWallJumping();
        HandleMovement();

        if (Time.time - lastAttackTime >= dodgeCooldownTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isWallJumping)
            {
                isDodging = true;
                dodgeTimeCounter = dodgeDuration;
                isInvincible = true;
                invincibilityTimeCounter = invincibilityDuration;

                if (direction != 0)
                {
                    rb.linearVelocity = new Vector2(direction * dodgeSpeed, rb.linearVelocity.y);
                    Debug.Log("Dodging with direction: " + direction);
                }
                else
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                    Debug.Log("Dodging in place");
                }

                lastAttackTime = Time.time;
            }
        }
        else
        {
            Debug.Log("Dodge is on cooldown.");
        }

        if (isDodging)
        {
            dodgeTimeCounter -= Time.deltaTime;
            if (dodgeTimeCounter <= 0f)
            {
                isDodging = false;
                Debug.Log("Dodge Ended");
            }
        }

        if (isInvincible)
        {
            invincibilityTimeCounter -= Time.deltaTime;
            Debug.Log("Invincibility Time Left: " + invincibilityTimeCounter);
            if (invincibilityTimeCounter <= 0f)
            {
                isInvincible = false;
                Debug.Log("Invincibility Ended");
            }
        }

        HandleFall();
        HandleFlip();
        HandleJump();

        // Update camera position with vertical offset
        float verticalOffset = 4f;
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.x = transform.position.x;
        cameraPos.y = transform.position.y + verticalOffset;
        Camera.main.transform.position = cameraPos;

        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, maxFallSpeed));
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping && !isDodging && !GetComponent<ShadowDodge>().IsShadowDashing())
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
    }

    private void HandleWallSliding()
    {
        float rayLength = 1f;
        Vector2 rayDirection = transform.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayLength, LayerMask.GetMask("Wall"));

        // Debug Ray for wall check
        Debug.DrawRay(transform.position, rayDirection * rayLength, Color.red);

        if (hit.collider != null)
        {
            isTouchingWall = true;
            Debug.Log("Wall detected: " + hit.collider.name);
        }
        else
        {
            isTouchingWall = false;
        }

        if (isTouchingWall && !IsGrounded())
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
            Debug.Log("Wall sliding");
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void HandleWallJumping()
    {
        if (isWallSliding)
        {
            wallJumpingDirection = -Mathf.Sign(transform.position.x);
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            float targetRotation = wallJumpingDirection > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, targetRotation, 0f);

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
            Debug.Log("Wall Jumped");
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
        Debug.Log("Stopped Wall Jumping");
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
            Debug.Log("Jumped");
        }
    }

    private void HandleFall()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, maxFallSpeed));
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void HandleMovement()
    {
        direction = Input.GetAxis("Horizontal");

        if (direction != 0f && !isWallSliding && !isDodging && !GetComponent<ShadowDodge>().IsShadowDashing())
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
        else if (!isWallSliding && !isDodging && !GetComponent<ShadowDodge>().IsShadowDashing())
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void HandleFlip()
    {
        if (direction != 0f && !isWallSliding && !isDodging)
        {
            float targetRotation = direction > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, targetRotation, 0f);
        }
    }

    private bool IsGrounded()
    {
        float rayLength = 2.25f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, LayerMask.GetMask("Ground"));

        // Debug Ray for ground check
        Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.green);

        if (hit.collider != null)
        {
            Debug.Log("Grounded on: " + hit.collider.name);
            return true;
        }
        else
        {
            Debug.Log("Not grounded");
            return false;
        }
    }
}
