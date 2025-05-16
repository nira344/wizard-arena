using UnityEngine;

public class ShadowDodge : MonoBehaviour
{
    public float shadowDashSpeed = 35f;
    public float shadowDashDuration = 0.3f;
    public float sDashCooldownTime = 3f;

    private float shadowDashTimeCounter;
    private bool isShadowDashing = false;
    private float lastDashTime = -999f;

    private Rigidbody2D rb;
    private PlayerMovmentScript playerMovement;
    private GameObject shadowDashParticles;
    private HealthAndMana statScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovmentScript>();
        statScript = GetComponent<HealthAndMana>();
        shadowDashParticles = transform.Find("Shadow Dash")?.gameObject;
        if (shadowDashParticles != null) shadowDashParticles.SetActive(false);
    }

    void Update()
    {
        if (isShadowDashing)
        {
            shadowDashTimeCounter -= Time.deltaTime;
            rb.linearVelocity = new Vector2(playerMovement.direction * shadowDashSpeed, 0f);

            if (shadowDashTimeCounter <= 0f)
            {
                isShadowDashing = false;
                if (shadowDashParticles != null) shadowDashParticles.SetActive(false);
            }
        }
    }

    public bool TryTriggerShadowDash()
    {
        if (isShadowDashing || Time.time - lastDashTime < sDashCooldownTime) return false;
        if (playerMovement.isWallJumping || playerMovement.direction == 0) return false;
        if (statScript == null || statScript.currentMana < 13) return false;

        statScript.currentMana -= 13;
        isShadowDashing = true;
        shadowDashTimeCounter = shadowDashDuration;
        lastDashTime = Time.time;

        if (shadowDashParticles != null) shadowDashParticles.SetActive(true);
        rb.linearVelocity = new Vector2(playerMovement.direction * shadowDashSpeed, 0f);
        return true;
    }

    public bool IsShadowDashing()
    {
        return isShadowDashing;
    }
}
