using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public int projectileSpeed = 15;
    public int damage = 2;
    public float range = 30f;
    public float homingStrength = 5f;
    public GameObject explosion;

    private Rigidbody2D rb;
    private Transform target;
    private Animator animator;
    private PlayerMovmentScript playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Prevent it from falling

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            animator = player.GetComponent<Animator>();
            playerMovement = player.GetComponent<PlayerMovmentScript>();
            if (playerMovement != null && animator != null)
            {
                animator.SetTrigger("CastIce");
            }
        }

        target = FindClosestEnemy();

        // Launch forward (you can override this later in FixedUpdate for homing)
        rb.linearVelocity = transform.right * projectileSpeed;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        // Gradually adjust velocity toward the target
        Vector2 newVelocity = Vector2.Lerp(rb.linearVelocity, direction * projectileSpeed, Time.fixedDeltaTime * homingStrength);
        rb.linearVelocity = newVelocity;

        // Rotate to face movement direction
        float angle = Mathf.Atan2(newVelocity.y, newVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var health = other.GetComponent<enemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            if (explosion != null)
                Instantiate(explosion, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= range)
            {
                minDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest;
    }
}
