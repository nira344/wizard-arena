using UnityEngine;

public class fireball : MonoBehaviour
{
    public int projectileSpeed = 15;
    public int damage = 5;  // Make damage configurable from the Inspector
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

        // Check if the collision is with an object tagged "Enemy"
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Fireball hit enemy " + collision.gameObject);
            var healthComponent = collision.gameObject.GetComponent<enemyHealth>();

            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);  // Use the damage variable
                Destroy(gameObject);  // Destroy the fireball object
                GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
            }
        }
        else
        {
            Debug.Log("Fireball hit non-enemy " + collision.gameObject);
            Destroy(gameObject);  // Destroy the fireball object
            GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
        }
        
    }
}
