using UnityEngine;

public class melee : MonoBehaviour
{
    public int damage = 1;  // Configurable from the Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Solid")
        {
            Debug.Log("Hit non-enemy " + collision.gameObject);
            Destroy(gameObject);  // Destroy the object
            GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
        }

        // Check if the collision is with an object tagged "Enemy"
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Hit enemy " + collision.gameObject);
            var healthComponent = collision.gameObject.GetComponent<enemyHealth>();

            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);  // Use the damage variable
                Destroy(gameObject);  // Destroy the object
                GetComponent<PolygonCollider2D>().enabled = false;  // Disable the collider
            }
        }
    }
}
