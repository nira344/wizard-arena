using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public int projectileSpeed = 10;
    public int damage = 2;
    public GameObject explosionEffect;
    public GameObject miasmaPrefab;

    private Rigidbody2D rb;
    private bool hasExploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * projectileSpeed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return; // Prevent double-triggering
        if (collision.gameObject == gameObject) return; // Don't trigger on itself

        // Optional: prevent hitting shooter if needed (tag shooter as "Enemy" for example)
        if (collision.CompareTag("Enemy")) return;

        hasExploded = true;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Projectile hit player");
            var health = collision.GetComponent<HealthAndMana>();
            if (health != null)
                health.TakeDamage(damage);
        }

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (miasmaPrefab != null)
            Instantiate(miasmaPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
