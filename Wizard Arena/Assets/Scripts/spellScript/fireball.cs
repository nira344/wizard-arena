using UnityEngine;

public class fireball : MonoBehaviour
{
    public int projectileSpeed = 15;
    public int damage = 5;
    public float recoilForce = 5f;
    public float castDuration = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerMovmentScript playerMovement;
    private Rigidbody2D playerRb;
    private float originalGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * projectileSpeed;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            animator = player.GetComponent<Animator>();
            playerMovement = player.GetComponent<PlayerMovmentScript>();
            playerRb = player.GetComponent<Rigidbody2D>();

            if (playerMovement != null && playerRb != null)
            {
                playerMovement.isCasting = true;
                playerMovement.canMove = false;

                originalGravity = playerRb.gravityScale;
                playerRb.gravityScale = 0;
                playerRb.linearVelocity = Vector2.zero; // <- Set movement speed to zero

                if (animator != null)
                    animator.SetTrigger("CastFire");

                Vector2 recoilDirection = -transform.right;
                playerRb.linearVelocity = recoilDirection * recoilForce;

                Invoke(nameof(EndCast), castDuration);
            }
        }
    }
    
    void EndCast()
    {
        if (playerMovement != null && playerRb != null)
        {
            playerMovement.isCasting = false;
            playerMovement.canMove = true;
            playerRb.gravityScale = originalGravity;
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
        EndCast();
    }
}
