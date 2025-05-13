using UnityEngine;

public class IceShardEvil : MonoBehaviour
{
    [Header("Projectile Settings")]
    public int projectileSpeed = 15;
    public int damage = 2;
    public float homingStrength = 5f;
    public float range = 30f;
    public GameObject explosion;

    private Rigidbody2D rb;
    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        FindTarget();

        // Initial forward movement
        rb.linearVelocity = transform.right * projectileSpeed;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        // Homing logic
        Vector2 newVelocity = Vector2.Lerp(rb.linearVelocity, direction * projectileSpeed, Time.fixedDeltaTime * homingStrength);
        rb.linearVelocity = newVelocity;

        // Rotate to face movement direction
        float angle = Mathf.Atan2(newVelocity.y, newVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist <= range)
            {
                target = player.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("EVIL Ice Shard hit " + collision.gameObject);
            var healthComponent = collision.GetComponent<HealthAndMana>();

            if (healthComponent != null)
                healthComponent.TakeDamage(damage);

            if (explosion != null)
                Instantiate(explosion, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.CompareTag("Obstacle"))
        {
            Debug.Log("EVIL Ice Shard hit solid object");
            if (explosion != null)
                Instantiate(explosion, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
