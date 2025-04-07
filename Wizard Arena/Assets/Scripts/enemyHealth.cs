using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public float health = 15f;  // Enemy health

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to reduce health when taking damage
    public void TakeDamage(float amount)  // Fixed method name to match the call
    {
        health -= amount;
        Debug.Log("Enemy damage applied");

        // If health drops to zero or below, destroy the enemy
        if (health <= 0f)
        {
            Debug.Log("Enemy slain");
            Destroy(gameObject);  // Destroy the enemy object
        }
    }
}
