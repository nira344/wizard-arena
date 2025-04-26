using UnityEngine;

public class PlayerMovmentScript : MonoBehaviour
{
    // Gravity parameters
    public float fallGravityMult = 2.0f;
    public float maxFallSpeed = -10.0f;
    public float gravityScale = 1.0f;

    // Speed parameters
    public float speed = 5f;

    // Jump parameters
    public float jumpSpeed = 10f;

    // Wall sliding parameters
    public float wallSlideSpeed = 2f;

    // Wall jumping parameters
    public bool isWallJumping;
    public float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);
    private float wallSlideLockTimer = 0f;
    public float wallSlideLockDuration = 0.2f;

    // Dodge parameters
    public float dodgeCooldownTime = 1.0f;
    public float dodgeSpeed = 15f;
    public float dodgeDuration = 0.2f;
    public float invincibilityDuration = 0.3f;
    private bool isDodging = false;
    [HideInInspector] public bool isInvincible = false;
    private float dodgeTimeCounter = 0f;
    private float invincibilityTimeCounter = 0f;

    // Other variables
    private float lastAttackTime = 0f;
    public float direction = 0f;
    private bool isWallSliding = false;
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;
    private bool isTouchingWall = false;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer spriteRenderer;
    public HealthAndMana playerHealthAndMana;

    public AudioSource footstep;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealthAndMana = GetComponent<HealthAndMana>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerHealthAndMana.IsDead())
        {
            rb.linearVelocity = Vector2.zero;
            SetAnimationState(4); // Dead
            return;
        }

        CheckWallContacts();
        wallSlideLockTimer -= Time.deltaTime;

        HandleWallSliding();
        HandleWallJumping();
        HandleMovement();
        HandleDodge();
        HandleFall();
        HandleFlip();
        HandleJump();
        UpdateInvincibilityVisual();
        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        if (!isWallJumping && !isDodging && !GetComponent<ShadowDodge>().IsShadowDashing())
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
    }

    private void CheckWallContacts()
    {
        float rayLength = 0.9f;

        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, rayLength, LayerMask.GetMask("Wall"));
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, rayLength, LayerMask.GetMask("Wall"));

        isTouchingLeftWall = leftHit.collider != null;
        isTouchingRightWall = rightHit.collider != null;
        isTouchingWall = isTouchingLeftWall || isTouchingRightWall;

        Debug.DrawRay(transform.position, Vector2.left * rayLength, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * rayLength, Color.red);
    }

    private void HandleWallSliding()
    {
        bool facingWall = (isTouchingLeftWall && direction < 0) || (isTouchingRightWall && direction > 0);

        if (isTouchingWall && !IsGrounded() && wallSlideLockTimer <= 0f && facingWall)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
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
            if (isTouchingLeftWall) wallJumpingDirection = 1f;
            if (isTouchingRightWall) wallJumpingDirection = -1f;

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

            wallSlideLockTimer = wallSlideLockDuration;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
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
        if (Input.GetAxis("Horizontal") != 0 && !footstep.isPlaying && IsGrounded())
        {
            footstep.Play();
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

    private void HandleDodge()
    {
        if (Time.time - lastAttackTime >= dodgeCooldownTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                isDodging = true;
                dodgeTimeCounter = dodgeDuration;
                isInvincible = true;
                invincibilityTimeCounter = invincibilityDuration;

                rb.linearVelocity = new Vector2(direction * dodgeSpeed, rb.linearVelocity.y);
                lastAttackTime = Time.time;
            }
        }

        if (isDodging)
        {
            dodgeTimeCounter -= Time.deltaTime;
            if (dodgeTimeCounter <= 0f)
            {
                isDodging = false;
            }
        }

        if (isInvincible)
        {
            invincibilityTimeCounter -= Time.deltaTime;
            if (invincibilityTimeCounter <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(
            coll.bounds.center,
            coll.bounds.size,
            0f,
            Vector2.down,
            extraHeight,
            LayerMask.GetMask("Ground", "Default", "Enemy")
        );

        return hit.collider != null;
    }

    private void UpdateInvincibilityVisual()
    {
        if (spriteRenderer == null) return;

        if (isInvincible)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // transparent
        }
        else
        {
            spriteRenderer.color = Color.white; // normal
        }
    }

    private void UpdateAnimationState()
    {
        if (playerHealthAndMana.IsDead())
        {
            animator.SetInteger("State", 4); // Dead
        }
        else if (isDodging)
        {
            animator.SetInteger("State", 3); // Dash
        }
        else if (!IsGrounded() && rb.linearVelocity.y > 0.1f)
        {
            animator.SetInteger("State", 2); // Jumping
        }
        else if (!IsGrounded() && rb.linearVelocity.y < -0.1f)
        {
            animator.SetInteger("State", 5); // Falling
        }
        else if (Mathf.Abs(direction) > 0.1f)
        {
            animator.SetInteger("State", 1); // Walking
        }
        else
        {
            animator.SetInteger("State", 0); // Idle
        }
    }

    private void SetAnimationState(int state)
    {
        if (animator != null)
        {
            animator.SetInteger("State", state);
        }
    }
}