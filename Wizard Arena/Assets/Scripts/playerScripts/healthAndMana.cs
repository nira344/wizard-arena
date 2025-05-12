using UnityEngine;
using TMPro;

public class HealthAndMana : MonoBehaviour
{
    [Header("Values")]
    public int maxHealth = 10;
    public int maxMana = 20;
    public int currentHealth;
    public int currentMana;

    private bool isDead = false;

    [Header("HUD Elements")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI deathText;

    public HealthBar healthBar;
    public HealthBar manaBar;

    [Header("Debug")]
    public bool debugEnabled;

    void Start()
    {
        maxHealth = PlayerPrefs.GetInt("MaxHealth", maxHealth);
        maxMana = PlayerPrefs.GetInt("MaxMana", maxMana);

        currentHealth = maxHealth;
        currentMana = maxMana;

        if (deathText != null)
            deathText.gameObject.SetActive(false);

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);

        if (manaBar != null)
            manaBar.SetMaxHealth(maxMana);
    }

    void Update()
    {
        BalanceHealthAndMana();

        if (debugEnabled && Input.GetKeyDown("o"))
        {
            TakeDamage(currentHealth);
        }

        if (healthText != null)
            healthText.text = $"HP: {currentHealth}/{maxHealth}";
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (manaText != null)
            manaText.text = $"MP: {currentMana}/{maxMana}";
        if (manaBar != null)
            manaBar.SetHealth(currentMana);

        if (isDead && deathText != null)
        {
            deathText.gameObject.SetActive(true);
            return;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        var movement = GetComponent<PlayerMovmentScript>();
        if (movement != null && movement.isInvincible) return;

        currentHealth -= Mathf.FloorToInt(amount);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;

            if (deathText != null)
                deathText.text = "YOU DIED";
            Debug.Log("Player is dead!");

            if (SoulManager.Instance != null)
            {
                int lostSouls = SoulManager.Instance.soulEssence;
                SoulManager.Instance.soulEssence = 0;
                SoulManager.Instance.UpdateSoulText();

                if (GraveManager.Instance != null)
                {
                    GraveManager.Instance.CreateGrave(transform.position, lostSouls);
                }
            }
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

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Healed for " + amount + " HP. Current HP: " + currentHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);

        PlayerPrefs.SetInt("MaxHealth", maxHealth);
    }

    public void IncreaseMaxMana(int amount)
    {
        maxMana += amount;
        currentMana += amount;

        if (manaBar != null)
            manaBar.SetMaxHealth(maxMana);

        PlayerPrefs.SetInt("MaxMana", maxMana);
    }

    public void DrainMana(float amount)
    {
        currentMana -= Mathf.FloorToInt(amount);
        currentMana = Mathf.Max(currentMana, 0);
        Debug.Log("Mana drained: " + amount + ", Current Mana: " + currentMana);
    }
}
