using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 5f;  // Amount of damage the enemy does
    public float manaDamageMultiplier = 1.5f;  // Mana damage multiplier
    public float attackCooldown = 1f;  // Time between each attack
    private float lastAttackTime = 0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // cooldown check
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                // Get the HealthAndMana component from the player
                HealthAndMana playerHealthMana = collision.gameObject.GetComponent<HealthAndMana>();
                if (playerHealthMana != null)
                {
                    // Apply damage to the player's health and mana
                    playerHealthMana.TakeDamage(damage);
                    lastAttackTime = Time.time;  // Update the last attack time
                }
            }
        }
    }
}