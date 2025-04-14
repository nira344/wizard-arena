using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public int projectileSpeed = 15;
    public int damage = 2;  // Damage that can be adjusted in the Inspector
    public float homingSpeed = 5f;  // How fast the projectile homes towards the target
    public GameObject explosion;
    private Rigidbody2D rb;
    private Transform target;  // Target for homing (enemy)

    // Start is called once before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get Rigidbody2D component
        FindTarget();  // Find the nearest enemy to home towards
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Rotate towards the target and move the projectile in that direction
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            rb.linearVelocity = directionToTarget * projectileSpeed;

            // Rotate the projectile to face the target
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            // If no target, move in the current direction
            rb.linearVelocity = transform.right * projectileSpeed;
        }
    }

    // Find the nearest target (enemy) in the scene
    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        float shortestDistance = Mathf.Infinity;  // Initial very large distance
        GameObject nearestEnemy = null;

        // Loop through all enemies and find the nearest one
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // Set the target to the nearest enemy, if found
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
    }

    // Handle collision with enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("iceShard hit " + collision.gameObject);
            var healthComponent = collision.gameObject.GetComponent<enemyHealth>();

            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);  // Apply damage to enemy
            }

            //Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);  // Destroy the projectile after impact
        }

        if (collision.gameObject.CompareTag("Solid"))
        {
            Debug.Log("Homing projectile hit solid object");
            Destroy(gameObject);  // Destroy the projectile if it hits a solid object
        }
    }
}