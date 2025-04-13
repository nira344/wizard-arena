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

    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        deathText.gameObject.SetActive(false);
        healthBar.SetMaxHealth(maxHealth);
        manaBar.SetMaxHealth(maxMana);
    }

    void Update()
    {
        BalanceHealthAndMana();
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

    public bool IsDead() => isDead;

    public void TakeDamage(float amount)
    {
        if (isDead) return;

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
}
