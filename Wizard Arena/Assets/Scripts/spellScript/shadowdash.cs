using UnityEngine;

public class ShadowDodge : MonoBehaviour
{
    public float shadowDashSpeed = 35f;           // Super fast dash speed
    public float shadowDashDuration = .3f;      // Longer dash duration
    private float shadowDashTimeCounter;
    private bool isShadowDashing = false;
    public float sDashCooldownTime = 3f;  // Cooldown time for iceshard
    private float lastAttackTime = 0f;

    private Rigidbody2D rb;
    private PlayerMovmentScript playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovmentScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !playerMovement.isWallJumping && playerMovement.direction != 0)
        {
            // Check if the cooldown has passed
            if (Time.time - lastAttackTime >= sDashCooldownTime)
            {
                HealthAndMana statScript = GetComponent<HealthAndMana>();

                if (statScript.currentMana + statScript.currentHealth > 13)
                {
                    isShadowDashing = true;
                    shadowDashTimeCounter = shadowDashDuration;

                    rb.linearVelocity = new Vector2(playerMovement.direction * shadowDashSpeed, rb.linearVelocity.y);
                    Debug.Log("shadow dashing with direction: " + playerMovement.direction);
                    statScript.currentMana -= 13;
                    // Now we update the time only when dodge is triggered
                    lastAttackTime = Time.time;
                }
                else
                {
                    Debug.Log("Not enough mana for shadowdash");
                }
            
            }
            else
            {
                Debug.Log("shadow dash is on cooldown.");
            }
        }


        if (isShadowDashing)
        {
            shadowDashTimeCounter -= Time.deltaTime;

            // Keep the player moving fast in the direction during the dash
            rb.linearVelocity = new Vector2(playerMovement.direction * shadowDashSpeed, 0f);

            if (shadowDashTimeCounter <= 0f)
            {
                isShadowDashing = false;
                Debug.Log("Shadow dash ended");
            }
        }
    }

    /// <summary>
    /// Triggers the shadow dodge manually. Call this from the movement script or input handler.
    /// </summary>

    public bool IsShadowDashing()
    {
        return isShadowDashing;
    }
}
