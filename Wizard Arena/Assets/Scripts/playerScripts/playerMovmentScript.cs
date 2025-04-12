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
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;
    private bool isTouchingWall = false;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public HealthAndMana playerHealthAndMana;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        playerHealthAndMana = GetComponent<HealthAndMana>();
    }

    void Update()
    {
        if (playerHealthAndMana.IsDead())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        CheckWallContacts();

        if (wallSlideLockTimer > 0f)
        {
            wallSlideLockTimer -= Time.deltaTime;
        }

        HandleWallSliding();
        HandleWallJumping();
        HandleMovement();

        HandleDodge();
        HandleFall();
        HandleFlip();
        HandleJump();
        UpdateCameraPosition();
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

    private void UpdateCameraPosition()
    {
        float verticalOffset = 4f;
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.x = transform.position.x;
        cameraPos.y = transform.position.y + verticalOffset;
        Camera.main.transform.position = cameraPos;
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
            LayerMask.GetMask("Ground")
        );

        Color rayColor = hit.collider != null ? Color.green : Color.red;
        Debug.DrawRay(coll.bounds.center + new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(coll.bounds.center, Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);

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
