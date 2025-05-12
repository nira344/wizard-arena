using UnityEngine;
using TMPro;

public class wizardBoss : MonoBehaviour
{

    [Header("Configuration")]
    public float speed;
    public float spellCooldown;
    private float cooldownTimer;
    public GameObject spell;

    // Components
    private GameObject player;
    private enemyHealth hp;
    private float maxHp;
    
    // AI status
    private bool activated;

    [Header("HUD Elements")]
    public HealthBar healthBar;
    public bossBar bossHealthBar;
    public TextMeshProUGUI bossText;
    public TextMeshProUGUI winText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get own health script and turn invincible
        hp = GetComponent<enemyHealth>();
        hp.invincible = true;
        maxHp = hp.health;

        // Disable boss HUD elements
        winText.gameObject.SetActive(false);

        // Find player
        player = GameObject.FindGameObjectWithTag("Player");

        // Reset spell cooldown
        cooldownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            // Update boss health bar
            healthBar.SetHealth(hp.health);

            // Fire icicles if cooldown is at zero
            if (cooldownTimer <= 0)
            {
                Instantiate(spell, transform.position, Quaternion.identity);
                cooldownTimer = spellCooldown;
            }

            // Otherwise, reduce the timer
            else
            {
                cooldownTimer -= Time.deltaTime;
            }

            // Move Towards Player
            Vector2 direction = transform.position - player.transform.position;
            direction.Normalize();
            direction.y = 0;
            direction = direction * speed * Time.deltaTime;
            transform.Translate(direction);
        }
    }

    public void Activate()
    {
        // Enable AI + remove invincibility
        hp.invincible = false;
        activated = true;
        healthBar.SetMaxHealth(maxHp);
        bossText.text = "GILBERT THE GREAT";
        bossHealthBar.Show();
    }

    public void Deactivate()
    {
        // Disable AI + become invincible
        hp.invincible = true;
        activated = false;
        bossHealthBar.Hide();
    }

    private void OnDestroy()
    {
        if (activated)
        {
            // mods, drop a comical anvil on his head
            healthBar.SetHealth(hp.health);
            bossHealthBar.Hide();
            winText.gameObject.SetActive(true);
            winText.text = "GILBERT DEFEATED";
            Debug.Log("Player has win!");
            Time.timeScale = 0;
        }
    }
}
