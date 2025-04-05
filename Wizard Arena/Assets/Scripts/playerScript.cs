using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Gravity parameters
    public float fallGravityMult = 2.0f; // Fall gravity multiplier
    public float maxFallSpeed = -10.0f; // Maximum fall speed
    public float gravityScale = 1.0f; // Declare gravityScale variable

    // Speed parameters
    public float speed = 5f; // Declare speed variable

    // Jump parameters
    public float jumpForce = 10f; // Base jump force
    public float jumpSpeed = 8f; // Declare jumpSpeed variable
    public float maxJumpForce = 15f; // Maximum jump force (clamp to this value)

    // Other
    private float direction = 0f; // Declare direction variable
    private Rigidbody2D rb; //Declare Rigidbody as rb variable
    private bool jumpReleased = false; // Track whether the jump button is released

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody 
    }

    // Update is called once per frame
    void Update()
    {
        HandleJump();
        HandleFall();

        direction = Input.GetAxis("Horizontal"); // Sets the direction variable to A and D keys

        // Handle horizontal movement
        if (direction != 0f)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y); // Corrected to 'velocity'
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Corrected to 'velocity'
        }

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
            SetGravityScale(gravityScale * fallGravityMult);

            // Cap the maximum fall speed
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, maxFallSpeed));
        }
        else
        {
            // Reset gravity to normal if not falling
            rb.gravityScale = gravityScale;
        }
    }

    // Handle jumping logic
    private void HandleJump()
    {
        if (jumpReleased && rb.linearVelocity.y > 0) // Only stop upward motion if jump button is released
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Stop upward velocity
        }
    }

    // Handle falling logic
    private void HandleFall()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Apply higher gravity multiplier if falling
            SetGravityScale(gravityScale * fallGravityMult);

            // Cap the maximum fall speed
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, maxFallSpeed));
        }
        else
        {
            // Reset gravity to normal if not falling
            rb.gravityScale = gravityScale;
        }
    }

    // A function to set gravity scale for the rigidbody
    void SetGravityScale(float newGravityScale)
    {
        rb.gravityScale = newGravityScale;
    }
}
