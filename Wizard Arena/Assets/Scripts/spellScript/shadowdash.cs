using UnityEngine;

public class ShadowDodge : MonoBehaviour
{
    public float shadowDashSpeed = 25f;           // Super fast dash speed
    public float shadowDashDuration = 0.35f;      // Longer dash duration
    private float shadowDashTimeCounter;
    private bool isShadowDashing = false;

    private Rigidbody2D rb;
    private PlayerMovmentScript playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovmentScript>();
    }

    void Update()
    {
        if (isShadowDashing)
        {
            shadowDashTimeCounter -= Time.deltaTime;

            // Keep the player moving fast in the direction during the dash
            rb.linearVelocity = new Vector2(playerMovement.direction * shadowDashSpeed, 0f);

            if (shadowDashTimeCounter <= 0f)
            {
                isShadowDashing = false;
                Debug.Log("Shadow Dodge ended");
            }
        }
    }

    /// <summary>
    /// Triggers the shadow dodge manually. Call this from the movement script or input handler.
    /// </summary>
    public void PerformShadowDodge()
    {
        if (isShadowDashing || playerMovement.playerHealthAndMana.IsDead())
            return;

        if (playerMovement.direction != 0f)
        {
            isShadowDashing = true;
            shadowDashTimeCounter = shadowDashDuration;

            // Instantly set velocity (no i-frames, just raw speed)
            rb.linearVelocity = new Vector2(playerMovement.direction * shadowDashSpeed, 0f);

            Debug.Log("Performing Shadow Dodge in direction: " + playerMovement.direction);
        }
    }

    public bool IsShadowDashing()
    {
        return isShadowDashing;
    }
}
