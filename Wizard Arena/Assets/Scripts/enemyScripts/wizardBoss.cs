using UnityEngine;
using TMPro;

public class wizardBoss : MonoBehaviour
{
    [Header("Configuration")]
    public float spellCooldown = 3f;
    public GameObject normalSpell;
    public GameObject explosiveSpell;
    public float explosiveChance = 0.3f;

    [Header("Floating Settings")]
    public float floatHeight = 5f;
    public float floatSpeed = 2f;
    public float hoverRange = 3f;
    public float hoverSpeed = 2f;
    public float riseDelay = 2f; // Delay before rising

    [Header("Movement")]
    public float movementStartDelay = 1f;

    [Header("HUD Elements")]
    public HealthBar healthBar;
    public bossBar bossHealthBar;
    public TextMeshProUGUI bossText;
    public TextMeshProUGUI winText;

    private GameObject player;
    private enemyHealth hp;
    private float maxHp;
    private float cooldownTimer;

    private bool activated;
    private bool hasReachedHeight;
    private bool hoverStarted;

    private Vector3 hoverCenterPos;
    private float hoverTimer;
    private float riseDelayTimer;

    void Start()
    {
        hp = GetComponent<enemyHealth>();
        // Start invincible
        hp.invincible = true;
        maxHp = hp.health;

        winText.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        cooldownTimer = 0;

        hasReachedHeight = false;
        hoverStarted = false;
        riseDelayTimer = riseDelay;
    }

    void Update()
    {
        if (!activated) return;

        healthBar.SetHealth(hp.health);

        // Wait before rising
        if (riseDelayTimer > 0)
        {
            riseDelayTimer -= Time.deltaTime;
            return;
        }

        // Rising phase
        if (!hasReachedHeight)
        {
            Vector3 targetPos = new Vector3(transform.position.x, floatHeight, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, floatSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.position.y - floatHeight) < 0.05f)
            {
                // We've now finished rising
                hasReachedHeight = true;
                hoverCenterPos = transform.position;
                hoverTimer = -movementStartDelay;

                // NOW remove invincibility
                hp.invincible = false;
            }

            return;
        }

        // Hovering phase
        hoverTimer += Time.deltaTime * hoverSpeed;
        if (hoverTimer >= 0)
        {
            float xOffset = Mathf.Sin(hoverTimer) * hoverRange;
            Vector3 hoverPos = new Vector3(hoverCenterPos.x + xOffset, floatHeight, hoverCenterPos.z);
            transform.position = hoverPos;
        }

        // Spell casting
        if (cooldownTimer <= 0)
        {
            CastSpell();
            cooldownTimer = spellCooldown;
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void CastSpell()
    {
        float roll = Random.value;
        GameObject selectedSpell = (roll < explosiveChance && explosiveSpell != null) ? explosiveSpell : normalSpell;
        Instantiate(selectedSpell, transform.position, Quaternion.identity);
    }

    public void Activate()
    {
        if (activated) return;

        activated = true;
        // Keep invincible here until after rise is complete:
        // hp.invincible = false;  <-- removed from here

        healthBar.SetMaxHealth(maxHp);
        bossText.text = "GILBERT THE GREAT";
        bossHealthBar.Show();

        // Reset delay timer on activation
        riseDelayTimer = riseDelay;
        // Ensure we start rising again (in case of re-activation)
        hasReachedHeight = false;
    }

    public void Deactivate()
    {
        if (!activated) return;

        activated = false;
        hp.invincible = true;
        bossHealthBar.Hide();
    }

    private void OnDestroy()
    {
        if (activated)
        {
            healthBar.SetHealth(hp.health);
            bossHealthBar.Hide();
            winText.gameObject.SetActive(true);
            winText.text = "GILBERT DEFEATED";
            Debug.Log("Player has win!");
            Time.timeScale = 0;
        }
    }
}
