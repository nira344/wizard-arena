using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public int projectileSpeed = 15;
    public int damage = 2;
    public float range = 30f;
    public GameObject explosion;

    private Rigidbody2D rb;
    private Transform target;
    private PlayerMovmentScript playerMovement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovmentScript>();
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        target = FindClosestEnemy();

        if (playerMovement != null)
        {
            playerMovement.isCasting = true;
            animator.SetTrigger("CastIce");
            Invoke(nameof(EndCast), 0.4f);
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector2 directionToTarget = (target.position - transform.position).normalized;
        rb.linearVelocity = directionToTarget * projectileSpeed;

        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void EndCast()
    {
        if (playerMovement != null)
            playerMovement.isCasting = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
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
