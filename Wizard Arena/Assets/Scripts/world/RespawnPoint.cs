using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public GameObject player;                 // Reference to player object
    public float respawnDelay = 2f;           // Delay before respawn
    private HealthAndMana healthAndMana;      // Player's health script
    private Vector3 respawnPosition;          // Respawn location

    private bool hasRespawned = false;        // To avoid repeating respawn

    void Start()
    {
        if (player != null)
        {
            healthAndMana = player.GetComponent<HealthAndMana>();
            respawnPosition = transform.position;
        }
    }

    void Update()
    {
        if (player == null || healthAndMana == null)
            return;

        // Check if player is dead and hasn't already been respawned
        if (healthAndMana.IsDead() && !hasRespawned)
        {
            hasRespawned = true;
            Invoke(nameof(RespawnPlayer), respawnDelay);
        }
    }

    void RespawnPlayer()
    {
        player.transform.position = respawnPosition;

        // Restore health and mana
        healthAndMana.currentHealth = healthAndMana.maxHealth;
        healthAndMana.currentMana = healthAndMana.maxMana;

        // Reset death status
        typeof(HealthAndMana)
            .GetField("isDead", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(healthAndMana, false);

        // Hide death text
        healthAndMana.deathText.gameObject.SetActive(false);

        Debug.Log("Player respawned!");

        hasRespawned = false;
    }
}
