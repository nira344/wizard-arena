using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class enemyAirRanged : MonoBehaviour
{
    [Header("Movement Settings")]
    public float range = 10f;
    public float stopDistance = 5f;
    public float speed = 3f;
    public float hoverVerticalFactor = 0.5f;

    [Header("Audio")]
    public AudioSource flying;

    private GameObject player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (playerInRange())
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > stopDistance)
            {
                Vector2 direction = player.transform.position - transform.position;
                direction.Normalize();
                direction.y *= hoverVerticalFactor;

                direction *= speed * Time.deltaTime;
                transform.Translate(direction);

                // Face the player
                if (player.transform.position.x > transform.position.x)
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                else
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            // Hover bobbing (optional)
            // transform.Translate(new Vector2(0, Mathf.Sin(Time.time * 2f) * 0.5f) * Time.deltaTime);

            // Flying audio
            if (flying && !flying.isPlaying)
            {
                flying.Play();
            }

            // Damp knockback
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime * 2);
        }
    }

    bool playerInRange()
    {
        return Vector2.Distance(transform.position, player.transform.position) <= range;
    }
}
