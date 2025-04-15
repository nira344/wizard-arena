using UnityEngine;

public class IceShardEvil : MonoBehaviour
{
    public int projectileSpeed = 15;
    public int damage = 2;  // Damage that can be adjusted in the Inspector
    public float homingSpeed = 5f;  // How fast the projectile homes towards the target
    public GameObject explosion;
    private Rigidbody2D rb;
    private Transform target;  // Target for homing (player)
    Vector2 directionToTarget;

    // Start is called once before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get Rigidbody2D component
        FindTarget();  // Find the nearest enemy to home towards
        if (target != null)
        {
            // Rotate towards the target and move the projectile in that direction
            directionToTarget = (target.position - transform.position).normalized;
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

    // Update is called once per frame
    void Update()
    {

    }

    // Find the nearest target (enemy) in the scene
    void FindTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Handle collision with enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("EVIL Ice Shard hit " + collision.gameObject);
            var healthComponent = collision.gameObject.GetComponent<HealthAndMana>();

            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);  // Apply damage to enemy
            }

            //Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);  // Destroy the projectile after impact
        }
        else
        {
            Debug.Log("EVIL Ice Shard hit solid object");
            Destroy(gameObject);  // Destroy the projectile if it hits a solid object
        }
    }
}