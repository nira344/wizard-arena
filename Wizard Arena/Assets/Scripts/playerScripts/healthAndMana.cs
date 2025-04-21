using UnityEngine;
using TMPro;

public class HealthAndMana : MonoBehaviour
{
    public int maxHealth = 10;
    public int maxMana = 20;
    public int currentHealth;
    public int currentMana;

    private bool isDead = false;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI deathText;

    public HealthBar healthBar;
    public HealthBar manaBar;

    public bool debugEnabled;

    void Start()
    {
        maxHealth = PlayerPrefs.GetInt("MaxHealth", maxHealth);
        maxMana = PlayerPrefs.GetInt("MaxMana", maxMana);

        currentHealth = maxHealth;
        currentMana = maxMana;

        deathText.gameObject.SetActive(false);
        healthBar.SetMaxHealth(maxHealth);
        manaBar.SetMaxHealth(maxMana);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteKey("MaxHealth");
            PlayerPrefs.DeleteKey("MaxMana");
            Debug.Log("PlayerPrefs reset.");
        }

        BalanceHealthAndMana();

        if (debugEnabled && Input.GetKeyDown("o"))
        {
            TakeDamage(currentHealth);
        }

        healthText.text = "HP: " + currentHealth + "/" + maxHealth;
        healthBar.SetHealth(currentHealth);
        manaText.text = "MP: " + currentMana + "/" + maxMana;
        manaBar.SetHealth(currentMana);

        if (isDead)
        {
            deathText.gameObject.SetActive(true);
            return;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        if (GetComponent<PlayerMovmentScript>().isInvincible) return;

        currentHealth -= Mathf.FloorToInt(amount);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            deathText.text = "YOU DIED";
            Debug.Log("Player is dead!");

            int lostSouls = SoulManager.Instance.soulEssence;
            SoulManager.Instance.soulEssence = 0;
            SoulManager.Instance.UpdateSoulText();

            GraveManager.Instance.CreateGrave(transform.position, lostSouls);
        }
    }

    void BalanceHealthAndMana()
    {
        if (currentMana < 0)
        {
            currentHealth += currentMana;
            currentMana = 0;
        }
    }

    public bool IsDead() => isDead;

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        healthBar.SetMaxHealth(maxHealth);
        PlayerPrefs.SetInt("MaxHealth", maxHealth);
    }

    public void IncreaseMaxMana(int amount)
    {
        maxMana += amount;
        currentMana += amount;
        manaBar.SetMaxHealth(maxMana);
        PlayerPrefs.SetInt("MaxMana", maxMana);
    }
}
