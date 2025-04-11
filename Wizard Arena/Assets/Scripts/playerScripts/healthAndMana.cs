using UnityEngine;
using TMPro;

public class HealthAndMana : MonoBehaviour
{
   public int maxHealth = 10;
   public int maxMana = 20;
   public int currentHealth;
   public int currentMana;

   private bool isDead = false;  // Flag to check if the player is dead

   // Text
   public TextMeshProUGUI healthText;
   public TextMeshProUGUI manaText;
   public TextMeshProUGUI deathText;

   // Health Bar
   public HealthBar healthBar;
   public HealthBar manaBar;

    void Start()
    {
       currentHealth = maxHealth;
       currentMana = maxMana;
       deathText.gameObject.SetActive(false); // Hide death text at the start
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
           deathText.gameObject.SetActive(true); // Show death message when player dies
           return;  // Stop further updates if the player is dead
       }
   }
    
   // Add a public method to check if the player is dead
   public bool IsDead()
   {
        return isDead;
   }

   public void TakeDamage(float amount)
   {
       // If the player is already dead, do nothing
       if (isDead) return;

       // Remove health
       currentHealth -= Mathf.FloorToInt(amount);

       // Prevent health from going below zero
       if (currentHealth <= 0)
       {
           currentHealth = 0;
           isDead = true;  // Mark the player as dead
           deathText.text = "YOU DIED"; // Show the death text
           Debug.Log("Player is dead!");
       }
   }

   // Method to balance health and mana
   void BalanceHealthAndMana()
   {
        if (currentMana < 0)
        {
            currentHealth += currentMana;
            currentMana = 0;
        }
   }
}
