using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public GameObject player;
    public float respawnDelay = 2f;

    private HealthAndMana healthAndMana;
    private Vector3 respawnPosition;
    private bool hasRespawned = false;
    private bool isClaimed = false;
    private bool playerTouching = false;
    private bool spellMenuOpen = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            healthAndMana = player.GetComponent<HealthAndMana>();
            respawnPosition = player.transform.position;
        }
    }

    void Update()
    {
        if (player == null || healthAndMana == null) return;

        if (playerTouching && Input.GetKeyDown(KeyCode.F))
        {
            if (!isClaimed)
            {
                isClaimed = true;
                respawnPosition = transform.position;
                Debug.Log("Checkpoint claimed!");
            }
            else
            {
                healthAndMana.currentHealth = healthAndMana.maxHealth;
                healthAndMana.currentMana = healthAndMana.maxMana;
            }
        }

        if (healthAndMana.IsDead() && !hasRespawned)
        {
            hasRespawned = true;
            Invoke(nameof(RespawnPlayer), respawnDelay);
        }
    }

    void RespawnPlayer()
    {
        player.transform.position = new Vector3(respawnPosition.x, respawnPosition.y, player.transform.position.z);
        healthAndMana.currentHealth = healthAndMana.maxHealth;
        healthAndMana.currentMana = healthAndMana.maxMana;

        typeof(HealthAndMana)
            .GetField("isDead", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(healthAndMana, false);

        healthAndMana.deathText.gameObject.SetActive(false);
        Debug.Log("Player respawned!");
        hasRespawned = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerTouching = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerTouching = false;
        }
    }
}
