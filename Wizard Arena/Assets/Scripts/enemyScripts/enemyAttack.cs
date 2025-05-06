using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 5f;  // Amount of damage the enemy does
    public float manaDamageMultiplier = 1.5f;  // Mana damage multiplier
    public float attackCooldown = 1f;  // Time between each attack
    public bool useTrigger = true;
    public bool useCollider = false;
    private float lastAttackTime = 0f;
    private bool touchingPlayer = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Check if the collision is with an object tagged "Player"
        if (touchingPlayer)
        {
            // cooldown check
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                // Get the HealthAndMana component from the player
                HealthAndMana playerHealthMana = player.GetComponent<HealthAndMana>();
                if (playerHealthMana != null)
                {
                    // Apply damage to the player's health and mana
                    playerHealthMana.TakeDamage(damage);
                    lastAttackTime = Time.time;  // Update the last attack time
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged "Player"
        if (collision.gameObject.CompareTag("Player") && useCollider)
        {
            touchingPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged "Player"
        if (collision.gameObject.CompareTag("Player") && useCollider)
        {
            touchingPlayer = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with an object tagged "Player"
        if (collision.gameObject.CompareTag("Player") && useTrigger)
        {
            touchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the collision is with an object tagged "Player"
        if (collision.gameObject.CompareTag("Player") && useTrigger)
        {
            touchingPlayer = false;
        }
    }

    public bool IsTouchingPlayer()
    {
        return touchingPlayer;
    }
}