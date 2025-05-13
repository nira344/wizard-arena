using UnityEngine;

public class healthSteal : MonoBehaviour
{
    public float damage = 1f;              // Amount of health to steal per tick
    public float damageCooldown = 1f;      // Time between each health drain
    private float lastDamageTime = 0f;
    private bool touchingPlayer = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (touchingPlayer && Time.time - lastDamageTime >= damageCooldown)
        {
            HealthAndMana playerHealth = player.GetComponent<HealthAndMana>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);  // Drain HP
                lastDamageTime = Time.time;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = false;
    }

    public bool IsTouchingPlayer()
    {
        return touchingPlayer;
    }
}
