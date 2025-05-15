using UnityEngine;
using TMPro;

public class statueBoss : MonoBehaviour
{

    [Header("Configuration")]
    public float spellCooldown;
    private float cooldownTimer;
    public GameObject spell;
    public GameObject respawnPoint;
    public GameObject originalRespawnPoint;
    public cameraController cam;

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

        // Disable the respawn point
        respawnPoint.SetActive(false);
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
                Vector3 spawnPos1 = new Vector3 (transform.position.x - 2f, transform.position.y + 1.1f, transform.position.z - 3f);
                Instantiate(spell, spawnPos1, Quaternion.identity);
                Vector3 spawnPos2 = new Vector3 (transform.position.x + 2f, transform.position.y + 1.1f, transform.position.z - 3f);
                Instantiate(spell, spawnPos2, Quaternion.identity);
                cooldownTimer = spellCooldown;
            }

            // Otherwise, reduce the timer
            else
            {
                cooldownTimer -= Time.deltaTime;
            }
        }
    }

    public void Activate()
    {
        // Enable AI + remove invincibility
        hp.invincible = false;
        activated = true;
        healthBar.SetMaxHealth(maxHp);
        bossText.text = "WEEPING ANGEL";
        bossHealthBar.Show();
        cam.lockPosition = new Vector3(136.32f, 4.5f, cam.gameObject.transform.position.z);
        cam.xLocked = true;
        cam.yLocked = true;
    }

    public void Deactivate()
    {
        // Disable AI + become invincible
        hp.invincible = true;
        activated = false;
        bossHealthBar.Hide();
        cam.xLocked = false;
        cam.yLocked = false;
    }

    private void OnDestroy()
    {
        if (activated)
        {
            // mods, drop a comical anvil on his head
            healthBar.SetHealth(hp.health);
            bossHealthBar.Hide();
            winText.gameObject.SetActive(true);
            winText.GetComponent<textUtilities>().TitleAppear("ANGEL DEFEATED", 0.3f, 2f, 0.5f);
            Debug.Log("Player has defeated the Weeping Angel!");
            originalRespawnPoint.SetActive(false);
            respawnPoint.SetActive(true);
            respawnPoint.GetComponent<RespawnPoint>().Claim();
            cam.xLocked = false;
            cam.yLocked = false;
        }
    }
}
