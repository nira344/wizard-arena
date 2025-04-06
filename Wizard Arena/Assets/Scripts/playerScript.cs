using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Gravity parameters
    public float fallGravityMult = 2.0f; // Fall gravity multiplier
    public float maxFallSpeed = -10.0f; // Maximum fall speed
    public float gravityScale = 1.0f; // Gravity scale

    // Speed parameters
    public float speed = 5f; // Horizontal movement speed

    // Jump parameters
    public float jumpForce = 10f; // Base jump force
    public float jumpSpeed = 8f; // Jump speed (initial upward velocity)
    public float maxJumpForce = 15f; // Maximum jump force (cap this value)

    // Wall sliding parameters
    public float wallSlideSpeed = 2f;  // Speed at which the player slides down the wall
    public float wallJumpForce = 10f;  // Force applied when jumping off the wall
    public float wallJumpTime = 0.2f; // Time to allow jumping off the wall after pressing jump

    // Other variables
    private float direction = 0f;   // Horizontal direction
    private bool isWallSliding = false; // Is the player sliding on the wall
    private bool isTouchingWall = false; // Is the player touching a wall
    private bool canWallJump = false;  // Can the player wall jump
    private Rigidbody2D rb; // Rigidbody2D component
    private float wallJumpTimeCounter = 0f; // Timer for wall jump time
    private bool jumpReleased = false; // Tracks when the jump button is released

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        HandleWallSliding();
        HandleWallJumping();
        HandleMovement();  // Calling the newly created HandleMovement method

        direction = Input.GetAxis("Horizontal"); // Sets the direction variable to A and D keys

        // Apply gravity logic for falling
        HandleFall();

        // Handle jumping
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.linearVelocity.y) < 0.001f) // Check if the player is grounded
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed); // Corrected to 'velocity'
        }

        jumpReleased = Input.GetButtonUp("Jump"); // Tracks when the jump button is released

        // Build new camera position (X and Y changes only)
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.x = transform.position.x;
        cameraPos.y = transform.position.y;

        // Update camera position
        Camera.main.transform.position = cameraPos;

        // Check if velocity is negative (falling)
        if (rb.linearVelocity.y < 0)
        {
            // Apply a higher gravity multiplier when falling
            rb.gravityScale = gravityScale * fallGravityMult;

            // Cap the maximum fall speed
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, maxFallSpeed));
        }
        else
        {
            // Reset gravity to normal if not falling
            rb.gravityScale = gravityScale;
        }
    }

    // Wall sliding logic
    private void HandleWallSliding()
    {
        float rayLength = 1.5f; // Increased the ray length a bit

        Vector2 rayDirection = Vector2.zero;
        if (transform.localScale.x > 0)
        {
            rayDirection = Vector2.right; // Player is facing right, shoot ray to the right
        }
        else
        {
            rayDirection = Vector2.left; // Player is facing left, shoot ray to the left
        }

        // Use Physics2D.Raycast to check for wall collision
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayLength, LayerMask.GetMask("Wall"));

        // Debug the ray to see where it's casting
        Debug.DrawRay(transform.position, rayDirection * rayLength, Color.red, 0.1f);

        // Detect if the player is touching a wall (left or right)
        isTouchingWall = hit.collider != null;

        // Debugging: Log if the wall is being detected
        if (isTouchingWall)
        {
            Debug.Log("Touching wall on side: " + (rayDirection == Vector2.right ? "Right" : "Left"));
        }
        else
        {
            Debug.Log("Not touching wall.");
        }

        // If the player is touching a wall and not grounded
        if (isTouchingWall && Mathf.Abs(rb.linearVelocity.y) < 0.1f && !IsGrounded())
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);  // Apply wall sliding speed (downwards)
            Debug.Log("Wall sliding started.");
        }
        else
        {
            isWallSliding = false;
        }
    }


    // Wall jumping logic
    private void HandleWallJumping()
    {
        // Check if player is in wall sliding state and press jump
        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            canWallJump = true;
            wallJumpTimeCounter = wallJumpTime;  // Allow wall jump for a short period
            Debug.Log("Wall jump initiated");
        }

        if (canWallJump)
        {
            // Apply force to jump off the wall (in the opposite direction)
            float wallJumpDirection = Mathf.Sign(transform.localScale.x); // Right if on left wall, left if on right wall
            rb.linearVelocity = new Vector2(-wallJumpDirection * wallJumpForce, jumpSpeed); // Jump in the opposite direction
            canWallJump = false;  // Disable wall jump after it's performed
            Debug.Log("Wall jump performed");
        }

        if (wallJumpTimeCounter > 0)
        {
            wallJumpTimeCounter -= Time.deltaTime;
        }
    }

    // Handle normal jumping logic
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);  // Apply jump force
        }
    }

    // Handle falling logic
    private void HandleFall()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Apply higher gravity multiplier if falling
            rb.gravityScale = gravityScale * fallGravityMult;

            // Cap the maximum fall speed
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, maxFallSpeed));
        }
        else
        {
            // Reset gravity to normal if not falling
            rb.gravityScale = gravityScale;
        }
    }

    // Handle horizontal movement
    private void HandleMovement()
    {
        direction = Input.GetAxis("Horizontal");  // Get horizontal input

        // Handle horizontal movement
        if (direction != 0f && !isWallSliding)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);  // Move player left/right
        }
        else if (!isWallSliding)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);  // Stop horizontal movement if not pressing keys
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
}

