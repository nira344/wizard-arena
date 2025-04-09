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

   void Start()
   {
       currentHealth = maxHealth;
       currentMana = maxMana;
       BalanceHealthAndMana();
       deathText.gameObject.SetActive(false); // Hide death text at the start
   }

   void Update()
   {
       BalanceHealthAndMana();
       healthText.text = "HP: " + currentHealth + "/" + maxHealth;
       manaText.text = "MP: " + currentMana + "/" + maxMana;

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

       // If there's mana, damage the mana first (with 1.5x damage)
       if (currentMana > currentHealth)
       {
           float manaDamage = Mathf.Min(amount * 1.5f, currentMana);
           currentMana -= Mathf.FloorToInt(manaDamage);
       }
       else
       {
           currentHealth -= Mathf.FloorToInt(amount);
           currentMana -= Mathf.FloorToInt(amount);
       }

       // Prevent health from going below zero
       if (currentHealth <= 0)
       {
           currentHealth = 0;
           isDead = true;  // Mark the player as dead
           deathText.text = "YOU DIED"; // Show the death text
           Debug.Log("Player is dead!");
       }

       // Prevent mana from going below zero
       if (currentMana < 0)
       {
           currentMana = 0;
       }

       // Ensure that HP and Mana balance out
       BalanceHealthAndMana();
   }

   // Method to balance health and mana
   void BalanceHealthAndMana()
   {
       if (currentHealth > currentMana) currentHealth = currentMana;
       else if (currentMana < currentHealth) currentMana = currentHealth;
   }
}
