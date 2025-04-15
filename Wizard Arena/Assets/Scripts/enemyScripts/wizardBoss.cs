using UnityEngine;

public class wizardBoss : MonoBehaviour
{

    public float speed;
    public float spellCooldown;
    public float cooldownTimer;
    public GameObject spell;

    private GameObject player;
    private enemyHealth hp;
    private bool activated;

    public HealthBar healthBar;
    public bossBar bossHealthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hp = gameObject.GetComponent<enemyHealth>();
        cooldownTimer = 0;
        healthBar.SetMaxHealth(hp.health);
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
        activated = true;
        bossHealthBar.Show();
    }

    public void Deactivate()
    {
        activated = false;
        bossHealthBar.Hide();
    }

    private void OnDestroy()
    {
        healthBar.SetHealth(hp.health);
        bossHealthBar.Hide();
    }
}
