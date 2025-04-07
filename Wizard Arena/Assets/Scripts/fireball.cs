using UnityEngine;

public class fireball : MonoBehaviour
{
    public int projectileSpeed = 15;
    private Rigidbody2D rb;  // Declare the Rigidbody2D

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component
        rb.linearVelocity = transform.right * projectileSpeed;  // Use velocity to move the fireball
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Fixing the typo "collison" to "collision"
        if (collision.tag != "Projectile")
        {
            Destroy(gameObject);  // Destroy the fireball object
            GetComponent<CircleCollider2D>().enabled = false;  // Disable the collider
        }

        // Check if the collision is with an object tagged "Enemy"
        if (collision.tag == "Enemy")
        {
            var healthComponent = collision.GetComponent<enemyHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(5);  // Call TakeDamage method correctly
            }
        }
    }
}
