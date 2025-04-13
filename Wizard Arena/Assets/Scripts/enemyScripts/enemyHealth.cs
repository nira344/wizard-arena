using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public float health = 15f;  // Enemy health
    public int soulReward = 1;

    void Start()
    {
        Debug.Log($"{gameObject.name} spawned with {health} health.");
    }

    void Update()
    {
        // Optional: Debugging current health
        // Debug.Log($"{gameObject.name} current health: {health}");
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Health now: {health}");

        if (health <= 0f)
        {
            Die();  // Properly call the method that adds souls and destroys the enemy
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died. Granting {soulReward} souls.");
        if (SoulManager.Instance != null)
        {
            SoulManager.Instance.AddSouls(soulReward);
        }
        else
        {
            Debug.LogWarning("SoulManager.Instance is null! Souls not added.");
        }

        Destroy(gameObject);
    }
}

