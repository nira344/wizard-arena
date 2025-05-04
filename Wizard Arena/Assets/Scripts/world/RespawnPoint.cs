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
                if (!spellMenuOpen)
                {
                    healthAndMana.currentHealth = healthAndMana.maxHealth;
                    healthAndMana.currentMana = healthAndMana.maxMana;
                    SpellMenuManager.Instance.OpenMenu();
                    Time.timeScale = 0f;
                    spellMenuOpen = true;
                    Debug.Log("Healed and spell menu opened.");
                }
                else
                {
                    SpellMenuManager.Instance.CloseMenu();
                    Time.timeScale = 1f;
                    spellMenuOpen = false;
                    Debug.Log("Spell menu closed.");
                }
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

            // Auto-close menu if still open
            if (spellMenuOpen)
            {
                SpellMenuManager.Instance.CloseMenu();
                Time.timeScale = 1f;
                spellMenuOpen = false;
                Debug.Log("Spell menu auto-closed on exit.");
            }
        }
    }
}
