using UnityEngine;

public class fireball : MonoBehaviour
{
    public int projectileSpeed = 15;
    public int damage = 5;
    public float recoilForce = 5f;  // Recoil force to push the player back

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerMovmentScript playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * projectileSpeed;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            animator = player.GetComponent<Animator>();
            playerMovement = player.GetComponent<PlayerMovmentScript>();

            if (playerMovement != null)
            {
                playerMovement.isCasting = true;
                if (animator != null)
                    animator.SetTrigger("CastFire");

                Invoke(nameof(EndCast), 0.5f); // Match this to your animation duration

                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 recoilDirection = -transform.right;
                    playerRb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var healthComponent = collision.gameObject.GetComponent<enemyHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
            }
        }

        Destroy(gameObject);

        if (playerMovement == null)
        {
            playerMovement = FindAnyObjectByType<PlayerMovmentScript>();
        }

        if (playerMovement != null)
        {
            playerMovement.isCasting = false;
        }
    }

    void EndCast()
    {
        if (playerMovement != null)
        {
            playerMovement.isCasting = false;
        }
    }
}
