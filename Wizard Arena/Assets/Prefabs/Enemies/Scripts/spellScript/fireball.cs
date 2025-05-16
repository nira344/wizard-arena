using UnityEngine;

public class fireball : MonoBehaviour
{
    public int projectileSpeed = 15;
    public int damage = 5;
    public float castDuration = 0.5f;
    public GameObject explosion;

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

                if (animator != null)
                animator.SetTrigger("CastFire");

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

        if (explosion != null)
                Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
