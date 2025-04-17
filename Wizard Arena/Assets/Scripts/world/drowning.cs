using UnityEngine;

public class Drown : MonoBehaviour
{

    private void OnTriggerEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the HealthAndMana component from the player
            HealthAndMana playerHealthMana = collision.gameObject.GetComponent<HealthAndMana>();
            if (playerHealthMana != null)
            {
                // Apply damage to the player's health and mana
                playerHealthMana.TakeDamage(1000);
            }
        }
    }
}