using UnityEngine;

public class PlayerScript : MonoBehaviour
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
    public float dodgeSpeed = 15f; // Dodge movement speed
    public float dodgeDuration = 0.2f; // Duration of the dodge
    public float invincibilityDuration = 0.5f; // Duration of invincibility after dodge
    private bool isDodging = false; // Is the player dodging
    private bool isInvincible = false; // Is the player invincible
    private float dodgeTimeCounter = 0f; // Timer for dodge duration
    private float invincibilityTimeCounter = 0f; // Timer for invincibility duration

    // Other variables
    private float direction = 0f;   
    private bool isWallSliding = false; 
    private bool isTouchingWall = false; 
    private Rigidbody2D rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody
    }

    void Update()
    {
        HandleWallSliding();
        HandleWallJumping();
        HandleMovement();

        // Handle Dodge
        if (Input.GetKeyDown(KeyCode.S) && !isWallJumping) // Press S to dodge
        {
            if (direction != 0) // If moving horizontally, do a dash in that direction
            {
                isDodging = true;
                dodgeTimeCounter = dodgeDuration;
                isInvincible = true;
                invincibilityTimeCounter = invincibilityDuration;
                rb.linearVelocity = new Vector2(direction * dodgeSpeed, rb.linearVelocity.y); // Dash in the direction of movement
                Debug.Log("Dodging with direction: " + direction);
            }
            else // If standing still, dodge in place
            {
                isDodging = true;
                dodgeTimeCounter = dodgeDuration;
                isInvincible = true;
                invincibilityTimeCounter = invincibilityDuration;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stay in place while dodging
                Debug.Log("Dodging in place");
            }
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

        // Handle invincibility
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

        // Update camera position
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.x = transform.position.x;
        cameraPos.y = transform.position.y;
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
        if (!isWallJumping && !isDodging)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
    }

    private void HandleWallSliding()
    {
        float rayLength = 1.5f; 
        Vector2 rayDirection = transform.right; 

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayLength, LayerMask.GetMask("Wall"));
        isTouchingWall = hit.collider != null;

        if (isTouchingWall && !IsGrounded())
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

        if (direction != 0f && !isWallSliding && !isDodging)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);  
        }
        else if (!isWallSliding && !isDodging)
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.25f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
}
