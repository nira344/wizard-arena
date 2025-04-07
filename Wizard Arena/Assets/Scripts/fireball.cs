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

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Fixing the typo "collison" to "collision"
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
            Debug.Log("Fireball hit non-enemy " + collision.gameObject);
            Destroy(gameObject);  // Destroy the fireball object
            GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
        }

        // Check if the collision is with an object tagged "Enemy"
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Fireball hit enemy " + collision.gameObject);
            var healthComponent = collision.gameObject.GetComponent<enemyHealth>();

            if (healthComponent != null)
            {
                healthComponent.TakeDamage(5);  // Call TakeDamage method correctly
                Destroy(gameObject);  // Destroy the fireball object
                GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
            }
        }
    }
}
