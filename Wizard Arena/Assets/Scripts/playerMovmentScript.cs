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
    public float jumpSpeed = 10f; // Jump speed (initial upward velocity)
    public float maxJumpForce = 15f; // Maximum jump force (cap this value)

    // Wallsliding parameters
    public float wallSlideSpeed = 2f;  // Speed at which the player slides down the wall
    public float wallJumpForce = 10f;  // Force applied when jumping off the wall
    public float wallJumpTime = 0.2f; // Time to allow jumping off the wall after pressing jump

    // Walljumping parameters
    public bool isWallJumping;
    public float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // Other variables
    private float direction = 0f;   // Horizontal direction
    private bool isWallSliding = false; // Is the player sliding on the wall
    private bool isTouchingWall = false; // Is the player touching a wall
    private Rigidbody2D rb; // Rigidbody2D component
    private bool jumpReleased = false; // Tracks when the jump button is released

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update called");

        HandleWallSliding();
        HandleWallJumping();

        if (!isWallJumping)
        {
            HandleFlip();   // Handle character flipping
        }

        HandleMovement();  // Calling the newly created HandleMovement method

        direction = Input.GetAxis("Horizontal"); // Sets the direction variable to A and D keys

        HandleFall();   // Handle character falling
        HandleFlip();   // Handle character flipping

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

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
    }

    private void HandleWallSliding()
    {
        float rayLength = 1.5f; // Ray length

        Vector2 rayDirection = transform.right; // This gives us the right direction of the character (relative to its rotation)

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

        // If the player is touching a wall and not grounded, allow wall sliding
        if (isTouchingWall && !IsGrounded())
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);  // Apply wall sliding speed (downwards)
            Debug.Log("Wall sliding started.");
        }
        else
        {
            // If the player is grounded or not touching the wall, stop wall sliding
            isWallSliding = false;
            Debug.Log("Wall sliding stopped.");
        }
    }

    private void HandleWallJumping()
    {
        // If the player is wall sliding, prepare for the wall jump
        if (isWallSliding)
        {
            wallJumpingDirection = -Mathf.Sign(transform.position.x); // Get direction from wall
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping)); // Cancel any ongoing wall jump timeout
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime; // Reduce counter for wall jumping time
        }

        // Wall jump activation when the player presses the jump button and we're in the wall jump window
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            
            // Apply a force in the direction of the wall jump
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);

            // Reset the wall jump time counter
            wallJumpingCounter = 0f;

            // Rotate the character to face the opposite direction after the jump
            float targetRotation = wallJumpingDirection > 0 ? 0f : 180f; // Flip based on direction of jump
            transform.rotation = Quaternion.Euler(0f, targetRotation, 0f); // Apply rotation

            // Invoke the stop of the wall jump after a short delay
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    // Stops wall jumping after a certain duration
    private void StopWallJumping()
    {
        isWallJumping = false;
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

    // Flip the character based on movement direction
    private void HandleFlip()
    {
        if (direction != 0f && !isWallSliding)
        {
            // Rotate the character on the Y-axis to flip it
            float targetRotation = direction > 0 ? 0f : 180f; // Flip 180 degrees when moving left
            transform.rotation = Quaternion.Euler(0f, targetRotation, 0f); // Apply the rotation
        }
    }

    private bool IsGrounded()
    {
        // Cast a ray downwards to check if the player is grounded with a ray length of 1
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.25f, LayerMask.GetMask("Ground"));

        // Debug the ray to visualize the ground check with a length of 1
        Debug.DrawRay(transform.position, Vector2.down * 2.25f, Color.green, 0.2f);  // Green ray with length 1 for 0.2 seconds

        // Log the result of the raycast check
        if (hit.collider != null)
        {
            Debug.Log("Grounded: Raycast hit something.");
        }
        else
        {
            Debug.Log("Grounded: Raycast hit nothing.");
        }

        // Return true if the ray hits something (i.e., the player is grounded)
        return hit.collider != null;
    }
}
